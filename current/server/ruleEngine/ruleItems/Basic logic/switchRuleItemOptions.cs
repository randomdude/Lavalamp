using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ruleEngine.ruleItems.Basic_logic
{
    using System.Xml.Serialization;

    [Serializable]
    public class switchRuleItemOptions : BaseOptions
    {
        public override string displayName
        {
            get
            {
                return "Switch Options";
            }
        }

        public override string typedName
        {
            get
            {
                return "Switch";
            }
        }
        [XmlElement]
        /// <summary>
        /// Does the True Pin appear for the user to hook to
        /// </summary>
        public bool UseTruePin
        {
            get; set;
        }

        [XmlElement]
        /// <summary>
        /// Does the False Pin appear for the user to hook to
        /// </summary>
        public bool UseFalsePin
        {
            get;set;
        }
        [XmlElement]
        /// <summary>
        /// If UseTruePin is false this will be used when the switch is set to true
        /// </summary>
        public object TrueValue {
            get;set;
        }
        [XmlElement]
        /// <summary>
        /// If UseFalsePin is false this will be used when the switch is set to true
        /// </summary>
        public object FalseValue {
            get;set;
        }
        [XmlElement]
        public Type DataTypeFalse { get; set; }

        [XmlElement]
        public Type DataTypeTrue { get; set; }

        public switchRuleItemOptions() { }

        public switchRuleItemOptions(switchRuleItemOptions options)
        {
            this.FalseValue = options.FalseValue;
            TrueValue = options.TrueValue;
            UseFalsePin = options.UseFalsePin;
            UseTruePin = options.UseTruePin;
        }
    }
}
