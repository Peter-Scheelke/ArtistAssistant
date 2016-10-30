using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtistAssistant.DrawableObject;
using System.Drawing;
using System.Reflection;
using System.Collections.Generic;
using ArtistAssistant.Command;
using System.Windows.Forms;

namespace ArtistAssistantTests
{
    [TestClass]
    public class ArtistAssistantTests
    {
        /// <summary>
        /// Make sure that the ImagePool never creates more than one instance
        /// of an image, that it can find the appropriate resources,
        /// and that it doesn't throw any exceptions while doing so.
        /// </summary>
        [TestMethod]
        public void TestImagePool()
        {
            Assert.IsTrue(ImagePool.Count == 0);
            bool failed = false;

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

        /// <summary>
        /// Make sure that <see cref="DrawableObject"/>s can be created correctly
        /// </summary>
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

        /// <summary>
        /// Make sure that <see cref="DrawableObjectList"/>s can be created
        /// and that they subscribe to their contained <see cref="DrawableObject"/>s
        /// </summary>
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

        /// <summary>
        /// Make sure the GetIdFromLocation method correctly gets the Id of the
        /// <see cref="DrawableObject"/> that is highest in the render order and
        /// contains the given <see cref="Point"/>
        /// </summary>
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

            // Make sure it works when the render order gets changed
            list[1].Size = new Size(20, 20);
            Assert.IsTrue(list.GetIdFromLocation(testPoint) == list[1].Id);

            list.BringToIndex(0, 1);
            Assert.IsFalse(list.GetIdFromLocation(testPoint) == list[1].Id);

        }

        /// <summary>
        /// Make sure the Render Order of <see cref="DrawableObjectList"/>s
        /// can be correctly changed with the BringToIndex method
        /// </summary>
        [TestMethod]
        public void TestRenderOrder()
        {
            DrawableObjectList list = DrawableObjectList.Create();
            List<DrawableObject> objList = new List<DrawableObject>();

            for (int i = 0; i < 10; ++i)
            {
                objList.Add(DrawableObject.Create(ImageType.Cloud, new Point(0, 0), new Size(1, 1)));
            }

            foreach (var obj in objList)
            {
                list.Add(obj);
            }

            int lastId = list[list.Count - 1].Id;
            int secondLastId = list[list.Count - 2].Id;

            int firstId = list[0].Id;
            int secondId = list[1].Id;

            Assert.IsTrue(list.RenderOrder[0].Id == firstId);
            Assert.IsTrue(list.RenderOrder[1].Id == secondId);

            Assert.IsTrue(list[list.Count - 2].Id == secondLastId);
            Assert.IsTrue(list[list.Count - 1].Id == lastId);

            // This should slide the second to last element down to
            // the second index and slide 2 -> Count - 1 up once
            list.BringToIndex(list.Count - 2, 1);

            Assert.IsTrue(list.RenderOrder[0].Id == firstId);
            Assert.IsTrue(list[list.Count - 1].Id == lastId);

            Assert.IsTrue(list.RenderOrder[1].Id == secondLastId);
            Assert.IsTrue(list.RenderOrder[2].Id == secondId);

            // This should slide the second element back to its original position,
            // leaving the DrawableObjectList in its original order
            list.BringToIndex(1, list.Count - 2);

            Assert.IsTrue(list.RenderOrder[0].Id == firstId);
            Assert.IsTrue(list.RenderOrder[1].Id == secondId);

            Assert.IsTrue(list[list.Count - 2].Id == secondLastId);
            Assert.IsTrue(list[list.Count - 1].Id == lastId);
        }

        /// <summary>
        /// Make sure the <see cref="AddCommand"/> is working correctly
        /// </summary>
        [TestMethod]
        public void TestAddCommand()
        {
            // Set up
            DrawableObjectList list = DrawableObjectList.Create();

            // Create a command
            AddCommand command = AddCommand.Create(list, ImageType.Cloud, new Point(5, 5), new Size(5, 5));

            // The list should be empty
            Assert.IsTrue(list.Count == 0);

            // The list should have one item
            command.Execute();
            Assert.IsTrue(list.Count == 1);

            // The list should have nothing in it
            command.Undo();
            Assert.IsTrue(list.Count == 0);

            // Undoing on an empty list should throw an error
            bool didFail = false;
            try
            {
                command.Undo();
            }
            catch (Exception)
            {
                didFail = true;
            }

            Assert.IsTrue(didFail);

            didFail = false;

            // Test adding a bit more
            command.Execute();
            Assert.IsTrue(list.Count == 1);
            command.Execute();
            Assert.IsTrue(list.Count == 2);
            command.Undo();
            Assert.IsTrue(list.Count == 1);
            command.Undo();
            Assert.IsTrue(list.Count == 0);
        }

