﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Timers;
using System.Windows.Forms;
using System.Xml.Serialization;
using ruleEngine.ruleItems.windows;
using Timer = System.Windows.Forms.Timer;

namespace ruleEngine.ruleItems
{
    /// <summary>
    /// position the text appears on the wallboard contrary to the documentation fill appears to be 0x20 and middle as 0x30
    /// on our model of the wallboard anyway..
    /// </summary>
    public enum position
    {
        middle = 0x30, top = 0x22, bottom = 0x26, fill = 0x20
    }
    public enum colour
    {
        red = 0x31, green = 0x32, amber = 0x33, rainbow1 = 0x39, rainbow2 = 0x41, mix = 0x42, auto = 0x43
    }
    public enum specialStyle
    {
        twinkle = 0x30, sparkle = 0x31, snow = 0x32, interlock = 0x33, style_switch = 0x34,
        slide = 0x35, spray = 0x36, starburst = 0x37, welcome = 0x38, slot_machine = 0x39,
        thankyou = 0x53, no_smoking = 0x55, dont_drink_and_drive = 0x56, running_animal = 0x57,
        fireworks = 0x58, turbocar = 0x59, cherry_bomb = 0x5A,
        none = 0xFF
    }
    public enum mode
    {
        rotate = 0x61, hold = 0x62, flash = 0x63, mode_reserved_1 = 0x64, roll_up = 0x65, roll_down = 0x66, roll_left = 0x67, roll_right = 0x68, wipe_up = 0x69, wipe_down = 0x6A, wipe_left =0x6B, wipe_right =0x6C,
        scroll = 0x6D, random_mode = 0x6F, roll_in = 0x70, roll_out = 0x71, wipe_in = 0x72, wipe_out = 0x73, compressed_rotate = 0x74
    }
    [ToolboxRule]
    [ToolboxRuleCategory("Notifiers")]
    public class ruleItemWallboard : ruleItemBase
    {
        private Timer _timeSincePrevChange = new Timer();
        private Queue<string> _updatesTodo = new Queue<string>(); 
        private string _lastVal;
        /// <summary>
        /// the wallboard options created with defaults which will be overwritten if deserialized
        /// </summary>
        [XmlElement("options")]
        public wallboardOptions _options = new wallboardOptions{   colour = colour.auto,
                                                                    mode = mode.hold,position = position.fill,
                                                                    specialStyle = specialStyle.none,
                                                                    state = wallboardErrorState.Unknown
                                                                };

        private bool _canBeChangedNow = true;

        public ruleItemWallboard()
        {
            controls.Add(new Label() { Text = "Wallboard", Size = new Size(73, 37), Location = new Point(3, 39), TextAlign = ContentAlignment.MiddleCenter });
            _timeSincePrevChange.Tick += new EventHandler(_timeSincePrevChange_Tick);
        }

        void _timeSincePrevChange_Tick(object sender, EventArgs e)
        {
            
            _canBeChangedNow = true;
        }

        public override string ruleName()
        {
            return "Wallboard Notifier";
        }

        public override Dictionary<string, pin> getPinInfo()
        {
            var pins = base.getPinInfo();
            pins.Add("input",new pin(){name = "input",description = "Display on Wallboard",direction = pinDirection.input, valueType = typeof(pinDataTypes.pinDataString)});
            return pins;
        }

        public override void evaluate()
        {
            string newValToShow = pinInfo["input"].value.ToString();
            if (newValToShow != _lastVal || _updatesTodo.Count > 0)
            {
                if (!_canBeChangedNow && newValToShow != _lastVal)
                {
                    _updatesTodo.Enqueue(newValToShow);
                    _lastVal = pinInfo["input"].value.ToString();
                    return;
                }
                if (_updatesTodo.Count != 0)
                {
                    if(newValToShow != _lastVal)
                        _updatesTodo.Enqueue(newValToShow);
                    newValToShow = _updatesTodo.Dequeue();
                }
                else // we only want to update the last value if it hasn't been queued already
                    _lastVal = newValToShow;
                
                friendlySendWallMessage(newValToShow, _options.position, _options.mode,
                                        _options.colour,
                                        _options.specialStyle);

                if (_options.timeBeforeCanBeChanged > 0)
                {
                    _canBeChangedNow = false;
                    _timeSincePrevChange.Interval = _options.timeBeforeCanBeChanged;
                    _timeSincePrevChange.Start();
                
                }
            }
            
        }

        public override Form ruleItemOptions()
        {
            frmWallboardOptions frm = new frmWallboardOptions(_options);
            frm.Closed += frm_Closed;
            return frm;
        }

