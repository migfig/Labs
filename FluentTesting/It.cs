using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog;

namespace FluentTesting
{
    /// <summary>
    /// Fluent parent testing class for a cleanear testing strategy
    /// </summary>
    public class It
    {
        #region properties

        /// <summary>
        /// Test description
        /// </summary>
        protected string Description { get; private set; }

        /// <summary>
        /// Target object to test with
        /// </summary>
        protected object Target { get; private set; }

        /// <summary>
        /// Method name to call for testing
        /// </summary>
        protected string MethodName { get; private set; }

        /// <summary>
        /// Method parameters
        /// </summary>
        protected object[] Parameters { get; private set; }

        /// <summary>
        /// Properties used for verification
        /// </summary>
        protected IEnumerable<NameValue> Properties { get; private set; }

        /// <summary>
        /// Log strategy for optional testing evidence
        /// </summary>
        protected ILogger Logger { get; private set; }

        #endregion properties

        #region public methods/constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="description">test description</param>
        public It(string description)
            : this(description,
                new LoggerConfiguration()
                    .WriteTo.ColoredConsole()
                    .WriteTo.RollingFile(
                        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Test-{Date}.txt"))
                    .CreateLogger())
        {
        }

        public It(string description, ILogger logger)
        {
            Assert.IsNotNull(description);
            Assert.IsFalse(string.IsNullOrEmpty(description));
            Description = description;
            Assert.IsNotNull(logger);
            Logger = logger;
            Logger.Information(Description);
        }

#if ENGLISH_LANGUAGE
        /// <summary>
        /// Specifies the target on which the test will be applied
        /// </summary>
        /// <param name="target">target instance</param>
        /// <returns>Next object in fluent sequence</returns>
        public OnIt On(object target)
        {
            Assert.IsNotNull(target);
            Logger.Information(target.ToString());
            Target = target;
            return new OnIt(this);
        }
#endif

#if SPANISH_LANGUAGE
        /// <summary>
        /// Specifies the target on which the test will be applied
        /// </summary>
        /// <param name="target">target instance</param>
        /// <returns>Next object in fluent sequence</returns>
        public OnIt Sobre(object target)
        {
            Assert.IsNotNull(target);
            Logger.Information(target.ToString());
            Target = target;
            return new OnIt(this);
        }
#endif

#if ENGLISH_LANGUAGE
        /// <summary>
        /// Specifies the parameters to be used for the method call
        /// </summary>
        /// <param name="parameters">single or multiple parameters</param>
        /// <returns>Next object in fluent sequence</returns>
        public WithParamsIt WithOtherParams(params object[] parameters)
        {
            Parameters = parameters;
            Parameters
                .ToList()
                .ForEach(p => Logger.Information("Parameter: {0}", p.ToString()));
            return new WithParamsIt(this);
        }
#endif

#if SPANISH_LANGUAGE
        /// <summary>
        /// Specifies the parameters to be used for the method call
        /// </summary>
        /// <param name="parameters">single or multiple parameters</param>
        /// <returns>Next object in fluent sequence</returns>
        public WithParamsIt ConOtrosParametros(params object[] parameters)
        {
            Parameters = parameters;
            Parameters
                .ToList()
                .ForEach(p => Logger.Information("Parameter: {0}", p.ToString()));
            return new WithParamsIt(this);
        }
#endif

        /// <summary>
        /// Does the actual transacted test
        /// </summary>
        /// <returns>Parent fluent object to start again if needed</returns>
        protected It RunTest()
        {
            Assert.IsNotNull(Target);
            Assert.IsNotNull(MethodName);
            Assert.IsFalse(string.IsNullOrEmpty(MethodName));
            Assert.IsNotNull(Properties);
            Assert.IsTrue(Properties.Any());

            return AssertTransaction();
        }

        #endregion public methods/constructors

        #region private methods

        /// <summary>
        /// Does the actual testing implementation based on the specified objects and paramters
        /// </summary>
        /// <returns>Parent fluent object</returns>
        private It AssertTransaction()
        {
            var method = Target
                .GetType()
                .GetMethods()
                .First(m => m.Name == MethodName
                            && m.GetParameters().Count() == Parameters.Count());
            Assert.IsNotNull(method);

            var result = method.Invoke(Target, Parameters);
            Assert.IsNotNull(result);

            Properties
                .ToList()
                .ForEach(p =>
                {
                    object value = null;
                    PropertyInfo prop = null;
                    switch (p.MemberType)
                    {
                        case MemberTypes.Property:
                            prop = GetProperty(p.PropertyName, Target, result);
                            Assert.IsNotNull(prop);

                            value = GetPropertyValue(prop, Target, result);
                            Assert.AreEqual(p.PropertyValue, value);

                            Logger.Information("Assertion [{0}]=[{1}]", p.PropertyValue.ToString(), value.ToString());
                            break;

                        case MemberTypes.Method:
                            var propMethod = GetMethod(p.PropertyName, Target, result);
                            Assert.IsNotNull(propMethod);

                            value = GetMethodResult(propMethod, Target, result);
                            Assert.AreEqual(p.PropertyValue, value);
                            Logger.Information("Assertion [{0}]=[{1}]", p.PropertyValue.ToString(), value.ToString());
                            break;
                        case MemberTypes.Custom:
                            if (p.PropertyValue.GetType() == typeof (Func<object, object, object>))
                            {
                                prop = GetProperty(p.PropertyName, Target, result);
                                Assert.IsNotNull(prop);

                                value = GetPropertyValue(prop, Target, result);
                                //Assert.AreEqual(p.PropertyValue, value);

                                value = ((Func<object, object, object>) p.PropertyValue)
                                    .Invoke(value, prop);
                                Assert.IsTrue((bool)value);
                                Logger.Information("Assertion [Func<>]=[{0}]", value.ToString());
                            }
                            break;
                    }
                });

            return this;
        }

        private MethodInfo GetMethod(string name, params object[] sources)
        {
            return sources
                .Select(
                    source => source.GetType()
                        .GetMethods()
                        .First(x => x.Name == name))
                .FirstOrDefault(prop => null != prop);
        }

        private object GetMethodResult(MethodInfo method, params object[] sources)
        {
            return sources.Select(
                source => method.Invoke(Target, null))
                    .FirstOrDefault(value => null != value);
        }

        private PropertyInfo GetProperty(string name, params object[] sources)
        {
            return sources
                .Select(
                    source => source.GetType()
                        .GetProperties()
                        .First(x => x.Name == name))
                .FirstOrDefault(prop => null != prop);
        }

        private object GetPropertyValue(PropertyInfo property, params object[] sources)
        {
            return sources
                .Select(
                    property
                        .GetValue)
                .FirstOrDefault(value => null != value);
        }

        #endregion private methods

        #region Inner classes

        /// <summary>
        /// Placeholder on wich call the method name
        /// </summary>
        public class OnIt : BaseIt
        {
            public OnIt(It parent)
                : base(parent)
            {
            }

#if ENGLISH_LANGUAGE
            /// <summary>
            /// Specifies the method to be called
            /// </summary>
            /// <param name="method">method name</param>
            /// <returns>Next object in fluent sequence</returns>
            public CallIt Call(string method)
            {
                Assert.IsNotNull(method);
                Assert.IsFalse(string.IsNullOrEmpty(method));
                Parent.MethodName = method;
                Parent.Logger.Information("Method: {0}", method);
                return new CallIt(Parent);
            }
#endif

#if SPANISH_LANGUAGE
            /// <summary>
            /// Specifies the method to be called
            /// </summary>
            /// <param name="method">method name</param>
            /// <returns>Next object in fluent sequence</returns>
            public CallIt Llama(string method)
            {
                Assert.IsNotNull(method);
                Assert.IsFalse(string.IsNullOrEmpty(method));
                Parent.MethodName = method;
                Parent.Logger.Information("Method: {0}", method);
                return new CallIt(Parent);
            }
#endif

        }

        /// <summary>
        /// Placeholder to provide the parameters to be used in the method call
        /// </summary>
        public class CallIt : BaseIt
        {
            public CallIt(It parent)
                : base(parent)
            {
            }

#if ENGLISH_LANGUAGE
            /// <summary>
            /// Specifies the parameters to be used on the method call
            /// </summary>
            /// <param name="parameters">single or multiple parameter objects</param>
            /// <returns>Next object in fluent sequence</returns>
            public WithParamsIt WithParams(params object[] parameters)
            {
                Parent.Parameters = parameters;
                parameters
                    .ToList()
                    .ForEach(p => Parent.Logger.Information("Parameter: {0}", p.ToString()));
                return new WithParamsIt(Parent);
            }
#endif

#if SPANISH_LANGUAGE
            /// <summary>
            /// Specifies the parameters to be used on the method call
            /// </summary>
            /// <param name="parameters">single or multiple parameter objects</param>
            /// <returns>Next object in fluent sequence</returns>
            public WithParamsIt ConParametros(params object[] parameters)
            {
                Parent.Parameters = parameters;
                parameters
                    .ToList()
                    .ForEach(p => Parent.Logger.Information("Parameter: {0}", p.ToString()));
                return new WithParamsIt(Parent);
            }
#endif

        }

        /// <summary>
/// Placeholder to provide the properties to verify
/// </summary>
public class WithParamsIt : BaseIt
        {
            public WithParamsIt(It parent)
                :base(parent)
            {
            }

#if ENGLISH_LANGUAGE
            /// <summary>
            /// Specifies the property names and values in optionally is they are a property name
            /// or a method name. i.e. as used by reflection
            /// </summary>
            /// <param name="properties"></param>
            /// <returns>Parent fluent object</returns>
            public It ThenVerifyProps(params NameValue[] properties)
            {
                Assert.IsNotNull(properties);
                Assert.IsTrue(properties.Any());
                Parent.Properties = properties;
                properties
                    .ToList()
                    .ForEach(p => Parent.Logger.Information(p.ToString()));
                return Parent.RunTest();
            }
#endif

#if SPANISH_LANGUAGE
            /// <summary>
            /// Specifies the property names and values in optionally is they are a property name
            /// or a method name. i.e. as used by reflection
            /// </summary>
            /// <param name="properties"></param>
            /// <returns>Parent fluent object</returns>
            public It LuegoVerificaPropiedades(params NameValue[] properties)
            {
                Assert.IsNotNull(properties);
                Assert.IsTrue(properties.Any());
                Parent.Properties = properties;
                properties
                    .ToList()
                    .ForEach(p => Parent.Logger.Information(p.ToString()));
                return Parent.RunTest();
            }
#endif

        }

        /// <summary>
        /// Placeholder to make the test run
        /// </summary>
        public class ThenVerifyPropsIt : BaseIt
        {
            public ThenVerifyPropsIt(It parent)
                :base(parent)
            {
            }

#if ENGLISH_LANGUAGE
            /// <summary>
            /// Does the actual run test
            /// </summary>
            /// <returns>Parent fluent object</returns>
            public It RunTest()
            {
                return Parent.RunTest();
            }
#endif

#if SPANISH_LANGUAGE
            /// <summary>
            /// Does the actual run test
            /// </summary>
            /// <returns>Parent fluent object</returns>
            public It CorreLaPrueba()
            {
                return Parent.RunTest();
            }
#endif

        }

        #endregion Inner classes
    }
}
