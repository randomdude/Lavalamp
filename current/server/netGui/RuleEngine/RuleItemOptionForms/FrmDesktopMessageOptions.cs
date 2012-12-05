namespace netGui.RuleItemOptionForms
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;
    using ruleEngine.ruleItems;

    public partial class FrmDesktopMessageOptions : Form, IOptionForm
    {
        public desktopMessageOptions currentOptions;

        public FrmDesktopMessageOptions()
        {
            this.currentOptions = new desktopMessageOptions();

            this.commonConstructorStuff();
        }

        public FrmDesktopMessageOptions(IFormOptions newOptions)
        {
            this.currentOptions = newOptions as desktopMessageOptions;

            this.commonConstructorStuff();
        }

        private void commonConstructorStuff()
        {
            this.InitializeComponent();
            this.CenterToParent();
            this.cmbLocation.Items.Clear();
            foreach (string positionName in Enum.GetNames(typeof(desktopMessageLocation)))
                this.cmbLocation.Items.Add(positionName);
            this.cmbLocation.Text = this.currentOptions.dsklocation.ToString();

            this.txtMessage.Text = this.currentOptions.message;
            this.txtMessage.BackColor = this.currentOptions.background;
            this.txtMessage.ForeColor = this.currentOptions.foreground;

            if (this.currentOptions.holdSpeed==0)
                this.trackbarFadeInSpeed.Value = 0;
            else
                this.trackbarFadeInSpeed.Value = this.currentOptions.holdSpeed / 10;
            this.lblholdSpeed.Text = this.trackbarFadeInSpeed.Value.ToString() + "s";
        }

        private void cmdPreview_Click(object sender, EventArgs e)
        {
            ruleItem_desktopMessage preview = new ruleItem_desktopMessage(this.currentOptions);
            preview.showIt(this.txtMessage.Text);
        }

        private void trackbarFadeInSpeed_Scroll(object sender, EventArgs e)
        {
            this.lblholdSpeed.Text = this.trackbarFadeInSpeed.Value.ToString() + "s";

            // values are stored in hundreds of milliseconds, so multiply
            // seconds by 10
            this.currentOptions.holdSpeed = this.trackbarFadeInSpeed.Value * 10;
        }

        private void cmbLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.currentOptions.dsklocation = (desktopMessageLocation) Enum.Parse(typeof (desktopMessageLocation), this.cmbLocation.Text);
        }

        private void txtMessage_TextChanged(object sender, EventArgs e)
        {
            this.currentOptions.message = this.txtMessage.Text;
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            currentOptions.setChanged();
            this.Close();
        }

        private void btnChangeFgd_Click(object sender, EventArgs e)
        {
            this.colorDialog1.Color = this.currentOptions.foreground;
            if (this.colorDialog1.ShowDialog() == DialogResult.OK)
                this.currentOptions.foreground = this.colorDialog1.Color;

            this.txtMessage.ForeColor = this.currentOptions.foreground;
        }

        private void btnChangeBkgd_Click(object sender, EventArgs e)
        {
            this.colorDialog1.Color = this.currentOptions.background;
            if (this.colorDialog1.ShowDialog() == DialogResult.OK)
                this.currentOptions.background = this.colorDialog1.Color;

            this.txtMessage.BackColor = this.currentOptions.background;
        }

        public IFormOptions SelectedOptions()
        {
            return currentOptions;
        }

        public void formClosing(object sender, CancelEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
