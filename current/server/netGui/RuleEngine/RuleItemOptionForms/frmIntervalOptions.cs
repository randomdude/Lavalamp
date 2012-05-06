using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;

namespace netGui.RuleEngine.RuleItemOptionForms
{
    using netGui.RuleItemOptionForms;
    using System.Text;
    using System.Windows.Forms;

    using ruleEngine.ruleItems;

    public partial class frmIntervalOptions : Form, IOptionForm
    {
        private IntervalOption _options;

        public frmIntervalOptions(IFormOptions opts)
        {
            InitializeComponent();
            _options = opts as IntervalOption;
            interval.hours = _options.hours;
            interval.minutes = _options.mins;
        }


        public IFormOptions SelectedOptions()
        {
            return _options;
        }

        public void formClosing(object sender, CancelEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            _options.hours = interval.hours;
            _options.mins = interval.minutes;
            _options.setChanged();
            this.Close();
        }
    }
}
