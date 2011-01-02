using System;
using System.Collections.Generic;
using IronPython.Runtime;
using IronPython.Runtime.Types;
using Microsoft.Scripting.Hosting;

namespace ruleEngine.ruleItems
{
    public class pythonEngine : IScriptEngine
    {
        /// <summary>
        /// The prefix which is present on ruleItem Python classes.
        /// </summary>
        private const string magicIndicator = "lavalampRuleItem_";

        private const string magicParameters = "parameters";
        private const string magicName = "name";
        private const string magicDirection = "direction";
        private const string magicValue = "value";
        private const string magicPins = "pins";
        private const string magicFriendlyName = "friendlyName";
        private const string magicCategory = "category";
        private const string magicState = "state";
        private const string magicEval = "eval";
        private const string magicGlobals = "globals";

        /// <summary>
        /// The name of our instance of the ruleItem Python class
        /// </summary>
        private OldInstance _pythonInstance;

        /// <summary>
        /// Parameters passed to the python class before evaluation
        /// </summary>
        private Dictionary<String, String> _parameters = new Dictionary<string, string>();
        
        /// <summary>
        /// Parameters passed to the python class before evaluation
        /// </summary>
        public Dictionary<String, String> parameters
        {
            get { return _parameters; }
            set { _parameters = value; }
        }

        private readonly Dictionary<string, pin> _pinList = new Dictionary<string, pin>();
        public Dictionary<string, pin> getPinInfo()
        {
            return _pinList;
        }

        private readonly ScriptEngine _engine;
        private readonly ScriptScope _scope;

        public pythonEngine(String pythonFilename)
        {
            // Create a new engine and scope
            ScriptRuntimeSetup setup = new ScriptRuntimeSetup();
            setup.LanguageSetups.Add(IronPython.Hosting.Python.CreateLanguageSetup(null));
            ScriptRuntime runtime = new ScriptRuntime(setup);
            runtime.IO.RedirectToConsole();
            _engine = runtime.GetEngine("IronPython");
            _scope = _engine.CreateScope();

            loadPythonFile(pythonFilename);
        }

        private void loadPythonFile(string filename)
        {
            // Run the new script, in order to register classes from it
            ScriptSource source = _engine.CreateScriptSourceFromFile(filename);
            source.Execute(_scope);

            // Fish a class which begins with our magic by examining the return of the 
            // Python globals() function.
            IEnumerable<string> globalFunctionNames = _scope.GetVariableNames();

            foreach (String globalName in globalFunctionNames)
            {
                if (globalName.ToUpper().Contains(magicIndicator.ToUpper()))
                {
                    loadPythonClass(globalName);
                    break;  // We support only one python class per file
                }
            }
        }

        private void loadPythonClass(string className)
        {
            // first, instantiate the object.
            object classObject = _scope.GetVariable(className);
            _pythonInstance = (OldInstance)_scope.Engine.Operations.CreateInstance(classObject);

            // Pluck fields out of our python module
            loadPins();
            loadParameters();
        }

        /// <summary>
        /// Load parameters from the python file in to our class.
        /// </summary>
        private void loadParameters()
        {
            // Check if the 'parameters' variable exists, and bail if not
            if (!hasMember(_pythonInstance, magicParameters))
                return;

            IronPython.Runtime.List parametersObject = (List) _engine.Operations.GetMember(_pythonInstance, magicParameters);

            // Now add each to our array.
            _parameters.Clear();
            foreach (OldInstance thisParameterObject in parametersObject)
            {
                string thisParamName = (string)_engine.Operations.GetMember(thisParameterObject, magicName);
                string thisParamValue = (string) _engine.Operations.GetMember(thisParameterObject, magicValue);

                _parameters.Add(thisParamName, thisParamValue);
            }
        }

        private IEnumerable<string> getMemberNames(object parentObject)
        {
            return _engine.Operations.GetMemberNames(parentObject);
        }

        private bool hasMember(object parentObject, string memberName)
        {
            IEnumerable<string> memberList = getMemberNames(parentObject);

            foreach (string thisMemberName in memberList)
            {
                if (thisMemberName.ToUpper() == memberName.ToUpper())
                    return true;
            }
            return false;
        }

        private void loadPins()
        {
            // Now fish out pin objects, storing them in our class list. The rest of the app
            // will pluck them out via the getPinInfo() method.
            List pins = (IronPython.Runtime.List) _scope.Engine.Operations.GetMember(_pythonInstance, "pins");

            foreach (OldInstance thisPin in pins)
            {
                string thisPinName = (string) _scope.Engine.Operations.GetMember(thisPin, magicName);
                string thisPinDirectionString = (string)_scope.Engine.Operations.GetMember(thisPin, magicDirection);

                pinDirection thisPinDirection = (pinDirection) Enum.Parse(typeof(pinDirection), thisPinDirectionString.Trim());

                pin newPin = new pin { name = thisPinName, direction = thisPinDirection };
                _pinList.Add(thisPinName, newPin);
            }
        }

        public string getDescription()
        {
            // return the value in the .friendlyName member.
            return (string)_engine.Operations.GetMember(_pythonInstance, magicFriendlyName );
        }

        public string getCategory()
        {
            // return the value in the .category member, or String.Empty if it is not present.
            if (!_engine.Operations.ContainsMember(_pythonInstance, magicCategory))
                return string.Empty;

            return (string)_engine.Operations.GetMember(_pythonInstance, magicCategory);
        }

        /// <summary>
        /// Update the .state method of pin objects in our python instance
        /// </summary>
        private void propagatePinStatesToPython()
        {
            IronPython.Runtime.List pinsObject = (List) _engine.Operations.GetMember(_pythonInstance, magicPins);

            foreach (OldInstance thisPinObject in pinsObject)
            {
                string propname = (string) _engine.Operations.GetMember(thisPinObject, magicName);
                object newValue = _pinList[propname].value.getData();
                _engine.Operations.SetMember(thisPinObject, magicState, newValue);
            }
        }

        private void propagatePinStatesFromPython()
        {
            IronPython.Runtime.List pinsObject = (List)_engine.Operations.GetMember(_pythonInstance, magicPins);

            foreach (OldInstance thisPinObject in pinsObject)
            {
                string propname = (string)_engine.Operations.GetMember(thisPinObject, magicName);
                object newValue = _engine.Operations.GetMember(thisPinObject, magicState);

                _pinList[propname].value.setData(newValue);
            }
        }


        /// <summary>
        /// update the .value member of our python object instance to reflect current parameters
        /// </summary>
        private void propogateParametersToPython()
        {
            // If the 'parameters' value is missing, return early. There's nothing to do.
            if (parameters.Count == 0)
                return;

            IronPython.Runtime.List parametersObject = (List)_engine.Operations.GetMember(_pythonInstance, magicParameters);

            foreach (OldInstance thisParameterObject in parametersObject)
            {
                string propname = (string)_engine.Operations.GetMember(thisParameterObject, magicName);
                object newValue = _parameters[propname];
                _engine.Operations.SetMember(thisParameterObject, magicValue, newValue);
            }
        }

        public void evaluateScript()
        {
            // prepare the python object instance for execution, by supplying it with the
            // necessary inputs and parameters.
            propagatePinStatesToPython();
            propogateParametersToPython();

            // call the python class itself
            _engine.Operations.InvokeMember(_pythonInstance, magicEval);

            // And suck all our outputs out, propogating them to their relevant output
            // pins.
            propagatePinStatesFromPython();
        }
    }
}