using System;
using System.Windows.Forms;
using ruleEngine.ruleItems;

namespace ruleEngine
{
    public partial class frmDebug : Form
    {
        public frmDebug(lineChain debugThis)
        {
            InitializeComponent();

            if (debugThis.serial != null)
                this.lblDebugInfo.Text += "ID = " + debugThis.serial.ToString() + Environment.NewLine;
            else
                this.lblDebugInfo.Text += "ID is null!" + Environment.NewLine;
            this.lblDebugInfo.Text += "Color = " + debugThis.col.ToString() + Environment.NewLine;
            this.lblDebugInfo.Text += "Deleted = " + debugThis.deleted.ToString() + Environment.NewLine;
            if (debugThis.sourcePin!= null)
                this.lblDebugInfo.Text += "Src.  Pin name = " + debugThis.sourcePin.ToString() + Environment.NewLine;
            else
                this.lblDebugInfo.Text += "Src.  Pin id is null!" + Environment.NewLine;

            if (debugThis.destPin != null)
                this.lblDebugInfo.Text += "Dst.  pin id = " + debugThis.destPin.ToString() + Environment.NewLine;
            else
                this.lblDebugInfo.Text += "Dst.  pin id is null!" + Environment.NewLine;
        }

        public frmDebug(ruleItemBase debugThis)
        {
            InitializeComponent();

            if (debugThis.serial == null)
                this.lblDebugInfo.Text += "ID is null!" + Environment.NewLine;
            else
                this.lblDebugInfo.Text += "ID = " + debugThis.serial.ToString() + Environment.NewLine;
        }
    }
}
