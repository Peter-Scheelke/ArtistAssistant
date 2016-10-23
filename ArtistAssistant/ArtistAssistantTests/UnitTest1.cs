using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtistAssistant.DrawableObject;
using System.Drawing;
using System.Reflection;

namespace ArtistAssistantTests
{
    [TestClass]
    public class UnitTest1
    {
        /// <summary>
        /// This test should be run first since it tests a singleton
        /// </summary>
        [TestMethod]
        public void TestImagePool()
        {
            Assert.IsTrue(ImagePool.Count == 0);
            bool failed = false;

            // Make sure that the ImagePool never creates more than one instance
            // of an image resource, that it can find the appropriate resources,
            // and that it doesn't throw any exceptions.
            try
            {
                Image image = ImagePool.GetImage(ImageType.Cloud).Image;
                Assert.IsTrue(ImagePool.Count == 1);
                Assert.IsFalse(image == null);
                image = ImagePool.GetImage(ImageType.Cloud).Image;
                Assert.IsTrue(ImagePool.Count == 1);
                Assert.IsFalse(image == null);

                image = ImagePool.GetImage(ImageType.Mountain).Image;
                Assert.IsTrue(ImagePool.Count == 2);
                Assert.IsFalse(image == null);
                image = ImagePool.GetImage(ImageType.Mountain).Image;
                Assert.IsTrue(ImagePool.Count == 2);
                Assert.IsFalse(image == null);

                image = ImagePool.GetImage(ImageType.Pine).Image;
                Assert.IsTrue(ImagePool.Count == 3);
                Assert.IsFalse(image == null);
                image = ImagePool.GetImage(ImageType.Pine).Image;
                Assert.IsTrue(ImagePool.Count == 3);
                Assert.IsFalse(image == null);

                image = ImagePool.GetImage(ImageType.Pond).Image;
                Assert.IsTrue(ImagePool.Count == 4);
                Assert.IsFalse(image == null);
                image = ImagePool.GetImage(ImageType.Pond).Image;
                Assert.IsTrue(ImagePool.Count == 4);
                Assert.IsFalse(image == null);

                image = ImagePool.GetImage(ImageType.Rain).Image;
                Assert.IsTrue(ImagePool.Count == 5);
                Assert.IsFalse(image == null);
                image = ImagePool.GetImage(ImageType.Rain).Image;
                Assert.IsTrue(ImagePool.Count == 5);
                Assert.IsFalse(image == null);

                image = ImagePool.GetImage(ImageType.Tree).Image;
                Assert.IsTrue(ImagePool.Count == 6);
                Assert.IsFalse(image == null);
                image = ImagePool.GetImage(ImageType.Tree).Image;
                Assert.IsTrue(ImagePool.Count == 6);
                Assert.IsFalse(image == null);
            }
            catch (Exception)
            {
                failed = true;
            }

            Assert.IsFalse(failed);
            Assert.IsTrue(ImagePool.Count == 6);
        }
    }
}
