using System.Xml.Serialization;

namespace ruleEngine.ruleItems
{
    using System;

    public interface IFormOptions
    {
        /// <summary>
        /// Used for the options title, display name on the client.
        /// </summary>
        [XmlIgnore]
        string displayName { get; }

        /// <summary>
        /// This is used to create a type on the client for editing 
        /// the options it shouldn't be changed,
        /// if it is the client code will also need to be changed to reflect this 
        /// recommended pattern is the ruleItems name i.e. 'Rss' or 'CheckEmail'
        /// </summary>
        [XmlIgnore]
        string typedName { get; }

        void setChanged();
        event EventHandler optionsChanged;
    }
}
