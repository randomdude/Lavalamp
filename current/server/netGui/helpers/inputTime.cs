
namespace netGui.helpers
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.IO;
    using System.Text.RegularExpressions;

    public partial class inputTime : UserControl
    {
        private readonly int MaxHourVal;

        private string _suffix;

        public inputTime()
        {
            InitializeComponent();
            this.Resize += resizeForm;
            
            MaxHourVal = this.format == DateTimeFormat.TwentyFourHourClock ? 24 : 12;
            txtTimeBox.Text = string.Format("{0}{0}{1}{0}{0}{2}", NumberFormatInfo.CurrentInfo.NativeDigits[0], DateTimeFormatInfo.CurrentInfo.TimeSeparator, suffix);
            
        }

        string suffix
        {
            
            get
            {
                Contract.Ensures(this.format == DateTimeFormat.TwelveHourClock ? _suffix != null : _suffix == string.Empty);
               
                if (this.format == DateTimeFormat.TwelveHourClock)
                {
                    return _suffix == "" ? _suffix = " " + DateTimeFormatInfo.CurrentInfo.AMDesignator : _suffix;
                }
                return "";
            }
            
        }

        private void resizeForm(object sender, EventArgs e)
        {
            txtTimeBox.Width = this.Width;
            this.Height = txtTimeBox.Height;
        }

        private void ensureInputFormat(object sender, KeyPressEventArgs e)
        {
            Contract.Requires(sender != null && e != null);
            Contract.Requires(sender.GetType() == typeof(TextBox));

            TextBox input = sender as TextBox;

            int pos = input.SelectionStart;

            if (e.KeyChar == (char)Keys.Up || e.KeyChar == (char)Keys.Down)
            {

                int sum = e.KeyChar == (char)Keys.Up ? 1 : -1;
                if (pos <= 2)
                {
                    hours = Convert.ToInt32(this.moveHours(this.hours.ToString(NumberFormatInfo.CurrentInfo), sum));
                }
                else
                {
                    int tmins = minutes + sum;
                    if (tmins >= 60)
                    {
                        hours = Convert.ToInt32(this.moveHours(this.hours.ToString(NumberFormatInfo.CurrentInfo), sum));
                        minutes = 0;
                    }
                    else if (tmins < 0)
                    {
                        minutes = 59;
                        hours = Convert.ToInt32(this.moveHours(this.hours.ToString(NumberFormatInfo.CurrentInfo), sum));
                        
                    }
                }
                e.Handled = true;
                return;
            }
            if (e.KeyChar == (char)Keys.Back)
            {
                if (pos != 0)
                {
                    --pos;
                    if (pos == 2) --pos;
                    char[] time = input.Text.ToCharArray();
                    time[pos] = '0';
                    input.Text = new string(time);
                    input.SelectionStart = pos;
                }
                e.Handled = true;
                return;
            }
            if (e.KeyChar == (char)Keys.Delete)
            {
                if (pos != 4)
                {
                    if (pos == 2) ++pos;
                    char[] time = input.Text.ToCharArray();
                    time[pos] = '0';
                    input.Text = new string(time);
                    input.SelectionStart = pos + 1;
                }
                e.Handled = true;
                return;
            }
            Regex r = new Regex(@"[0-9]+");

            string key = e.KeyChar.ToString();
            
            if (!r.IsMatch(key))
            {
                e.Handled = true;
                return;
            }
            if (pos > 4 )
            {
                e.Handled = true;
                return;
            }
            char[] c = input.Text.ToCharArray();
            Contract.Assume(key.Length == 1);
            int digit = int.Parse(key);

            if (digit > 6 && pos == 2 || 
                digit > 2 && pos == 0 && format == DateTimeFormat.TwentyFourHourClock || 
                digit > 1 && pos == 0 && format == DateTimeFormat.TwelveHourClock)
            {
                e.Handled = true;
                return;
            }
            if (pos == 2) pos++;
            
            string sDigit = digit.ToString(NumberFormatInfo.CurrentInfo);
            Contract.Assume(sDigit.Length == 1);
            c[pos] = char.Parse(sDigit);
            input.Text = new string(c);
            e.Handled = true;
            input.SelectionStart = 1 + pos;

        }

        private string moveHours(string s, int sum)
        {
            Contract.Requires(s != null);
            Contract.Ensures(Contract.Result<string>() != null &&
                             int.Parse(Contract.Result<string>()) < MaxHourVal
                             && int.Parse(Contract.Result<string>()) >= 0);
       
            int hrs = int.Parse(s) + sum;
            if (hrs > MaxHourVal)
            {
                if (format == DateTimeFormat.TwelveHourClock)
                    swapSuffix();
                hrs = 0;
            }
            else if (hours < 0)
                hrs = MaxHourVal;

            return hrs.ToString("00");
        }

        private void swapSuffix()
        {
            if (format == DateTimeFormat.TwentyFourHourClock) 
                this._suffix = string.Empty;
            else
                this._suffix = this._suffix == " " + DateTimeFormatInfo.CurrentInfo.AMDesignator ? " " + DateTimeFormatInfo.CurrentInfo.PMDesignator : " " + DateTimeFormatInfo.CurrentInfo.AMDesignator;
        }

        [DesignOnly(true)]
        [Description("Choose between 12 and 24 hours")]
        [Browsable(true)]
        [DefaultValue(DateTimeFormat.TwentyFourHourClock)]
        public DateTimeFormat format { get; set; }

        public int hours
        {
            get
            {
                return int.Parse(txtTimeBox.Text.Substring(0, this.txtTimeBox.Text.IndexOf(DateTimeFormatInfo.CurrentInfo.TimeSeparator, System.StringComparison.CurrentCultureIgnoreCase)));
            }
            set
            {
                if (value > MaxHourVal || value < 0)
                    throw new InvalidDataException("Hours out of range");
                string hours = txtTimeBox.Text.Substring(0, this.txtTimeBox.Text.IndexOf(DateTimeFormatInfo.CurrentInfo.TimeSeparator, System.StringComparison.CurrentCultureIgnoreCase));
                string mins = txtTimeBox.Text.Substring((this.txtTimeBox.Text.IndexOf(DateTimeFormatInfo.CurrentInfo.TimeSeparator, System.StringComparison.CurrentCultureIgnoreCase) + 1),
                                                        (txtTimeBox.Text.LastIndexOfAny(NumberFormatInfo.CurrentInfo.NativeDigits.toCharArray()) - 2));
                hours = value.ToString("00");

                txtTimeBox.Text = hours + DateTimeFormatInfo.CurrentInfo.TimeSeparator + mins + suffix;
            }
        }

        public int minutes
        {
            get
            {
                return int.Parse(txtTimeBox.Text.Substring(this.txtTimeBox.Text.IndexOf(DateTimeFormatInfo.CurrentInfo.TimeSeparator, System.StringComparison.CurrentCultureIgnoreCase) + 1,
                                                        (txtTimeBox.Text.LastIndexOfAny(NumberFormatInfo.CurrentInfo.NativeDigits.toCharArray()) - 2)));
            }
            set
            {
                if (value > 60 || value < 0)
                    throw new InvalidDataException("minites out of range");
                string hours = txtTimeBox.Text.Substring(0, txtTimeBox.Text.IndexOf(DateTimeFormatInfo.CurrentInfo.TimeSeparator));
                string mins = txtTimeBox.Text.Substring(this.txtTimeBox.Text.IndexOf(DateTimeFormatInfo.CurrentInfo.TimeSeparator, System.StringComparison.CurrentCultureIgnoreCase),
                                                        (txtTimeBox.Text.LastIndexOfAny(NumberFormatInfo.CurrentInfo.NativeDigits.toCharArray()) - 1));

                mins = value.ToString("00");
                txtTimeBox.Text = hours + DateTimeFormatInfo.CurrentInfo.TimeSeparator + mins + suffix;
            }
        }
    }

    public enum DateTimeFormat
    {
        TwentyFourHourClock,
        TwelveHourClock
    }
}
