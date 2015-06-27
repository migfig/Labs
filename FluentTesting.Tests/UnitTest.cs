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
}
