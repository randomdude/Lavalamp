using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using IronPython.Hosting;
using IronPython.Runtime;
using IronPython.Runtime.Types;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using Microsoft.Scripting.Runtime;
using netGui.RuleEngine.ruleItems.windows;
using netGui.RuleEngine.ruleItems;

namespace netGui.RuleEngine
{
    public class pythonEngine 
    {
        private const string magicIndicator = "lavalampRuleItem_";
        public string description;
        public string category;
        public Dictionary<String,String> Parameters = new Dictionary<string, string>();
        public readonly Dictionary<string, pin> pinList = new Dictionary<string, pin>();

        ScriptEngine engine;
        ScriptScope scope;

        public pythonEngine(String pythonFilename)
        {
            // Create a new engine and scope
            ScriptRuntimeSetup setup = new ScriptRuntimeSetup();
            setup.LanguageSetups.Add(IronPython.Hosting.Python.CreateLanguageSetup(null));
            ScriptRuntime runtime = new ScriptRuntime(setup);
            runtime.IO.RedirectToConsole();
            engine = runtime.GetEngine("IronPython");
            scope = engine.CreateScope();

            loadPythonFile(pythonFilename);
        }

        private void loadPythonFile(string filename)
        {
            // Run the new script, in order to register classes from it
            ScriptSource source = engine.CreateScriptSourceFromFile(filename);
            source.Execute(scope);

            // Fish out any methods that begin with our magic
            // todo: this could be improved to not use a temporary value.
            // todo: eep, what if this temporary value clashes with one in the script?!
            source = engine.CreateScriptSourceFromString("g=globals()", SourceCodeKind.Statements);
            source.Execute(scope);
            PythonDictionary globals = (PythonDictionary) scope.GetVariable("g");

            foreach (String globalName in globals.keys())
            {
                if (globalName.ToUpper().Contains(magicIndicator.ToUpper()))
                    loadPythonClass(globalName);
            }
        }

        private void loadPythonClass(string name)
        {
            // Pluck static fields out of our python module
            loadFriendlyName(name);
            loadCategory(name);
            loadPins(name);
            loadParameters(name);

            // finally, instantiate the object.
            ScriptSource source = engine.CreateScriptSourceFromString("objectInstance = " + name + "()", SourceCodeKind.Statements);
            source.Execute(scope);
        }

        private void loadParameters(string name)
        {
            // Now fish out paramter objects
            ScriptSource source = engine.CreateScriptSourceFromString("temp = " + name + ".parameters",
                                                                      SourceCodeKind.Statements);
            this.Parameters.Clear();

            //TODO: noooooo
            try
            {
                source.Execute(scope);
            } 
            catch (System.MissingMemberException)
            {
                return;
            }

            List newParameters = (IronPython.Runtime.List) scope.GetVariable("temp");

            for (int n = 0; n < newParameters.Count; n++)
            {
                source = engine.CreateScriptSourceFromString(createPythonToGetParameterName("temp", name, n),
                                                             SourceCodeKind.Statements);
                source.Execute(scope);
                String thisParamName = (String) scope.GetVariable("temp");

                source = engine.CreateScriptSourceFromString(createPythonToGetParameterValue("temp", name, n),
                                                             SourceCodeKind.Statements);
                source.Execute(scope);
                String thisParamValue = (String) scope.GetVariable("temp");

                try
                {
                    Parameters.Add(thisParamName, thisParamValue);
                }
                catch (System.ArgumentException)
                {
                    throw new Exception("Parameter names are not unique. They should be.");
                }
            }
        }

        private string createPythonToGetParameterValue(string output, string name, int index)
        {
            return output + " = " + name + ".parameters[" + index + "].value";
        }

        private string createPythonToGetParameterName(string output, string name, int index)
        {
            return output + " = " + name + ".parameters[" + index + "].name";
        }

