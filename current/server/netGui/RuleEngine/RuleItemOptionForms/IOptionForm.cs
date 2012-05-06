namespace netGui.RuleItemOptionForms
{
    using System.Windows.Forms;

    using ruleEngine.ruleItems;

    public interface IOptionForm
    {
        IFormOptions SelectedOptions();
        void formClosing(object sender, System.ComponentModel.CancelEventArgs e);

        DialogResult ShowDialog(IWin32Window ctlRuleItemWidget);
    }
}