        void frm_Closed(object sender, EventArgs e)
        {
            frmWallboardOptions frm = (frmWallboardOptions) sender;
            if (frm.DialogResult == DialogResult.OK)
                _options = frm.getChosenOptions();
        }

        [Pure]
        public static wallboardErrorState testWallboardConnectivity(string port, position testPos, mode testMode, colour testCol, specialStyle testSpecial)
        {
            if (string.IsNullOrEmpty(port))
                throw new NullReferenceException("No Port set for wallboard");
            IntPtr portHwd = IntPtr.Zero;
            wallboardErrorState state;
            try
            {
                portHwd = connectWallboard(port);
                if (portHwd == IntPtr.Zero)
                    return wallboardErrorState.Unknown;
                state = checkWallboardErrorState(portHwd);
                if (state == wallboardErrorState.None)
                {
                    sendWallMessage(portHwd, "Hello World", testPos, testMode, testCol, testSpecial, false);
                }
            }
            finally
            {
                friendlyClosePort(ref portHwd);
            }
            return state;
        }

        public static void resetWallboard(string port)
        {
            if (string.IsNullOrEmpty(port))
                throw new NullReferenceException("No Port set for wallboard");
            IntPtr portHwd = IntPtr.Zero;
            try
            {
                portHwd = connectWallboard(port);
                if (portHwd == IntPtr.Zero)
                    return;
                resetWallboard(portHwd);
            }
            finally
            {
                friendlyClosePort(ref portHwd);
            }
        }

        #region "Wallboard interop"
        public enum wallboardErrorState
        {
            IllegalCommand = 0x6,
            ChecksumError = 0x5,
            BufferOverflow = 0x4,
            SerialTimeout = 0x3,
            BaudrateError = 0x2,
            ParityError = 0x1,
            None = 0,
            Unknown = -1
        }

        [DllImport(@"wallboard.dll")]
        private static extern IntPtr connectWallboard([MarshalAs(UnmanagedType.LPStr)] string portname);

        [DllImport(@"wallboard.dll")]
        private static extern void sendWallMessage(IntPtr port, [MarshalAs(UnmanagedType.LPStr)] String sayit, position pos, mode style,  colour col, specialStyle special, bool dumppkt);

        [DllImport(@"wallboard.dll")]
        private static extern void closeWallboard(IntPtr port);

         [DllImport(@"wallboard.dll")]
        private static extern void resetWallboard(IntPtr port);

        [DllImport(@"wallboard.dll")]
        private static extern wallboardErrorState checkWallboardErrorState(IntPtr port);

        private static void friendlyClosePort(ref IntPtr port)
        {
            if (port != IntPtr.Zero)
                closeWallboard(port);
            port = IntPtr.Zero;
        }

        private void friendlySendWallMessage(String sayit, position pos, mode useMode, colour col, specialStyle useStyle)
        {
            IntPtr portHwd = IntPtr.Zero;
            if (string.IsNullOrEmpty(_options.port))
                throw new NullReferenceException("No Port set for wallboard");
            try
            {
                portHwd = connectWallboard(_options.port);
                sendWallMessage(portHwd, sayit, pos, useMode, col, useStyle, false);
                
            }
            catch(wallboardException ex)
            {
                errorHandler(ex);
            }
            catch (Exception ex)
            {
                _options.state = wallboardErrorState.Unknown;
                //attempt to get additional info about the error if the handle was successfully opened
                if (portHwd != IntPtr.Zero)
                    _options.state = checkWallboardErrorState(portHwd);
                errorHandler(new wallboardException("an exception occurred while sending " + sayit + " to the wallboard", _options.state, ex));
            }
            finally
            {
                friendlyClosePort(ref portHwd);
            }
            
        }
    #endregion

    }

    internal class wallboardException : Exception
    {
        private readonly ruleItemWallboard.wallboardErrorState _errorCode;
        public wallboardException(string exeception) : base(exeception) { }
        public wallboardException(string exception, ruleItemWallboard.wallboardErrorState errorCode, Exception innerException) :base(exception,innerException)
        {
            _errorCode = errorCode;
        }
        public override string Message
        {
            get
            {
                return base.Message + " Wallboard Error: " + _errorCode.ToString();
            }
        }
    }

    [Serializable]
    public class wallboardOptions
    {
        public int timeBeforeCanBeChanged { get; set; }

        public string port{ get; set; }

        public mode mode { get; set; }

        public colour colour { get; set; }

        public specialStyle specialStyle {get; set; }

        public position position{ get; set; }

        public ruleItemWallboard.wallboardErrorState state { get; set; }
    }
}
