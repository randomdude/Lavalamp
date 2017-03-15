using ruleEngine.ruleItems;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Text;

namespace netGui
{
    /// <summary>
    /// a View factory for rule items
    /// </summary>
    /// <typeparam name="T">The type of the form to be returned</typeparam>
    /// <typeparam name="U">The base type of the form to be returned</typeparam>
    internal abstract class formFactory<T, U, C> where T : class 
                                                 where C : IFormOptions
    {
        private string _typePrefix { get; set; }
        private string _typeSuffix { get; set; }

        public formFactory(string typePrefix, string typeSuffix)
        {
            _typePrefix = typePrefix ?? string.Empty;
            _typeSuffix = typeSuffix ?? string.Empty;
        }

        protected virtual T DefaultForm(C args) { return null; }

        /// <summary>
        /// Creates the form for options of the passed interface
        /// </summary>
        /// <param name="options"></param>
        /// <returns>if there isn't a form for this option, null else the form to configure these options</returns>
        public T createForm(C options)
        {
            Contract.Requires(_typeSuffix != null);
            Contract.Requires(_typePrefix != null);
            Contract.Requires(options != null && options.typedName != null);

            T toRet = null;

            // try to get type from the standard form name syntax
            // "frmOptionsNameOptions"
            string standardName = _typePrefix + options.typedName + _typeSuffix;
            Type optionsFormType = Type.GetType(standardName);
            if (optionsFormType != null)
            {
                ConstructorInfo constructor = optionsFormType.GetConstructor(new[] { typeof(C) });
                if (constructor != null)
                    toRet = constructor.Invoke(new object[] { options }) as T;

            }

            if(toRet == null)
            { 
                //if not look through ALL the types (slow and bad :|)
                foreach (var formInAssembly in Assembly.GetExecutingAssembly().GetTypes().Where(t => t.BaseType != null && t.BaseType == typeof(U)))
                {
                    if (formInAssembly.Name.Contains(options.typedName))
                    {
                        ConstructorInfo constructorInfo = formInAssembly.GetConstructor(new[] { typeof(C) });
                        //if we can't get the constructor presume we have the wrong type.
                        if (constructorInfo != null)
                            toRet = constructorInfo.Invoke(new object[] { options }) as T;

                    }
                }

             }

            if (toRet == null)
            {
                toRet = DefaultForm(options);
            }

            return toRet;
        }

    }
}
