using System;

namespace netGui
{
    public class options
    {
        public string rulesPath;
        public string portname;
        public key myKey = new key();

        public options()
        {
            portname = Properties.Settings.Default["portname"] as string;
            try
            {
                myKey.setKey(Properties.Settings.Default["key"] as string);
            } catch (FormatException)
            {
                myKey.setKey("00112233445566778899aabbccddeeff");                
            }
            rulesPath = Properties.Settings.Default["rulesPath"] as string;
        }

        public options(options newOptions) 
        {
            portname = newOptions.portname;
            myKey = newOptions.myKey;
            rulesPath = newOptions.rulesPath;
        }

        public void save()
        {
            Properties.Settings.Default["rulesPath"] = rulesPath;
            Properties.Settings.Default["portname"] = portname;
            Properties.Settings.Default["key"] = myKey.ToString();
            Properties.Settings.Default.Save();
        }
    }
}
