using System;
using System.Net;
using System.Windows.Forms;
using System.Xml;

namespace ruleEngine.ruleItems
{
    public partial class frmWeatherOptions : Form
    {
        private WeatherOptions options;
        public WeatherOptions SelectedOption { get { return options; } }
        public frmWeatherOptions(WeatherOptions options)
        {
            InitializeComponent();
            txtPlace.Text = options.city;
            if (options.selectRead.StartsWith("temp"))
            {
                cboWeatherAttri.SelectedIndex = 0;
                cboUnit.SelectedIndex = options.selectRead[5] == 'c' ? 0 : 1;
            }
            else
            {
                cboUnit.Visible = false;
                switch (options.selectRead)
                {
                    case "humidity":
                        cboWeatherAttri.SelectedIndex = 1;
                        break;
                    case "condition":
                        cboWeatherAttri.SelectedIndex = 2;
                        break;
                    case "wind_condition":
                        cboWeatherAttri.SelectedIndex = 3;
                        break;
                }
            }
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            try
            {
                HttpWebRequest request =
                    (HttpWebRequest) WebRequest.Create(@"http://www.google.com/ig/api?weather=" + txtPlace.Text);
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
            cboUnit.Visible = (string) cboWeatherAttri.SelectedItem == "Temperature";
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            options = new WeatherOptions {city = txtPlace.Text};

            switch (cboWeatherAttri.SelectedIndex)
            {
                case 0:
                    options.selectRead = "temp_";
                    if (cboUnit.SelectedIndex == 0)
                        options.selectRead += "c";
                    else
                        options.selectRead += "f";
                    break;
                case 1:
                    options.selectRead = "humidity";
                    break;
                case 2:
                    options.selectRead = "condition";
                    break;
                case 3:
                    options.selectRead = "wind_condition";
                    break; 
            }
        }

        private void frmWeatherOptions_Load(object sender, EventArgs e)
        {

        }
    }
}
