using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace netGui
{
    static class Program
    {
        static public Options MyOptions = new Options();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmMain());
        }
    }
}