using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ruleEngine.ruleItems.Support
{
    public interface IFactory
    {

        T GetInstance<T>() where T : class;

    }
}
