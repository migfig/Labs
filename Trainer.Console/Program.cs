using Common;
using System;
using Trainer.Domain;

namespace Trainer.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileName = @"C:\Code\RelatedRecords.Tests\Trainer.Domain\common.xml";
            var components = XmlHelper<Components>.Load(fileName);
            foreach(var component in components.Component)
            {
                if(string.IsNullOrEmpty(component.Id))
                {
                    component.Id = Guid.NewGuid().ToString("N");
                }
            }
            XmlHelper<Components>.Save(fileName, components);
        }
    }
}
