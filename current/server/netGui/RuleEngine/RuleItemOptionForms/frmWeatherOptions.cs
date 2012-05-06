namespace netGui.RuleItemOptionForms
{
    using System;
    using System.ComponentModel;
    using System.Net;
    using System.Windows.Forms;
    using System.Xml;

    using ruleEngine.ruleItems;

    public partial class frmWeatherOptions : Form, IOptionForm
    {
        private readonly WeatherOptions _options;
        public frmWeatherOptions(IFormOptions opts)
        {
            this.InitializeComponent();
            this._options = (WeatherOptions)opts;
            this.txtPlace.Text = this._options.city;
            if (this._options.selectRead.StartsWith("temp"))
            {
                this.cboWeatherAttri.SelectedIndex = 0;
                this.cboUnit.SelectedIndex = this._options.selectRead[5] == 'c' ? 0 : 1;
            }
            else
            {
                this.cboUnit.Visible = false;
                switch (this._options.selectRead)
                {
                    case "humidity":
                        this.cboWeatherAttri.SelectedIndex = 1;
                        break;
                    case "condition":
                        this.cboWeatherAttri.SelectedIndex = 2;
                        break;
                    case "wind_condition":
                        this.cboWeatherAttri.SelectedIndex = 3;
                        break;
                }
            }
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            try
            {
                HttpWebRequest request =
                    (HttpWebRequest) WebRequest.Create(@"http://www.google.com/ig/api?weather=" + this.txtPlace.Text);
                HttpWebResponse webResponse = (HttpWebResponse) request.GetResponse();
                if (webResponse.StatusCode != HttpStatusCode.OK)
                {
                    MessageBox.Show(this , "Could not connect to web service provider" , "Error" , MessageBoxButtons.OK ,
                                    MessageBoxIcon.Error);
                }

                using (XmlTextReader reader = new XmlTextReader(webResponse.GetResponseStream()))
                {
                    while (reader.Read())
                    {
                        if (reader.Name == "problem_cause")
                        {
                            MessageBox.Show(this , "Could not find the place name entered" , "Error" ,
                                            MessageBoxButtons.OK ,
                                            MessageBoxIcon.Warning);
                            return;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(this, "Could not connect to web service provider due to an unknown error:" + ex.Message, "Error", MessageBoxButtons.OK,
                                   MessageBoxIcon.Error);
            }

        }

        private void cboWeatherAttri_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cboUnit.Visible = (string) this.cboWeatherAttri.SelectedItem == "Temperature";
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            _options.city = txtPlace.Text;
            switch (this.cboWeatherAttri.SelectedIndex)
            {
                case 0:
                    this._options.selectRead = "temp_";
                    if (this.cboUnit.SelectedIndex == 0)
                        this._options.selectRead += "c";
                    else
                        this._options.selectRead += "f";
                    break;
                case 1:
                    this._options.selectRead = "humidity";
                    break;
                case 2:
                    this._options.selectRead = "condition";
                    break;
                case 3:
                    this._options.selectRead = "wind_condition";
                    break; 
            }
        }

        private void frmWeatherOptions_Load(object sender, EventArgs e)
        {

        }

        public IFormOptions SelectedOptions()
        {
            return this._options;
        }

        public void formClosing(object sender, CancelEventArgs e)
        {

        }
    }
}
