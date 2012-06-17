﻿// -----------------------------------------------------------------------
// <copyright file="RuleItemRepository.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ruleEngine
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    public class ruleItemRepository : IRuleItemRepository
    {
        public List<DirectoryInfo> searchDirectories { get; set; }

        /// <summary>
        /// returns all rule items from this assembly and any assemblies in the <see cref="searchDirectories">directories listed</see>
        /// </summary>
        /// <returns>a list contain all possible rule items which can be created in the system</returns>
        public List<IRuleItem> getAllRuleItems()
        {
            List<Assembly> asmsToLookin = new List<Assembly>(new[] { GetType().Assembly });
           
            foreach (DirectoryInfo directory in searchDirectories)
            {
                asmsToLookin.AddRange(directory.GetFiles("*.dll").Select(f => Assembly.Load(f.FullName)).Where(a => a.FullName == GetType().Assembly.FullName));
            }
            List<IRuleItem> toRet = new List<IRuleItem>(asmsToLookin.Count);
            foreach (var assembly in asmsToLookin)
            {
                foreach (var module in assembly.GetModules())
                {
                    foreach (var types in module.GetTypes().Where(t => t.IsDefined(typeof(ToolboxRule),false)))
                    {
                        ConstructorInfo constr = types.GetConstructor(new Type[0]);
                        if (constr != null)
                        {
                            Object newRuleItem = constr.Invoke(new object[0]);
                            toRet.Add(newRuleItem as IRuleItem);
                        }
                    }
                }
            }
            return toRet;
        }
    }

    public interface IRuleItemRepository
    {

        List<IRuleItem> getAllRuleItems();


    }
}
