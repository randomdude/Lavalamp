using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Timers;
using System.Windows.Forms;
using ruleEngine.ruleItems.windows;
using Timer = System.Windows.Forms.Timer;

namespace ruleEngine.ruleItems
{
    public enum position
    {
        middle, top, bottom, fill
    }
    public enum colour
    {
        red, green, amber, rainbow1, rainbow2, mix, auto
    }
    public enum specialStyle
    {
        twinkle, sparkle, snow, interlock, style_switch, slide, spray, starburst, welcome, slot_machine,
        none
    }
    public enum mode
    {
        rotate, hold, flash, mode_reserved_1, roll_up, roll_down, roll_left, roll_right, wipe_up, wipe_down, wipe_left, wipe_right,
        scroll, mode_reserved_2, random_mode, roll_in, roll_out, wipe_in, wipe_out, compressed_rotate
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
        private wallboardOptions _options = new wallboardOptions{   colour = colour.auto,
                                                                    mode = mode.hold,position = position.fill,
                                                                    specialStyle = specialStyle.none,
                                                                    state = wallboardErrorState.Unknown
                                                                };

        private bool _canBeChangedNow = true;

        public ruleItemWallboard()
        {
            controls.Add(new Label(){Text = "Wallboard"});
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

        public override ContextMenuStrip addMenus(ContextMenuStrip strip1)
        {
            var menu = strip1;
            menu.Items.Add("Options", null, delegate
                                                { 
                                                   ruleItemOptions().ShowDialog();
                                                });
            return base.addMenus(strip1);
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
        public static bool testWallboardConnectivity(string port)
        {

            IntPtr portHwd = IntPtr.Zero;
            wallboardErrorState state;
            try
            {
                portHwd = connectWallboard(port);
                if (portHwd == IntPtr.Zero)
                    return false;
                state = checkWallboardErrorState(portHwd);
            }
            finally
            {
                friendlyClosePort(ref portHwd);
            }
            return state == wallboardErrorState.None;
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
        private static extern void sendWallMessage(IntPtr port, [MarshalAs(UnmanagedType.LPStr)] String sayit, char pos, UInt32 style, char col, char special, bool dumppkt);

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
            try
            {
                portHwd = connectWallboard(_options.port);
                sendWallMessage(portHwd, sayit, posToMagic(pos), modeToMagic(useMode), colourToMagic(col), styleToMagic(useStyle), false);
                
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

        
        private char modeToMagic(mode toFind)
        {
            switch (toFind)
            {
                case (mode.rotate):
                    return (char)0x0;
                case (mode.hold):
                    return (char)0x1;
                case (mode.flash):
                    return (char)0x2;
                case (mode.mode_reserved_1):
                    return (char)0x3;
                case (mode.roll_up):
                    return (char)0x4;
                case (mode.roll_down):
                    return (char)0x5;
                case (mode.roll_left):
                    return (char)0x6;
                case (mode.roll_right):
                    return (char)0x7;
                case (mode.wipe_up):
                    return (char)0x8;
                case (mode.wipe_down):
                    return (char)0x9;
                case (mode.wipe_left):
                    return (char)0xa;
                case (mode.wipe_right):
                    return (char)0xb;

                case (mode.scroll):
                    return (char)0xc;
                case (mode.mode_reserved_2):
                    return (char)0xd;
                case (mode.random_mode):
                    return (char)0xe;
                case (mode.roll_in):
                    return (char)0xf;
                case (mode.roll_out):
                    return (char)0x10;
                case (mode.wipe_in):
                    return (char)0x11;
                case (mode.wipe_out):
                    return (char)0x12;
                case (mode.compressed_rotate):
                    return (char)0x13;

            }
            throw new wallboardException("unknown mode");
        }

        private char styleToMagic(specialStyle toFind)
        {
            // todo: there must be a better way to do this, with Enum...
            switch (toFind)
            {
                case (specialStyle.twinkle):
                    return (char)0x30;
                case (specialStyle.sparkle):
                    return (char)0x31;
                case (specialStyle.snow):
                    return (char)0x32;
                case (specialStyle.interlock):
                    return (char)0x33;
                case (specialStyle.style_switch):
                    return (char)0x34;
                case (specialStyle.slide):
                    return (char)0x35;
                case (specialStyle.spray):
                    return (char)0x36;
                case (specialStyle.starburst):
                    return (char)0x37;
                case (specialStyle.welcome):
                    return (char)0x38;
                case (specialStyle.slot_machine):
                    return (char)0x39;
                case (specialStyle.none):
                    return (char)0xff;
            }
            throw new wallboardException("unknown specialStyle");
        }

        private char colourToMagic(colour toFind)
        {
            switch (toFind)
            {
                case (colour.red):
                    return '1';
                case (colour.green):
                    return '2';
                case (colour.amber):
                    return '3';
                case (colour.rainbow1):
                    return '9';
                case (colour.rainbow2):
                    return 'A';
                case (colour.mix):
                    return 'B';
                case (colour.auto):
                    return 'C';
            }
            throw new wallboardException("unknown colour");
        }

        private char posToMagic(position toFind)
        {
            switch (toFind)
            {
                case (position.top):
                    return (char)0x22;
                case (position.middle):
                    return (char)0x20;
                case (position.bottom):
                    return (char)0x26;
                case (position.fill):
                    return (char)0x30;
            }
            throw new wallboardException("unknown position");
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