        /// <summary>
        /// Make sure the <see cref="BringToIndexCommand"/>
        /// is working correctly
        /// </summary>
        [TestMethod]
        public void TestBringToIndexCommand()
        {
            // Set up
            DrawableObjectList list = DrawableObjectList.Create();

            List<DrawableObject> objList = new List<DrawableObject>();

            for (int i = 0; i < 10; ++i)
            {
                objList.Add(DrawableObject.Create(ImageType.Cloud, new Point(0, 0), new Size(1, 1)));
            }

            foreach (var obj in objList)
            {
                list.Add(obj);
            }

            // Create a command
            BringToIndexCommand command = BringToIndexCommand.Create(list, 0, 8);

            int id = list.RenderOrder[0].Id;

            // This should move list[0] to the 8th index and slide everything else down
            command.Execute();
            Assert.IsTrue(list.RenderOrder[8].Id == id);

            // This should return the list to its original state
            command.Undo();
            Assert.IsTrue(list.RenderOrder[0].Id == id);

            // Create a command to test sliding everything the other way
            id = list.RenderOrder[8].Id;
            command = BringToIndexCommand.Create(list, 8, 0);

            // This should move list[8] to the front of the list and slide everything else up
            command.Execute();
            Assert.IsTrue(list.RenderOrder[0].Id == id);

            // This should return the list to its original state
            command.Undo();
            Assert.IsTrue(list.RenderOrder[8].Id == id);
        }

        /// <summary>
        /// Make sure the <see cref="RemoveCommand"/> is working correctly
        /// </summary>
        [TestMethod]
        public void TestRemoveCommand()
        {
            // Set up
            DrawableObjectList list = DrawableObjectList.Create();

            List<DrawableObject> objList = new List<DrawableObject>();

            for (int i = 0; i < 10; ++i)
            {
                objList.Add(DrawableObject.Create(ImageType.Cloud, new Point(0, 0), new Size(1, 1)));
            }

            foreach (var obj in objList)
            {
                list.Add(obj);
            }

            DrawableObject third = list.RenderOrder[3];

            // Create a command
            RemoveCommand removeCommand = RemoveCommand.Create(list, third.Id);

            // This should remove third from the list
            removeCommand.Execute();
            Assert.IsFalse(third.Id == list.RenderOrder[3].Id);

            // This should add third back to the list at the same index
            removeCommand.Undo();
            Assert.IsTrue(third.Id == list.RenderOrder[3].Id);
        }

        /// <summary>
        /// Make sure the <see cref="SelectCommand"/> is
        /// working correctly
        /// </summary>
        [TestMethod]
        public void TestSelectCommand()
        {
            // Set up
            DrawableObjectList list = DrawableObjectList.Create();

            List<DrawableObject> objList = new List<DrawableObject>();

            for (int i = 0; i < 10; ++i)
            {
                objList.Add(DrawableObject.Create(ImageType.Cloud, new Point(0, 0), new Size(1, 1)));
            }

            foreach (var drawableObj in objList)
            {
                list.Add(drawableObj);
            }

            foreach (var drawableObj in list)
            {
                Assert.IsFalse(drawableObj.Selected);
            }

            DrawableObject obj = list[0];

            // Create a command
            SelectCommand command = SelectCommand.Create(list, obj);

            // Execute the command. Should select list[0]
            command.Execute();
            Assert.IsTrue(list[0].Selected);

            // Undo the command. Should deselect list[0]
            command.Undo();
            Assert.IsFalse(list[0].Selected);

            foreach (var drawableObj in list)
            {
                Assert.IsFalse(drawableObj.Selected);
            }

            // Select something in the list
            list[3].Select();
            Assert.IsFalse(list[0].Selected);
            Assert.IsTrue(list[3].Selected);

            // This should deselect list[3] and select list[0]
            command.Execute();
            Assert.IsTrue(list[0].Selected);
            Assert.IsFalse(list[3].Selected);

            // This should deselect list[0] and reselect list[3]
            command.Undo();
            Assert.IsFalse(list[0].Selected);
            Assert.IsTrue(list[3].Selected);
        }

        /// <summary>
        /// Make sure that the <see cref="DeselectCommand"/>
        /// is working correctly
        /// </summary>
        [TestMethod]
        public void TestDeselectCommand()
        {
            // Set up
            DrawableObjectList list = DrawableObjectList.Create();

            List<DrawableObject> objList = new List<DrawableObject>();

            for (int i = 0; i < 10; ++i)
            {
                objList.Add(DrawableObject.Create(ImageType.Cloud, new Point(0, 0), new Size(1, 1)));
            }

            foreach (var obj in objList)
            {
                list.Add(obj);
            }

            // Create command
            DeselectCommand command = DeselectCommand.Create(list);

            // Select something
            list[4].Select();

            // Execute the command
            command.Execute();

            // Nothing should be selected now
            foreach (var obj in list)
            {
                Assert.IsFalse(obj.Selected);
            }

            // This should reselect only list[4]
            command.Undo();
            Assert.IsTrue(list[4].Selected);

            foreach (var obj in list)
            {
                if (obj.Id != list[4].Id)
                {
                    Assert.IsFalse(obj.Selected);
                }
            }
        }

