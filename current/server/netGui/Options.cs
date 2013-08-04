using System;

namespace netGui
{
    using System.IO;

    using lavalamp;

    using ruleEngine;
    using ruleEngine.nodes;

    public class options
    {
        public bool useEncryption;
        public string rulesPath;
        public string portname;
        public key myKey = new key();
        public IRuleRepository RuleRepository { get; set; }


        public options()
        {

            if (!bool.TryParse(Properties.Settings.Default["useEncryption"] as string, out useEncryption))
                useEncryption = false;
            portname = Properties.Settings.Default["portName"] as string;
            try
            {
                myKey.setKey(Properties.Settings.Default["key"] as string);
            } 
            catch (FormatException)
            {
                myKey.setKey("00112233445566778899aabbccddeeff");                
            }
            rulesPath = Properties.Settings.Default["rulesPath"] as string;
            Uri path;
            if (Uri.TryCreate(rulesPath, UriKind.RelativeOrAbsolute,out path))
            {
                if (path.Scheme == Uri.UriSchemeHttp || path.Scheme ==  Uri.UriSchemeHttps)
                    RuleRepository = new serviceRuleRepository(rulesPath);
                else 
                    RuleRepository = new ruleRepository(rulesPath);

            }
        }

        public options(options newOptions) 
        {
            useEncryption = newOptions.useEncryption;
            portname = newOptions.portname;
            myKey = newOptions.myKey;
            rulesPath = newOptions.rulesPath;
            Uri path;
            if (Uri.TryCreate(rulesPath, UriKind.RelativeOrAbsolute, out path))
            {
                if (path.Scheme == Uri.UriSchemeHttp || path.Scheme == Uri.UriSchemeHttps)
                    RuleRepository = new serviceRuleRepository(rulesPath);
                else
                    RuleRepository = new ruleRepository(rulesPath);

            }
        }

        public void save()
        {
            Properties.Settings.Default["useEncryption"] = useEncryption.ToString();
            Properties.Settings.Default["rulesPath"] = rulesPath;
            Properties.Settings.Default["portName"] = portname;
            Properties.Settings.Default["key"] = myKey.ToString();
            Properties.Settings.Default.Save();
        }
    }
}
