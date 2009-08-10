using System;
using System.Collections.Generic;
using System.Text;

namespace netGui
{
    public class Options
    {
        public string portname = "COM1";
        public Options CopyOf
        {
            get
            {
                Options newOptions = new Options();
                newOptions.portname = this.portname;
                return newOptions;
            }
        }
    }
}
