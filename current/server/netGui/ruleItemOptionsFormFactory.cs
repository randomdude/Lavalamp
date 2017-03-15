
namespace netGui
{
    using System.Windows.Forms;

    using netGui.RuleItemOptionForms;
    using ruleEngine.ruleItems;

    internal class ruleItemOptionsFormFactory : formFactory<IOptionForm,Form, IFormOptions>
    {
        public ruleItemOptionsFormFactory() : base("frm", "Options")
        { }
    }

}
