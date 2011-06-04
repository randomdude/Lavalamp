using System;
using System.Windows.Forms;

namespace virtualNodeNetwork
{
    public abstract class ctlNodeSensorWidget : UserControl
    {
        public abstract void updateValue(int newValue);
        public event Action<int> onInputChanged;

        /// <summary>
        /// Fire the onInputChanged event
        /// </summary>
        /// <param name="newVal"></param>
        protected void inputChanged(int newVal)
        {
            onInputChanged.Invoke(newVal);
        }
    }
}