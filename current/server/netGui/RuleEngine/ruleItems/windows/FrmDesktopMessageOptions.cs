using System;
using System.Windows.Forms;

namespace netGui.RuleEngine.ruleItems.windows
{
    public partial class FrmDesktopMessageOptions : Form
    {
        public desktopMessageOptions currentOptions;

        public FrmDesktopMessageOptions()
        {
            currentOptions = new desktopMessageOptions();

            commonConstructorStuff();
        }

        public FrmDesktopMessageOptions(desktopMessageOptions newOptions)
        {
            currentOptions = new desktopMessageOptions(newOptions);

            commonConstructorStuff();
        }

        private void commonConstructorStuff()
        {
            InitializeComponent();

            cmbLocation.Items.Clear();
            foreach (string positionName in Enum.GetNames(typeof(desktopMessageLocation)))
                cmbLocation.Items.Add(positionName);
            cmbLocation.Text = currentOptions.location.ToString();

            txtMessage.Text = currentOptions.message;
            txtMessage.BackColor = currentOptions.background;
            txtMessage.ForeColor = currentOptions.foreground;
        }

        private void cmdPreview_Click(object sender, EventArgs e)
        {
            ruleItems.ruleItem_desktopMessage preview = new ruleItem_desktopMessage(currentOptions);
            preview.showIt();
        }

        private void trackbarFadeInSpeed_Scroll(object sender, EventArgs e)
        {
            this.lblholdSpeed.Text = trackbarFadeInSpeed.Value.ToString() + "s";

            // values are stored in hundreds of milliseconds, so multiply
            // seconds by 10
            currentOptions.holdSpeed = trackbarFadeInSpeed.Value * 10;
        }

        private void cmbLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentOptions.location = (desktopMessageLocation) Enum.Parse(typeof (desktopMessageLocation), cmbLocation.Text);
        }

        private void txtMessage_TextChanged(object sender, EventArgs e)
        {
            currentOptions.message = txtMessage.Text;
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void btnChangeFgd_Click(object sender, EventArgs e)
        {
            this.colorDialog1.Color = currentOptions.foreground;
            if (colorDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                currentOptions.foreground = this.colorDialog1.Color;

            txtMessage.ForeColor = currentOptions.foreground;
        }

        private void btnChangeBkgd_Click(object sender, EventArgs e)
        {
            this.colorDialog1.Color = currentOptions.background;
            if (colorDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                currentOptions.background = this.colorDialog1.Color;

            txtMessage.BackColor = currentOptions.background;
        }
    }
}