        private void loadPins(string name)
        {
            // Now fish out pin objects, storing them in our class list. The rest of the app
            // will pluck them out via the getPinInfo() method.
            ScriptSource source = engine.CreateScriptSourceFromString("temp = " + name + ".pins", SourceCodeKind.Statements);
            source.Execute(scope);
            List pins = (IronPython.Runtime.List)scope.GetVariable("temp");

            for (int n = 0; n < pins.Count; n++)
            {
                source = engine.CreateScriptSourceFromString(createPythonToGetPinName("temp", name, n), SourceCodeKind.Statements);
                source.Execute(scope);
                String thisPinName = (String)scope.GetVariable("temp");

                source = engine.CreateScriptSourceFromString(createPythonToGetPinDirection("temp", name, n), SourceCodeKind.Statements);
                source.Execute(scope);
                String thisPinDirectionString = (String)scope.GetVariable("temp");

                pinDirection thisPinDirection;

                if (thisPinDirectionString.ToUpper().Trim() == "input".ToUpper())
                    thisPinDirection = pinDirection.input;
                else if (thisPinDirectionString.ToUpper().Trim() == "output".ToUpper())
                    thisPinDirection = pinDirection.output;
                else
                    throw new Exception("Unrecognised pin direction '" + thisPinDirectionString + "'. Should be 'input' or 'output'.");

                if (pinList.ContainsKey(thisPinName))
                    throw new Exception("Pin names are not unique. They should be.");

                pin newPin = new pin { name = thisPinName, direction = thisPinDirection };
                pinList.Add(thisPinName, newPin);
            }
        }

        private void loadFriendlyName(string name)
        {
            // todo: this could be improved to not use a temporary value.
            ScriptSource source = engine.CreateScriptSourceFromString("temp = " + name + ".friendlyName", SourceCodeKind.Statements);
            source.Execute(scope);
            description = (String)scope.GetVariable("temp");            
        }

        private void loadCategory(String name)
        {
            ScriptSource source = engine.CreateScriptSourceFromString("temp = " + name + ".category", SourceCodeKind.Statements);
            try
            {
                source.Execute(scope);
                category = (String)scope.GetVariable("temp");
            }
            catch (MissingMemberException)
            {
                category = "";
            }
        }

        private string createPythonToGetPinDirection(string output, string name, int index)
        {
            return output + " = " + name + ".pins[" + index + "].direction";
        }

        private string createPythonToGetPinName(string output, string name, int index)
        {
            return output + " = " + name + ".pins[" + index + "].name";
        }

        /// <summary>
        /// Execute some python code
        /// </summary>
        /// <param name="codeToExecute">some python code to execute</param>
        private void executePython(string codeToExecute)
        {
            ScriptSource source = engine.CreateScriptSourceFromString(codeToExecute, SourceCodeKind.Statements);
            source.Execute(scope);
        }

        /// <summary>
        /// Update the .state method of pin objects in our python instance
        /// </summary>
        private void propagatePinStatesToPython()
        {
            foreach (string varName in pinList.Keys)
            {
                // todo: This is a bit of a kludge because I can't figure out how to access
                // instance methods from IronPython.
                string pythonCommand = "objectInstance." + varName + ".state = " + pinList[varName].value.getData();
                executePython(pythonCommand);
            }
        }

        /// <summary>
        /// update the .value member of our python object instance to reflect current parameters
        /// </summary>
        private void propogateParametersToPython()
        {
            // Add any parameters to the object instance
            if (Parameters != null)
            {
                foreach (string paramName in Parameters.Keys)
                {
                    // todo: This is a bit of a kludge because I can't figure out how to access instance methods from ironPython. plz halp!
                    string pythonCommand = "objectInstance." + paramName + ".value = '" + Parameters[paramName] + "'";
                    executePython(pythonCommand);
                }
            }
        }

        public void runPythonFile()
        {
            // prepare the python object instance for execution, by supplying it with the
            // neccessary inputs and parameters.
            propagatePinStatesToPython();
            propogateParametersToPython();

            // call the python class itself
            executePython("objectInstance.eval()");

            // And suck all our outputs out, propogating them to their relevant output
            // pins.
            foreach (string varName in pinList.Keys)
            {
                if (pinList[varName].direction == pinDirection.input)
                    continue;

                // Get the value..
                string pythonCode = "temp = objectInstance." + varName + ".state";
                executePython(pythonCode);
                bool newvalue = (bool)scope.GetVariable("temp");

                // propogate the new value to the new pin
                pinList[varName].value.setData(newvalue);
            }
        }
    }
}