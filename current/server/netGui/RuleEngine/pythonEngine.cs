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

namespace netGui.RuleEngine
{
    public class pythonEngine
    {
        public const string magicIndicator = "lavalampRuleItem_";
        public string description;
        public Dictionary<String, pin> pinList = new Dictionary<string, pin>();
        ScriptEngine engine;
        ScriptScope scope;
        public string category;
        public Dictionary<String,String> Parameters = new Dictionary<string, string>();

        public pythonEngine(String filename)
        {
            // Create a new engine and scope
            ScriptRuntimeSetup setup = new ScriptRuntimeSetup();
            setup.LanguageSetups.Add(IronPython.Hosting.Python.CreateLanguageSetup(null));
            ScriptRuntime runtime = new ScriptRuntime(setup);
            runtime.IO.RedirectToConsole();
            engine = runtime.GetEngine("IronPython");
            scope = engine.CreateScope();

            // Run the new script
            ScriptSource source = engine.CreateScriptSourceFromFile(filename);
            source.Execute(scope);

            // Fish out any methods that begin with our magic
            // todo: this could be improved to not use a temporary value.
            source = engine.CreateScriptSourceFromString("g=globals()", SourceCodeKind.Statements);
            source.Execute(scope);
            PythonDictionary globals = (PythonDictionary) scope.GetVariable("g");

            foreach (String globalName in globals.keys())
            {
                if (globalName.ToUpper().Contains(magicIndicator.ToUpper()))
                {
                    processPythonClass(globalName);
                    break;  // each file should have only one class.
                }
            }
        }

        private void processPythonClass(string name)
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
            try
            {
                source.Execute(scope);
            } 
            catch (System.MissingMemberException)
            {
                this.Parameters = null;
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
            // Now fish out pin objects
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

                try
                {
                    pinList.Add(thisPinName, new pin { name = thisPinName, direction = thisPinDirection });
                }
                catch (System.ArgumentException)
                {
                    throw new Exception("Pin names are not unique. They should be.");
                }
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

        public Dictionary<string, object> runPythonFile(Dictionary<string, object> pinList, Dictionary<string, string> parameters)
        {
            ScriptSource source;

            // Throw our pin states in to the object instance
            foreach (string varName in pinList.Keys)
            {
                // todo: This is a bit of a kludge because I can't figure out how to access instance methods from ironPython. plz halp!
                source = engine.CreateScriptSourceFromString("objectInstance." + varName + ".state = " + pinList[varName], SourceCodeKind.Statements);
                source.Execute(scope);
            }

            if (parameters != null)
            {
                // Add any parameters to the object instance
                foreach (string paramName in parameters.Keys)
                {
                    // todo: This is a bit of a kludge because I can't figure out how to access instance methods from ironPython. plz halp!
                    source =
                        engine.CreateScriptSourceFromString(
                            "objectInstance." + paramName + ".value = '" + parameters[paramName] + "'",
                            SourceCodeKind.Statements);
                    source.Execute(scope);
                }
            }

            // call the python!
            source = engine.CreateScriptSourceFromString("objectInstance.eval()", SourceCodeKind.Statements);
            source.Execute(scope);

            // And suck all our parameters out.
            Dictionary<string, Object> toRet = new Dictionary<string, Object>();
            foreach (string varName in pinList.Keys)
            {
                // fixme: again, a nasty kludge
                source = engine.CreateScriptSourceFromString("temp = objectInstance." + varName + ".state", SourceCodeKind.Statements);
                source.Execute(scope);
                toRet[varName] = scope.GetVariable("temp");
            }

            return toRet;
        }
    }
}