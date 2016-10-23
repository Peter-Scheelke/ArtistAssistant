using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtistAssistant.DrawableObject;
using System.Drawing;
using System.Reflection;
using DrawableObject;

namespace ArtistAssistantTests
{
    [TestClass]
    public class UnitTests
    {
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
                Image image = ImagePool.GetImage(ImageType.Cloud);
                Assert.IsTrue(ImagePool.Count == 1);
                Assert.IsFalse(image == null);
                image = ImagePool.GetImage(ImageType.Cloud);
                Assert.IsTrue(ImagePool.Count == 1);
                Assert.IsFalse(image == null);

                image = ImagePool.GetImage(ImageType.Mountain);
                Assert.IsTrue(ImagePool.Count == 2);
                Assert.IsFalse(image == null);
                image = ImagePool.GetImage(ImageType.Mountain);
                Assert.IsTrue(ImagePool.Count == 2);
                Assert.IsFalse(image == null);

                image = ImagePool.GetImage(ImageType.Pine);
                Assert.IsTrue(ImagePool.Count == 3);
                Assert.IsFalse(image == null);
                image = ImagePool.GetImage(ImageType.Pine);
                Assert.IsTrue(ImagePool.Count == 3);
                Assert.IsFalse(image == null);

                image = ImagePool.GetImage(ImageType.Pond);
                Assert.IsTrue(ImagePool.Count == 4);
                Assert.IsFalse(image == null);
                image = ImagePool.GetImage(ImageType.Pond);
                Assert.IsTrue(ImagePool.Count == 4);
                Assert.IsFalse(image == null);

                image = ImagePool.GetImage(ImageType.Rain);
                Assert.IsTrue(ImagePool.Count == 5);
                Assert.IsFalse(image == null);
                image = ImagePool.GetImage(ImageType.Rain);
                Assert.IsTrue(ImagePool.Count == 5);
                Assert.IsFalse(image == null);

                image = ImagePool.GetImage(ImageType.Tree);
                Assert.IsTrue(ImagePool.Count == 6);
                Assert.IsFalse(image == null);
                image = ImagePool.GetImage(ImageType.Tree);
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

        [TestMethod]
        public void TestDrawableObjectCreation()
        {
            DrawableObject.DrawableObject drawableObject = DrawableObject.DrawableObject.Create(ImageType.Cloud, new Point(0, 0), new Size(10, 10));
            Assert.IsTrue(object.ReferenceEquals(drawableObject.Image, ImagePool.GetImage(ImageType.Cloud)));
            Assert.IsFalse(object.ReferenceEquals(drawableObject.Image, ImagePool.GetImage(ImageType.Mountain)));
            Assert.IsFalse(drawableObject.Image == null);

            drawableObject = DrawableObject.DrawableObject.Create(ImageType.Mountain, new Point(0, 0), new Size(10, 10));
            Assert.IsFalse(object.ReferenceEquals(drawableObject.Image, ImagePool.GetImage(ImageType.Cloud)));
            Assert.IsTrue(object.ReferenceEquals(drawableObject.Image, ImagePool.GetImage(ImageType.Mountain)));
            Assert.IsFalse(drawableObject.Image == null);

            Assert.IsTrue(drawableObject.Location.X == 0);
            Assert.IsTrue(drawableObject.Location.Y == 0);

            Assert.IsTrue(drawableObject.Size.Width == 10);
            Assert.IsTrue(drawableObject.Size.Height == 10);

            drawableObject.ImageType = ImageType.Pond;
            Assert.IsFalse(object.ReferenceEquals(drawableObject.Image, ImagePool.GetImage(ImageType.Mountain)));
            Assert.IsTrue(object.ReferenceEquals(drawableObject.Image, ImagePool.GetImage(ImageType.Pond)));
            Assert.IsFalse(drawableObject.Image == null);

            drawableObject.Location = new Point(10, 15);
            Assert.IsTrue(drawableObject.Location.X == 10);
            Assert.IsTrue(drawableObject.Location.Y == 15);

            drawableObject.Size = new Size(15, 30);
            Assert.IsTrue(drawableObject.Size.Width == 15);
            Assert.IsTrue(drawableObject.Size.Height == 30);
        }
    }
}
