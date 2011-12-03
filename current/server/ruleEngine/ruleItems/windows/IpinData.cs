using System;
using System.Drawing;

namespace ruleEngine.ruleItems.windows
{
    public interface IpinData
    {
        void setToDefault();
        string ToString();
        Color getColour();
        object data { get; set; }
        object noValue { get; }
        bool asBoolean();
        IpinData and(IpinData data);
        IpinData or(IpinData data);
        IpinData xor(IpinData data);
        IpinData not();
        Type getDataType();
    }
}