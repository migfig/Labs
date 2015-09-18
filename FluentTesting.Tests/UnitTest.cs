using System;
using System.Reflection;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentTesting.Tests
{
    [TestClass]
    public class UnitTest
    {

#if ENGLISH_LANGUAGE

        [TestMethod]
        public void It_Buildup_Test()
        {
            var item = new StringBuilder();

            "Test fluent interface building up"
                .On(item)
                
                .Call("Append")
                .WithParams("Hello")
                .ThenVerifyProps(new NameValue("Length", 5))
                .WithOtherParams(" World")
                .ThenVerifyProps(
                    new NameValue("Length", 11),
                    new NameValue("ToString", "Hello World", MemberTypes.Method))
                .WithOtherParams("!")
                .ThenVerifyProps(new NameValue("Length",
                    new Func<object, object, object>((len, result) => 12 == (int)len), MemberTypes.Custom))
                ;
        }

        [TestMethod]
        public void Instance_Buildup_Test()
        {
            var item = "Hello World!";
            var result =
            "Test fluent interface for instance build up"
                .With(item)
                .VerifyProperty("Length")
                .IsEqualTo(item.Length)
                .OrIsGreatherThan(10)
                .OrIsNotEqualTo(2)
                .AndIsGreatherThanOrEqual(9)
                .VerifyResults()
                .ResultsPassed
                ;

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Instance_Buildup_Object_Test()
        {
            var item = new TestClass
            {
                BoolProperty = true,
                IntProperty = 10,
                DateTimeProperty = DateTime.Now,
                StringProperty = "Sample"
            };

            var results =
            "Test fluent interface for class instance build up"
                .With(item)
                .VerifyProperty("IntProperty")
                .IsEqualTo(10)
                .VerifyResults()
                .VerifyProperty("BoolProperty")
                .IsNotEqualTo(false)
                .VerifyResults()
                .VerifyProperty("DateTimeProperty")
                .IsLessThanOrEqual(DateTime.Now)
                .VerifyResults()
                .VerifyProperty("StringProperty")
                .IsEqualTo("Sample")
                .VerifyResults()
                .ResultsPassed;

            Assert.IsTrue(results);
        }
#endif

#if SPANISH_LANGUAGE

        [TestMethod]
        public void It_Prueba_de_Construccion()
        {
            var objeto = new StringBuilder();

            "Probar la construccion de la interfaz fluida"
                .Sobre(objeto)
                .Llama("Append")
                .ConParametros("Hola")
                .LuegoVerificaPropiedades(new NameValue("Length", 4))
                .CorreLaPrueba()
                .ConOtrosParametros(" Mundo")
                .LuegoVerificaPropiedades(
                    new NameValue("Length", 10),
                    new NameValue("ToString", "Hola Mundo", MemberTypes.Method))
                .CorreLaPrueba()
                ;
        }

#endif

    }

    public class TestClass
    {
        public int IntProperty { get; set; }
        public string StringProperty { get; set; }
        public bool BoolProperty { get; set; }
        public DateTime DateTimeProperty { get; set; }
    }
}
