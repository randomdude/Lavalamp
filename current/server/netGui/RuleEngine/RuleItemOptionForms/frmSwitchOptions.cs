using netGui.RuleItemOptionForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ruleEngine.ruleItems;
using ruleEngine.ruleItems.Basic_logic;

namespace netGui.RuleEngine.RuleItemOptionForms
{
    public partial class frmSwitchOptions : Form, IOptionForm 
    {
        public frmSwitchOptions()
        {
            InitializeComponent();
    
        }

        private switchRuleItemOptions _options;

        public frmSwitchOptions(IFormOptions options) : this()
        {
            _options = (switchRuleItemOptions)options;
            chkPinFalse.Checked = _options.UseFalsePin;
            chkPinTrue.Checked = _options.UseTruePin;
            if (!_options.UseFalsePin)
            {
                txtFalseVal.Text = _options.FalseValue?.ToString();
            }

            if (!_options.UseTruePin)
            {
                txtTrueVal.Text = _options.TrueValue?.ToString();
            }
        }

        public void formClosing(object sender, CancelEventArgs e)
        {
        
        }

        public IFormOptions SelectedOptions()
        {

            return _options;
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            this._options.DataTypeTrue = this.GetDataType(this.cboDataTypeTrue);
            this._options.DataTypeFalse = GetDataType(this.cboDataTypeFalse);
            _options.UseFalsePin = chkPinFalse.Checked;
            _options.UseTruePin = chkPinTrue.Checked;
            bool success;
            _options.TrueValue = GetTypedData(txtTrueVal.Text,this.cboDataTypeTrue, out success);
            _options.FalseValue = GetTypedData(txtFalseVal.Text, this.cboDataTypeFalse,out success);
            _options.setChanged();
            this.Close();
        }

        private object GetTypedData(string data, ComboBox comboBox, out bool success)
        {
            object toRet = null;
            success = true;
            // fragile I know,
            switch (comboBox.SelectedIndex)
            {
                case 1:
                    toRet = typeof(ruleEngine.pinDataTypes.pinDataNumber);
                    float tempFl;
                    success = float.TryParse(data, out tempFl);
                    toRet = tempFl;
                    break;
                case 2:
                    toRet = typeof(ruleEngine.pinDataTypes.pinDataBool);
                    bool temp ;
                    success = bool.TryParse(data, out temp);
                    toRet = temp;
                    break;
                case 3:
           //         toRet =

                    break;
                default:
                    toRet = data;
      
                    break;

            }
            return toRet;
        }

        private Type GetDataType(ComboBox comboBox)
        {
            Type toRet = null;
            // fragile I know,
            switch (comboBox.SelectedIndex)
            {
                case 1:
                    toRet = typeof(ruleEngine.pinDataTypes.pinDataNumber);
                    break;
                case 2:
                    toRet = typeof(ruleEngine.pinDataTypes.pinDataBool);
                    break;
                case 3:
                    toRet = typeof(ruleEngine.pinDataTypes.pinDataObject);
                    break;
                default:
                    toRet = typeof(ruleEngine.pinDataTypes.pinDataString);
                    break;
                    
            }
            return toRet;
        }

        private void chkPin_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox usePinCheckbox = (CheckBox)sender;
            string pin = (string)usePinCheckbox.Tag;
            TextBox defaultValue = (TextBox)this.Controls["txt" + pin + "Val"];
            ComboBox dataType = (ComboBox)this.Controls["cboDataType" + pin];
            bool usePin = usePinCheckbox.Checked;
            dataType.Enabled = defaultValue.Enabled = !usePin;
            dataType.SelectedIndex = 0;
            if (usePin)
            {
                defaultValue.Text = "Using pin value";
                defaultValue.Font = new Font(this.txtTrueVal.Font,FontStyle.Italic);
            }
            else
            {
                defaultValue.Text = "";
                defaultValue.Font = new Font(this.txtTrueVal.Font, FontStyle.Regular);
            }



        }

        private void cboDataType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
