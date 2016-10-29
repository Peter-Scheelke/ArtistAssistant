using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtistAssistant.DrawableObject;
using System.Drawing;
using System.Reflection;
using System.Collections.Generic;

namespace ArtistAssistantTests
{
    [TestClass]
    public class ArtistAssistantTests
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
            DrawableObject drawableObject = DrawableObject.Create(ImageType.Cloud, new Point(0, 0), new Size(10, 10));
            Assert.IsTrue(drawableObject.Id == 0);
            Assert.IsTrue(object.ReferenceEquals(drawableObject.Image, ImagePool.GetImage(ImageType.Cloud)));
            Assert.IsFalse(object.ReferenceEquals(drawableObject.Image, ImagePool.GetImage(ImageType.Mountain)));
            Assert.IsFalse(drawableObject.Image == null);

            drawableObject = DrawableObject.Create(ImageType.Mountain, new Point(0, 0), new Size(10, 10));
            Assert.IsTrue(drawableObject.Id == 1);
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

            Assert.IsFalse(drawableObject.Selected);
            drawableObject.Select();
            Assert.IsTrue(drawableObject.Selected);
            drawableObject.Deselect();
            Assert.IsFalse(drawableObject.Selected);

            drawableObject.ImageType = ImageType.Pond;
            Assert.IsTrue(drawableObject.ImageType == ImageType.Pond);
        }

        [TestMethod]
        public void TestDrawableObjectList()
        {
            // Set stuff up
            TestDrawableObjectListObserver observer = new TestDrawableObjectListObserver();
            DrawableObjectList list = DrawableObjectList.Create();
            DrawableObject drawable = DrawableObject.Create(ImageType.Cloud, new Point(3, 3), new Size(10, 10));
            observer.Unsubscriber = list.Subscribe(observer);

            // Make sure that adding a drawable notifies the test observer
            list.Add(drawable);
            Assert.IsTrue(observer.WasNotified);
            observer.WasNotified = false;

            // Make sure that changing an item in the list notifies the test observer
            drawable.ImageType = ImageType.Mountain;
            Assert.IsTrue(observer.WasNotified);
            observer.WasNotified = false;

            drawable.Location = new Point(5, 5);
            Assert.IsTrue(observer.WasNotified);
            observer.WasNotified = false;

            drawable.Size = new Size(15, 15);
            Assert.IsTrue(observer.WasNotified);
            observer.WasNotified = false;

            drawable.Select();
            Assert.IsTrue(observer.WasNotified);
            observer.WasNotified = false;

            drawable.Deselect();
            Assert.IsTrue(observer.WasNotified);
            observer.WasNotified = false;

            // Make sure unsubscribing works
            observer.Unsubscriber.Dispose();
            drawable.ImageType = ImageType.Pond;
            Assert.IsFalse(observer.WasNotified);

            // Add a bunch of DrawableObjects to the list (also make a separate list of them for later use)
            List<DrawableObject> items = new List<DrawableObject>();
            for (int i = 0; i < 10; ++i)
            {
                items.Add(DrawableObject.Create(ImageType.Cloud, new Point(3, 3), new Size(10, 10)));
                list.Add(DrawableObject.Create(ImageType.Cloud, new Point(3, 3), new Size(10, 10)));
            }

            // Resubscribe the observer
            observer.Unsubscriber = list.Subscribe(observer);

            // Make sure changing items in the list notifies correctly for more items
            for (int i = 0; i < list.Count; ++i)
            {
                list[i].ImageType = ImageType.Pine;
                Assert.IsTrue(observer.WasNotified);
                observer.WasNotified = false;
            }

            // Make sure that clearing the list notifies the observer
            list.Clear();
            Assert.IsTrue(observer.WasNotified);
            observer.WasNotified = false;

            // See if adding a range of items works correctly
            list.AddRange(items);
            Assert.IsTrue(observer.WasNotified);
            observer.WasNotified = false;
            for (int i = 0; i < list.Count; ++i)
            {
                list[i].ImageType = ImageType.Pine;
                Assert.IsTrue(observer.WasNotified);
                observer.WasNotified = false;
            }

            // Clear and make sure that messing with the outside list
            // of DrawableObjects doesn't notify
            list.Clear();
            Assert.IsTrue(observer.WasNotified);
            observer.WasNotified = false;

            for (int i = 0; i < items.Count; ++i)
            {
                items[i].ImageType = ImageType.Pine;
                Assert.IsFalse(observer.WasNotified);
            }

            // Insert a new drawable into the middle of the list
            list.AddRange(items);
            Assert.IsTrue(observer.WasNotified);
            observer.WasNotified = false;
            drawable = DrawableObject.Create(ImageType.Rain, new Point(3, 3), new Size(10, 10));
            list.Insert(5, drawable);
            Assert.IsTrue(observer.WasNotified);
            observer.WasNotified = false;
            drawable.ImageType = ImageType.Pine;
            Assert.IsTrue(observer.WasNotified);
            observer.WasNotified = false;

            // Insert a range of objects
            list.InsertRange(7, items);
            Assert.IsTrue(observer.WasNotified);
            observer.WasNotified = false;

            for (int i = 0; i < list.Count; ++i)
            {
                list[i].ImageType = ImageType.Pine;
                Assert.IsTrue(observer.WasNotified);
                observer.WasNotified = false;
            }

            // Test removal
            list.Remove(drawable);
            Assert.IsTrue(observer.WasNotified);
            observer.WasNotified = false;

            drawable.ImageType = ImageType.Rain;
            Assert.IsFalse(observer.WasNotified);

            list.Clear();
            Assert.IsTrue(observer.WasNotified);
            observer.WasNotified = false;

            // Test range removal
            list.AddRange(items);
            Assert.IsTrue(observer.WasNotified);
            observer.WasNotified = false;

            list.RemoveRange(3, 3);
            Assert.IsTrue(observer.WasNotified);
            observer.WasNotified = false;
            int count = 0;
            foreach (var item in list)
            {
                item.ImageType = ImageType.Tree;
                if (observer.WasNotified)
                {
                    ++count;
                    observer.WasNotified = false;
                }
            }

            Assert.IsTrue(count == 7);

            // Test indexed removal
            var itemAtIndex = list[4];
            list.RemoveAt(4);
            Assert.IsTrue(observer.WasNotified);
            observer.WasNotified = false;
            itemAtIndex.ImageType = ImageType.Mountain;
            Assert.IsFalse(observer.WasNotified);
        }

        [TestMethod]
        public void TestGetIdFromPoint()
        {
            Point testPoint = new Point(50, 50);

            DrawableObject obj = DrawableObject.Create(ImageType.Cloud, new Point(0, 0), new Size(3, 3));

            DrawableObjectList list = DrawableObjectList.Create();
            Assert.IsTrue(list.GetIdFromLocation(testPoint) == -1);

            list.Add(obj);
            Assert.IsTrue(list.GetIdFromLocation(testPoint) == -1);

            obj = DrawableObject.Create(ImageType.Cloud, new Point(30, 30), new Size(19, 19));
            list.Add(obj);
            Assert.IsTrue(list.GetIdFromLocation(testPoint) == -1);

            obj.Size = new Size(20, 20);
            Assert.IsTrue(list.GetIdFromLocation(testPoint) == obj.Id);

            list[0].Location = obj.Location;
            list[0].Size = obj.Size;
            Assert.IsTrue(list.GetIdFromLocation(testPoint) == obj.Id && obj.Id != list[0].Id);

            obj.Size = new Size(19, 19);
            Assert.IsTrue(list.GetIdFromLocation(testPoint) == list[0].Id && obj.Id != list[0].Id);
        }

        [TestMethod]
        public void TestRenderOrder()
        {
            DrawableObjectList list = DrawableObjectList.Create();
            DrawableObject obj = DrawableObject.Create(ImageType.Cloud, new Point(0, 0), new Size(1, 1));
            Assert.IsTrue(list.RenderOrder.Count == 0);
            list.Add(obj);
            Assert.IsTrue(list.RenderOrder.Count == 1);

            obj = DrawableObject.Create(ImageType.Cloud, new Point(0, 0), new Size(2, 2));
            list.Add(obj);
            Assert.IsTrue(list.RenderOrder.Count == 2);

            Assert.IsTrue(object.ReferenceEquals(list.RenderOrder[1], obj));
            list[0].Size = new Size(3, 3);
            Assert.IsTrue(object.ReferenceEquals(list.RenderOrder[0], obj));

            obj.Size = new Size(4, 4);
            Assert.IsTrue(object.ReferenceEquals(list.RenderOrder[1], obj));

            DrawableObject obj2 = DrawableObject.Create(ImageType.Cloud, new Point(0, 0), new Size(12, 12));
            list.Add(obj2);
            Assert.IsTrue(object.ReferenceEquals(obj2, list.RenderOrder[2]));

            list[0].Size = new Size(42, 42);
            Assert.IsTrue(object.ReferenceEquals(obj2, list.RenderOrder[1]));
            list[1].Size = new Size(41, 41);
            Assert.IsTrue(object.ReferenceEquals(obj2, list.RenderOrder[0]));

            list[2].Size = new Size(43, 43);
            Assert.IsTrue(object.ReferenceEquals(obj2, list.RenderOrder[2]));

            list[0].Select();
            Assert.IsTrue(list[0].Selected);
            list[1].Select();
            list[2].Select();
            Assert.IsFalse(list[0].Selected);
            Assert.IsFalse(list[1].Selected);
            Assert.IsTrue(list[2].Selected);
            list[2].Deselect();
            foreach (var item in list)
            {
                Assert.IsFalse(item.Selected);
            }
        }
    }
}
