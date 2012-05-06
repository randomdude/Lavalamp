using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace netGui.RuleEngine.RuleItemOptionForms
{
    using System.Diagnostics.Contracts;

    using netGui.RuleItemOptionForms;

    using ruleEngine.ruleItems;
    using ruleEngine.ruleItems.Starts;

    public partial class frmTimeOptions : Form, IOptionForm
    {
        private readonly int daysInCurrentMonth;

        private readonly TimeOptions _options = new TimeOptions();

        public frmTimeOptions(IFormOptions opts)
        {
            Contract.Requires(opts != null);
            InitializeComponent();
            Contract.Assume(opts.GetType() == typeof(TimeOptions));
            Height = 147;
            comboBox1.DataSource = Enum.GetNames(typeof(TimeToRun));
            cboWeekDay.DataSource = Enum.GetNames(typeof(DayOfWeek));
            daysInCurrentMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            _options = opts as TimeOptions;
            setupControl(_options.when, false);
            
            txtTime.hours = _options.hours;
            txtTime.minutes = _options.minutes;

        }

        private void setupControl(TimeToRun when, bool clearForm)
        {
            if (!clearForm) comboBox1.SelectedItem = when.ToString();

            switch (when)
            {
                case TimeToRun.Yearly:
                    if (!clearForm) dtSelectTime.Value = new DateTime(_options.Year, _options.Month, _options.Day);
                    else
                    {
                        dtSelectTime.Value = DateTime.Now;
                        dtMonth.Visible = false;
                        cboWeekDay.Visible = false;
                    }
                    Height = 147;
                    dtSelectTime.Visible = true;

                    break;
                case TimeToRun.Weekly:
                    Height = 150;
                    cboWeekDay.Visible = true;

                    if (!clearForm) cboWeekDay.SelectedIndex = _options.Day;
                    else
                    {
                        dtMonth.Visible = false;
                        dtSelectTime.Visible = false;
                        cboWeekDay.SelectedIndex = 1;
                    }
                    break;
                case TimeToRun.Monthly:
                    Height = 311;
                    cboWeekDay.Visible = false;
                    dtMonth.Visible = true;
                    dtMonth.MinDate = dtMonth.MinDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    dtMonth.MaxDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, daysInCurrentMonth);
                    if (!clearForm)
                    {
                        int dayInMonth = _options.Day > daysInCurrentMonth ? daysInCurrentMonth : _options.Day;
                        Contract.Assume(dayInMonth > 1);
                        dtMonth.SetDate(new DateTime(DateTime.Now.Year, DateTime.Now.Month, dayInMonth));
                    }
                    else
                    {
                        dtMonth.SetDate(DateTime.Now);
                    }
                    break;
                case TimeToRun.Daily:
                    Height = 147;
                    dtMonth.Visible = false;
                    cboWeekDay.Visible = false;
                    dtSelectTime.Visible = false;
                    break;
            }
        }


        public IFormOptions SelectedOptions()
        {
            return _options;
        }

        public void formClosing(object sender, CancelEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Contract.Requires(this.comboBox1.Text != null);

            TimeToRun selected = (TimeToRun)Enum.Parse(typeof(TimeToRun), this.comboBox1.Text);
            setupControl(selected, true);
        }

        private void btnOk_Click_1(object sender, EventArgs e)
        {
            Contract.Requires(this.comboBox1.Text != null);

            _options.when = (TimeToRun)Enum.Parse(typeof(TimeToRun), this.comboBox1.Text);
            if (dtSelectTime.Visible)
            {
                _options.Year = dtSelectTime.Value.Year;
                _options.Month = dtSelectTime.Value.Month;
                _options.Day = dtSelectTime.Value.Day;
            }
            else if (_options.when == TimeToRun.Monthly)
            {
                _options.Day = dtMonth.SelectionStart.Day;
            }
            else if (_options.when == TimeToRun.Weekly)
            {
                _options.Day = (int)dtMonth.SelectionStart.DayOfWeek + 1;


                _options.minutes = txtTime.minutes;
                _options.hours = txtTime.hours;
            }
            _options.setChanged();
            this.Close();
        }
    }
}
