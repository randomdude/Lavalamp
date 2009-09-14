using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace netGui.RuleEngine.ruleItems.windows
{
    public partial class FrmDesktopMessageOptions : Form
    {
        public desktopMessageOptions options;
        private desktopMessageOptions originalOptions;

        public FrmDesktopMessageOptions()
        {
            InitializeComponent();
        }

        private void cmdPreview_Click(object sender, EventArgs e)
        {
            ruleItems.ruleItem_desktopMessage preview = new ruleItem_desktopMessage();
            preview.showIt();
        }

        private void trackbarFadeInSpeed_Scroll(object sender, EventArgs e)
        {
            this.lblholdSpeed.Text = trackbarFadeInSpeed.Value.ToString() + "s";

            // values are stored in hundreds of milliseconds, so multiply
            // seconds by 10
            options.holdSpeed = trackbarFadeInSpeed.Value * 10;
        }
    }
}
