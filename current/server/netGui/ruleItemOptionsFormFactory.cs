using System;
using System.Windows.Forms;
using ruleEngine.ruleItems;
using System.Reflection;
namespace netGui
{
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Linq;

    using netGui.RuleItemOptionForms;

    internal class ruleItemOptionsFormFactory
    {
        /// <summary>
        /// Creates the form for options of the passed interface
        /// </summary>
        /// <param name="options"></param>
        /// <returns>if there isn't a form for this option, null else the form to configure these options</returns>
        [Pure]
        static public IOptionForm createForm(IFormOptions options)
        {
            Contract.Requires(options != null && options.typedName != null);
            // try to get type from the standard form name syntax
            // "frmOptionsNameOptions"
            string standardName = "frm" + options.typedName + "Options";
            Type optionsFormType = Type.GetType(standardName);
            if (optionsFormType != null)
            {
                ConstructorInfo constructor = optionsFormType.GetConstructor(new[] { typeof(IFormOptions) });
                if (constructor != null)
                    return constructor.Invoke(new object[] { options }) as IOptionForm;

            }

            //if not look through ALL the types (slow and bad :|)
            foreach (var formInAssembly in Assembly.GetExecutingAssembly().GetTypes().Where(t => t.BaseType != null && t.BaseType == typeof(Form)))
            {
                if (formInAssembly.Name.Contains(options.typedName))
                {
                    ConstructorInfo constructorInfo = formInAssembly.GetConstructor(new[] { typeof(IFormOptions) });
                    //if we can't get the constructor presume we have the wrong type.
                    if (constructorInfo != null)
                        return constructorInfo.Invoke(new object[] { options }) as IOptionForm;

                }
            }
            return null;


        }
    }

}
