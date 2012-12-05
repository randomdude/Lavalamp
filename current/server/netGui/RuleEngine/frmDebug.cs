namespace netGui.RuleEngine
{
    using System;
    using System.Windows.Forms;

    using ruleEngine;
    using ruleEngine.ruleItems;

    public partial class frmDebug : Form
    {
        public frmDebug(lineChain debugThis)
        {
            this.InitializeComponent();

            if (debugThis.serial != null)
                this.lblDebugInfo.Text += "ID = " + debugThis.serial.ToString() + Environment.NewLine;
            else
                this.lblDebugInfo.Text += "ID is null!" + Environment.NewLine;
            this.lblDebugInfo.Text += "Color = " + debugThis.col.ToString() + Environment.NewLine;
            this.lblDebugInfo.Text += "Deleted = " + debugThis.isDeleted.ToString() + Environment.NewLine;
            /*
            if (debugThis.sourcePin!= null)
                this.lblDebugInfo.Text += "Src.  Pin name = " + debugThis.sourcePin.ToString() + Environment.NewLine;
            else
                this.lblDebugInfo.Text += "Src.  Pin id is null!" + Environment.NewLine;

            if (debugThis.destPin != null)
                this.lblDebugInfo.Text += "Dst.  pin id = " + debugThis.destPin.ToString() + Environment.NewLine;
            else
                this.lblDebugInfo.Text += "Dst.  pin id is null!" + Environment.NewLine;
             * */
        }

        public frmDebug(ruleItemBase debugThis)
        {
            this.InitializeComponent();

            if (debugThis.serial == null)
                this.lblDebugInfo.Text += "ID is null!" + Environment.NewLine;
            else
                this.lblDebugInfo.Text += "ID = " + debugThis.serial.ToString() + Environment.NewLine;

            lblDebugInfo.Text += "pins: " + Environment.NewLine;
            int i = 1; 
            foreach (var entry in debugThis.pinInfo)
            {
                var pin = entry.Value;
                lblDebugInfo.Text += string.Format("{0} {5}: {1},{4}{6}description, {2}{4}{6}data-type, {3}{4}{6}current value, {7}{4}", 
                    pin.direction, entry.Key, pin.description, pin.valueType, Environment.NewLine,i++,"       ",pin.value.data);
            }
            

        }
    }
}
