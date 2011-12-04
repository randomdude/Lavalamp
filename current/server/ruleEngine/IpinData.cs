using System;
using System.Drawing;

namespace ruleEngine
{
    public interface IPinData
    {
        void setToDefault();
        string ToString();
        Color getColour();
        object data { get; set; }
        object noValue { get; }
        bool asBoolean();
        IPinData and(IPinData data);
        IPinData or(IPinData data);
        IPinData xor(IPinData data);
        IPinData not();
        Type getDataType();
        void performUpdate();
        void reevaluate();
    }
}