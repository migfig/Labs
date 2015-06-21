# Lab Experiments
## C# and Other Stuff ##

This is a series of lab experiment projects created for helping a developer in certain situations. They have been created just for **Fun and Joy** as of being a Developer.

The projects created up to this moment are:
> Fluent Testing
> 
> Related Records

They have been created from scratch using **MS Visual Studio**  on a **Microsoft Surface 3** with its corresponding **Type Cover**.
Also using **ReSharper** add-in.

## Fluent Testing Project ##

The **Fluent Testing** project is about hiding all of the assertion code from testing an object and making that process as Fluent as the **Natural Language**, English and Spanish in this case; all of this by using **.Net Reflection**.

The component classes are structured in such a way that helps the developer through intellisense, to choose the right methods that follow in the sequence.

The library supports English and Spanish languages. To enable one or the other, just turn on/off the corresponding language compilation constant at project properties level:


> ENGLISH_LANGUAGE
> 
> SPANISH_LANGUAGE  

Sample code in **English**:
    
    	[TestMethod]
        public void It_Buildup_Test()
        {
            var item = new StringBuilder();

            "Test fluent interface building up"
                .On(item)
                .Call("Append")
                .WithParams("Hello")
                .ThenVerifyProps(new NameValue("Length", 5))
                .RunTest()
                .WithOtherParams(" World")
                .ThenVerifyProps(
                    new NameValue("Length", 11),
                    new NameValue("ToString", "Hello World", MemberTypes.Method))
                .RunTest()
                .WithOtherParams("!")
                .ThenVerifyProps(new NameValue("Length",
                    new Func<object, object, object>((len, result) => 12 == (int)len), MemberTypes.Custom))
                .RunTest()
                ;
        }

Sample code in **Spanish**:
    
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

**What's missing**:
This is just a placeholder to start up building a very robust fluent testing light framework helper on top of any Testing Framework like **NUnit** or other well know frameworks, but useful enough for being used in real daily code testing.

## Related Records Project ##
The **Related Records** project is about making easy for a business developer/analyst to see given a database schema what table records are related to each other table records and let the user navigate back and forth and drilling down as much as possible through the data entities.

Related data may be exported as a JSON, XML or HTML string/file. It has the potential to be used as an Evidence result from the Testing documentation process.

**What's missing**: This is the start up placeholder for building a serious business data oriented application utility for daily mundane data review tasks. WPF windows client application has been created but at this moment is not bind to the related records library, it will be soon.
