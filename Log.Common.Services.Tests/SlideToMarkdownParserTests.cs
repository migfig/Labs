using Common;
using Log.Common.Services.Common;
using Log.Common.Services.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;
using Trainer.Domain;

namespace Log.Common.Services.Tests
{
    [TestClass]
    public class SlideToMarkdownParserTests
    {
        [TestInitialize]
        public void Setup()
        {
        }

        [TestCleanup]
        public void Teardown()
        {
        }

        [TestMethod]
        public void Markdown_Generated_WhenValidSlideProvided()
        {
            #region vars

            var xml = @"<?xml version='1.0' encoding='utf-8' ?>
  <Slide Title='Dependency Injection with Windsor Castle' Margin='-200,0,0,0'>
      <RichTextBlock FontSize='20' FontWeight='DemiBold'>     
           <Paragraph>     
             <Bold>Windsor Castle Container</Bold>        
           </Paragraph>
           <Paragraph>
            <Run>Dependency Injection</Run>
           </Paragraph>
           </RichTextBlock>
        
           <Component Id='94928827-5412-4BC8-A279-EB88E21AC64F' Name='Windsor Castle' IsBrowsable='true' Action='View' TargetFile='IoC\ServiceInstaller.cs' TargetProject='Code.Service' Language='csharp'>
                   
             <Code>
                   <![CDATA[ using Castle.Windsor;
            using Castle.Windsor.Installer;
            using Castle.MicroKernel.Registration;
            using Castle.MicroKernel.SubSystems.Configuration;

            var container = new WindsorContainer();
            container.Install(FromAssembly.This());
            //usage:
            var codeServices = container.Resolve<ICodeServices>();

  public class ServiceInstaller : IWindsorInstaller
        {
            public void Install(IWindsorContainer container, IConfigurationStore store)
            {
                container.Register(
                    Component.For<IGenericServices<domain.Presentation>>()
                        .ImplementedBy<GenericServices<domain.Presentation>>()
                        .DependsOn(Dependency.OnAppSettingsValue('path'),
                          Dependency.OnAppSettingsValue('pattern'))
                        .LifestyleSingleton(),

                    Classes.FromThisAssembly()
                        .BasedOn<ApiController>()
                        .LifestyleScoped()
                    );
            }
        }]]>
      </Code>
    </Component>
  </Slide>".Trim().Replace("'", "\"");

            var expectedMarkdown = @"
    #Dependency Injection with Windsor Castle#
    
      *Windsor Castle Container*
  Dependency Injection
    ```csharp
 using Castle.Windsor;
            using Castle.Windsor.Installer;
            using Castle.MicroKernel.Registration;
            using Castle.MicroKernel.SubSystems.Configuration;

            var container = new WindsorContainer();
            container.Install(FromAssembly.This());
            //usage:
            var codeServices = container.Resolve<ICodeServices>();

  public class ServiceInstaller : IWindsorInstaller
        {
            public void Install(IWindsorContainer container, IConfigurationStore store)
            {
                container.Register(
                    Component.For<IGenericServices<domain.Presentation>>()
                        .ImplementedBy<GenericServices<domain.Presentation>>()
                        .DependsOn(Dependency.OnAppSettingsValue('path'),
                          Dependency.OnAppSettingsValue('pattern'))
                        .LifestyleSingleton(),

                    Classes.FromThisAssembly()
                        .BasedOn<ApiController>()
                        .LifestyleScoped()
                    );
            }
        }
      
    ```
  ".Replace("'","\"");

            #endregion

            var parser = ParserFactory.CreateSlideParser(new MockApiServiceString());
            Assert.IsNotNull(parser);

            var markdown = parser.Parse(XmlHelper2<Slide>.LoadFromString(xml)).GetAwaiter().GetResult();

            TextHelper.SaveFileContent("expected-markdown.txt", expectedMarkdown);
            TextHelper.SaveFileContent("actual-markdown.txt", markdown);
            Assert.AreEqual(expectedMarkdown.Trim(), markdown.Trim());
        }
    }

    #region mock api service 

    internal class MockApiServiceString : IGenericApiService<string>
    {
        #region unused methods

        public Task<bool> AddItem(string item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddItems(IEnumerable<string> items)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<string>> GetItems(string url)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveItem(string item, string propertyName)
        {
            throw new NotImplementedException();
        }

        #endregion

        public Task<string> TransformXml(XElement xml, string styleSheet)
        {
            var xslt = new XslCompiledTransform(true);
            xslt.Load(@"C:\Code\RelatedRecords.Tests\Log.Common.Services\Common\" + styleSheet);

            var builder = new StringBuilder();
            using (var stream =  new StringWriter(builder))
            {
                xslt.Transform(xml.CreateReader(), null, stream);
            }

            return Task.FromResult(builder.ToString());
        }
    }

    #endregion
}