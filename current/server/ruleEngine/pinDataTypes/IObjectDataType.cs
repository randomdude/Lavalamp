// -----------------------------------------------------------------------
// <copyright file="objectPinBaseDataType.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ruleEngine.pinDataTypes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    using ruleEngine.ruleItems;

    /// <summary>
    /// 
    /// </summary>
    public interface IObjectDataType
    {
        string serialize();
        string TypeName { get; set; }
        string AssemblyName { get; set; }
        bool asBoolean();
        void setToDefault();

    }
}
