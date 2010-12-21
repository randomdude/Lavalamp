using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using netGui.Properties;

namespace netGui.RuleEngine.ruleItems.Starts
{
    [RuleEngine.ToolboxRule]
    [ToolboxRuleCategory("unfinished stuff")]
    public class ruleItem_shutdown : ruleItemBase
    {
        private Label lblCaption;
        private bool lastInput;

        // This'll only work under win32.

        // TODO: FINISH THIS

        public override string ruleName() { return "Shut down the system"; }

        public ruleItem_shutdown()
        {
            //Dictionary<String, pin> pinInfo = getPinInfo();
            //pinStates.pinInfo = getPinInfo();

            //foreach (String pinName in pinInfo.Keys)
            //    this.pinStates.Add(pinName, false);

            lblCaption = new Label();
            lblCaption.AutoSize = false;
            lblCaption.Width = preferredSize().Width;
            lblCaption.Height = 20;
            lblCaption.Left = 1;
            lblCaption.Top = preferredSize().Height - lblCaption.Height;
            lblCaption.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lblCaption.Visible = true;
            lblCaption.Text = "Shut down the system";
            controls.Add(lblCaption);

        }

        public override ContextMenuStrip addMenus(ContextMenuStrip strip1)
        {
            ContextMenuStrip toRet = base.addMenus( strip1 );

            while (strip1.Items.Count > 0)
                toRet.Items.Add(strip1.Items[0]);

            toRet.Items.Add(new ToolStripSeparator());

            ToolStripMenuItem newItem = new ToolStripMenuItem("Force shutdown");
            toRet.Items.Add(newItem);

            return toRet;
        }

        public override System.Drawing.Image background()
        {
            return Resources.Keys;
        }

        public override Dictionary<String, pin> getPinInfo()
        {
            Dictionary<String, pin> pinList = new Dictionary<string, pin>();
            pinList.Add("shutdownNow", new pin { name = "shutdownNow", description = "trigger to shutdown", direction = pinDirection.input });

            return pinList;
        }

        public override void evaluate()
        {
            bool input1 = (bool)pinInfo["shutdownNow"].value.getData();

            if (input1 && !lastInput)
            {
                //todo - error checking on API calls
                shutdownStuff.TokPriv1Luid tp;
                IntPtr hproc = shutdownStuff.GetCurrentProcess();
                IntPtr htok = IntPtr.Zero;
                shutdownStuff.OpenProcessToken(hproc, shutdownStuff.TOKEN_ADJUST_PRIVILEGES | shutdownStuff.TOKEN_QUERY, ref htok);
                tp.Count = 1;
                tp.Luid = 0;
                tp.Attr = shutdownStuff.SE_PRIVILEGE_ENABLED;
                shutdownStuff.LookupPrivilegeValue(null, shutdownStuff.SE_SHUTDOWN_NAMETEXT, ref tp.Luid);
                shutdownStuff.AdjustTokenPrivileges(htok, false, ref tp, 0, IntPtr.Zero, IntPtr.Zero);

                shutdownStuff.ExitWindowsEx((uint)shutdownStuff.ExitWindows.PowerOff, 0x00);
            }

            lastInput = input1;        
        }
    }

    public class shutdownStuff
    {
        // See http://pinvoke.net/default.aspx/advapi32/AdjustTokenPrivileges.html
        [DllImport("advapi32.dll", ExactSpelling = true, SetLastError = true)]
        internal static extern bool AdjustTokenPrivileges(IntPtr htok, bool disall,
        ref TokPriv1Luid newst, int len, IntPtr prev, IntPtr relen);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        internal static extern IntPtr GetCurrentProcess();

        [DllImport("advapi32.dll", ExactSpelling = true, SetLastError = true)]
        internal static extern bool OpenProcessToken(IntPtr h, int acc, ref IntPtr
        phtok);

        [DllImport("advapi32.dll", SetLastError = true)]
        internal static extern bool LookupPrivilegeValue(string host, string name,
        ref long pluid);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool ExitWindowsEx(uint uFlags, uint dwReason);

        [Flags]
        public enum ExitWindows : uint
        {
            // ONE of the following five:
            LogOff = 0x00,
            ShutDown = 0x01,
            Reboot = 0x02,
            PowerOff = 0x08,
            RestartApps = 0x40,
            // plus AT MOST ONE of the following two:
            Force = 0x04,
            ForceIfHung = 0x10,
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct TokPriv1Luid
        {
            public int Count;
            public long Luid;
            public int Attr;
        }

        internal const int SE_PRIVILEGE_ENABLED = 0x00000002;
        internal const int TOKEN_QUERY = 0x00000008;
        internal const int TOKEN_ADJUST_PRIVILEGES = 0x00000020;
        internal const string SE_SHUTDOWN_NAMETEXT = "SE_SHUTDOWN_NAME"; //http://msdn.microsoft.com/en-us/library/bb530716(VS.85).aspx

    }

}