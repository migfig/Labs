using Log.Common.Services.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trainer.Domain;

namespace Log.Common.Services.Tests
{
    [TestClass]
    public class ObjectHelperTests
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
        public void NewSlide_Created_WhenDeepCloningSlide()
        {
            #region src slide var
            var src = new Slide
            {
                Title = "Slide Title",
                Block = new ObservableCollection<RichTextBlock>
                {
                    new RichTextBlock
                    {
                        FontSize = 18,
                        FontWeight = "DemiBold",
                        LineHeight = 20
                    }
                }
            };
            #endregion

            var tgt = src.DeepClone();
            Assert.IsNotNull(tgt);
            Assert.AreEqual(src.Title, tgt.Title);
            Assert.IsNotNull(tgt.Block);
            Assert.IsTrue(tgt.Block.Any());
            Assert.AreEqual(src.Block.Count, tgt.Block.Count);
            var srtb = src.Block.FirstOrDefault();
            var trtb = tgt.Block.FirstOrDefault();
            Assert.IsNotNull(trtb);
            Assert.AreEqual(srtb.FontSize, trtb.FontSize);
            Assert.AreEqual(srtb.FontWeight, trtb.FontWeight);
            Assert.AreEqual(srtb.LineHeight, trtb.LineHeight);
        }
    }
}
