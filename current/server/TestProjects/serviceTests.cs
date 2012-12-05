using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProjects
{
    using ServiceStack.Text;

    using ruleEngine;

    [TestClass]
    public class serviceTests
    {
        private static T DeserializeAnonymousType<T>(T template, string json) where T : class
        {
            TypeConfig<T>.EnableAnonymousFieldSetters = true;
            return JsonSerializer.DeserializeFromString(json, template.GetType()) as T;
        }
        [TestMethod]
        public void testSerializationOfPin()
        {
            JsConfig.Reset();
            JsConfig.ConvertObjectTypesIntoStringDictionary = true;
            JsConfig<pin>.SerializeFn = p => string.Format(
                "{{\"direction\":\"{0}\",\"name\":\"{1}\",\"linkedto\":\"{2}\",\"description\":\"{3}\" }}",
                p.direction,
                p.name,
                p.linkedTo.id.ToString(),
                p.description);
            JsConfig<pin>.DeSerializeFn = s =>
            {
                var anomData = DeserializeAnonymousType(
                     new
                     {
                         direction = default(string),
                         name = default(string),
                         linkedto = default(string),
                         description = default(string)
                     },
                     s); return new pin()
                     {
                         direction = (pinDirection)Enum.Parse(typeof(pinDirection), anomData.direction),
                         description = anomData.description,
                         linkedTo = new pinGuid(anomData.linkedto),
                         name = anomData.name
                     };
            };
            JsConfig<pinGuid>.SerializeFn = guid => guid.id.ToString();
            JsConfig<pinGuid>.DeSerializeFn = s => new pinGuid(s);
            pin x = new pin()
                    {
                        description = "hello world",
                        direction = pinDirection.output,
                        name = "kat",
                        linkedTo = new pinGuid(Guid.NewGuid().ToString())
                    };
            JsonSerializer<pin> json = new JsonSerializer<pin>();
            string hello = json.SerializeToString(x);
            Assert.IsNotNull(hello);
        }
    }
}
