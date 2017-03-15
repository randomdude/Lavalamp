using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ruleEngine.ruleItems.Support
{
    using System.Reflection;

    /// <summary>
    /// Used to retrieve platform specific implementations of a given Interface
    /// </summary>
    public class SystemImplementationFactory : IFactory 
    {
        private readonly Dictionary<Type, Type> _loadedTypes = new Dictionary<Type, Type>(); 
        private Assembly _platfomAsm = null;
        
        protected Assembly ImplementationSpecificAsm 
        {
            get
            {
                if (_platfomAsm == null && _platfomAsm == null)
                {
                    string path = System.IO.Path.GetFullPath(
                            Environment.OSVersion.Platform == PlatformID.Unix
                                ? "LinuxSpecificRuleItems.dll"
                                : "WindowsSpecificRuleItems.dll");
                    _platfomAsm =
                        Assembly.LoadFile(path);
                }
                return _platfomAsm;
            }
        }

        public T GetInstance<T>() where T : class
        {
            ConstructorInfo typeConstuctor = null;
            Type interfaceType = typeof(T);
            if (_loadedTypes.ContainsKey(interfaceType))
            {
                typeConstuctor = this._loadedTypes[interfaceType].GetConstructor(new Type[] { });

            }
            else
            {
                foreach (var loadedType in ImplementationSpecificAsm.GetTypes())
                {
                    if (interfaceType.IsAssignableFrom(loadedType))
                    {
                        _loadedTypes[interfaceType] = loadedType;
                        break;
                    }
                }
                if (_loadedTypes.ContainsKey(interfaceType))
                {
                    typeConstuctor = this._loadedTypes[interfaceType].GetConstructor(new Type[] { });
                }

            }
            if (typeConstuctor != null)
                return typeConstuctor.Invoke(new object[] { }) as T;
             return null;
        }
     }
    
}