        /// <summary>
        /// Make sure that the <see cref="MoveCommand"/>
        /// is working correctly
        /// </summary>
        [TestMethod]
        public void TestMoveCommand()
        {
            // Set up
            DrawableObjectList list = DrawableObjectList.Create();

            List<DrawableObject> objList = new List<DrawableObject>();

            for (int i = 0; i < 10; ++i)
            {
                objList.Add(DrawableObject.Create(ImageType.Cloud, new Point(0, 0), new Size(1, 1)));
            }

            foreach (var obj in objList)
            {
                list.Add(obj);
            }

            // Create command
            MoveCommand command = MoveCommand.Create(list, list[3], new Point(10, 10));
            Assert.IsTrue(list[3].Location.X == 0 && list[3].Location.Y == 0);

            // This should move list[3] to (10, 10)
            command.Execute();
            Assert.IsTrue(list[3].Location.X == 10 && list[3].Location.Y == 10);

            // This should move list[3] back to (0, 0)
            command.Undo();
            Assert.IsTrue(list[3].Location.X == 0 && list[3].Location.Y == 0);

            command = MoveCommand.Create(list, list[4], new Point(20, 20));

            // There should be no position to go back to
            command.Undo();
            Assert.IsTrue(list[4].Location.X == 0 && list[4].Location.Y == 0);
        }

        /// <summary>
        /// Make sure the <see cref="ScaleCommand"/> works correctly
        /// </summary>
        [TestMethod]
        public void TestScaleCommand()
        {
            // Set up
            DrawableObjectList list = DrawableObjectList.Create();

            List<DrawableObject> objList = new List<DrawableObject>();

            for (int i = 0; i < 10; ++i)
            {
                objList.Add(DrawableObject.Create(ImageType.Cloud, new Point(0, 0), new Size(1, 1)));
            }

            foreach (var obj in objList)
            {
                list.Add(obj);
            }

            // Create a command
            ScaleCommand command = ScaleCommand.Create(list, list[5], new Size(15, 15));

            Assert.IsTrue(list[5].Size.Width == 1 && list[5].Size.Height == 1);

            // This should do nothing
            command.Undo();
            Assert.IsTrue(list[5].Size.Width == 1 && list[5].Size.Height == 1);

            // This should change the size to (15, 15)
            command.Execute();
            Assert.IsTrue(list[5].Size.Width == 15 && list[5].Size.Height == 15);

            // This should change the size back to (1, 1)
            command.Undo();
            Assert.IsTrue(list[5].Size.Width == 1 && list[5].Size.Height == 1);
        }

        /// <summary>
        /// Make sure the <see cref="DuplicateCommand"/> is working correctly
        /// </summary>
        [TestMethod]
        public void TestDuplicateCommand()
        {
            // Set up
            DrawableObjectList list = DrawableObjectList.Create();

            List<DrawableObject> objList = new List<DrawableObject>();

            for (int i = 0; i < 10; ++i)
            {
                objList.Add(DrawableObject.Create(ImageType.Mountain, new Point(0, 0), new Size(1, 1)));
            }

            foreach (var obj in objList)
            {
                list.Add(obj);
            }

            DuplicateCommand command = DuplicateCommand.Create(list, list[0]);
            Assert.IsTrue(list.Count == 10);

            // This should do nothing
            command.Undo();
            Assert.IsTrue(list.Count == 10);

            // This should add a DrawableObject to the list that has the same
            // image type, location, and size as list[0]
            command.Execute();
            Assert.IsTrue(list.Count == 11);
            Assert.IsTrue(list[0].Size.Width == list[10].Size.Width);
            Assert.IsTrue(list[0].Size.Height == list[10].Size.Height);
            Assert.IsTrue(list[0].Location.X == list[10].Location.X);
            Assert.IsTrue(list[0].Location.Y == list[10].Location.Y);

            ImageType test = list[0].ImageType;
            ImageType type0 = list[0].ImageType;
            ImageType type10 = list[8].ImageType;

            Assert.AreEqual(list[0].ImageType, list[10].ImageType);

            command.Undo();
            Assert.IsTrue(list.Count == 10);

            command.Execute();
            command.Execute();
            Assert.IsTrue(list.Count == 12);
            command.Undo();
            command.Undo();
            command.Undo();
            Assert.IsTrue(list.Count == 10);
        }
    }
}
