using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtistAssistant.DrawableObject;
using ArtistAssistant.Serializer;
using System.Drawing;
using System.Collections.Generic;
using ArtistAssistant.Command;
using ArtistAssistant;
using ArtistAssistant.Storage;
using System.IO;
using System.Drawing.Imaging;

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

            bool succeeded = true;
            try
            {
                Exception error = new Exception();
                list.OnError(error);
                succeeded = false;
            }
            catch (Exception) { }

            try
            {
                list.OnCompleted();
                succeeded = false;
            }
            catch (Exception) { }

            Assert.IsTrue(succeeded);

            int id = list.RenderOrder[3].Id;
            list.BringToIndex(-1, 3);

            Assert.IsTrue(list.RenderOrder[3].Id == id);

            id = list.RenderOrder[0].Id;
            list.BringToIndex(0, 9001);
            Assert.IsTrue(list.RenderOrder[0].Id == id);

            list.BringToIndex(9001, 0);
            Assert.IsTrue(list.RenderOrder[0].Id == id);
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
            Assert.IsTrue(list.GetObjectFromLocation(testPoint) == null);

            list.Add(obj);
            Assert.IsTrue(list.GetIdFromLocation(testPoint) == -1);
            Assert.IsTrue(list.GetObjectFromLocation(testPoint) == null);

            obj = DrawableObject.Create(ImageType.Cloud, new Point(30, 30), new Size(19, 19));
            list.Add(obj);
            Assert.IsTrue(list.GetIdFromLocation(testPoint) == -1);
            Assert.IsTrue(list.GetObjectFromLocation(testPoint) == null);

            obj.Size = new Size(20, 20);
            Assert.IsTrue(list.GetIdFromLocation(testPoint) == obj.Id);
            Assert.IsTrue(list.GetObjectFromLocation(testPoint).Id == obj.Id);

            list[0].Location = obj.Location;
            list[0].Size = obj.Size;
            Assert.IsTrue(list.GetIdFromLocation(testPoint) == obj.Id && obj.Id != list[0].Id);
            Assert.IsTrue(list.GetObjectFromLocation(testPoint).Id == obj.Id);

            obj.Size = new Size(19, 19);
            Assert.IsTrue(list.GetIdFromLocation(testPoint) == list[0].Id && obj.Id != list[0].Id);
            Assert.IsTrue(list.GetObjectFromLocation(testPoint).Id == list[0].Id);

            // Make sure it works when the render order gets changed
            list[1].Size = new Size(20, 20);
            Assert.IsTrue(list.GetIdFromLocation(testPoint) == list[1].Id);
            Assert.IsTrue(list.GetObjectFromLocation(testPoint).Id == list[1].Id);

            list.BringToIndex(0, 1);
            Assert.IsFalse(list.GetIdFromLocation(testPoint) == list[1].Id);
            Assert.IsFalse(list.GetObjectFromLocation(testPoint).Id == list[1].Id);

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

            // Test the other constructor
            command = AddCommand.Create(list, DrawableObject.Create(ImageType.Cloud, new Point(5, 5), new Size(5, 5)));
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

            // Test the other constructor
            removeCommand = RemoveCommand.Create(list, third);
            removeCommand.Execute();
            Assert.IsFalse(third.Id == list.RenderOrder[3].Id);
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

            // Test the second constructor
            list[3].Deselect();
            command = SelectCommand.Create(list, obj);

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

            // Make sure you can execute multiple times
            command.Execute();
            command.Execute();
            Assert.IsTrue(list.Count == 12);

            // Make sure undoing doesn't undo too much
            command.Undo();
            command.Undo();
            command.Undo();
            Assert.IsTrue(list.Count == 10);
        }

        /// <summary>
        /// Make sure that the <see cref="CommandFactory"/> correctly
        /// creates <see cref="ICommand"/>s
        /// </summary>
        [TestMethod]
        public void TestCommandFactory()
        {
            bool failed = true; // Failed is used to make sure that exceptions were thrown
            CommandFactory factory = CommandFactory.Create();
            CommandFactory.CommandArguments parameters;
            DrawableObjectList list = DrawableObjectList.Create();
            ICommand command = null;
            DrawableObject drawableObject = null;

            List<DrawableObject> objList = new List<DrawableObject>();
            for (int i = 0; i < 10; ++i)
            {
                objList.Add(DrawableObject.Create(ImageType.Mountain, new Point(0, 0), new Size(1, 1)));
            }

            foreach (var obj in objList)
            {
                list.Add(obj);
            }

            failed = CheckAddCommandCreation(failed, factory, out parameters, list, out command, out drawableObject);
            failed = CheckBringToIndexCommandCreation(failed, factory, out parameters, list, out command);
            failed = CheckDeselectCommandCreation(failed, factory, out parameters, list, out command);
            failed = CheckDuplicateCommandCreation(failed, factory, out parameters, list, out command);
            failed = CheckMoveCommandCreation(failed, factory, out parameters, list, out command);
            failed = CheckRemoveCommandCreation(failed, factory, out parameters, list, out command);
            failed = CheckScaleCommandCreation(failed, factory, out parameters, list, out command);
            failed = CheckSelectCommandCreation(failed, factory, out parameters, list, out command);
        }

        // The methods below are used to test the CommandFactory

        private static bool CheckSelectCommandCreation(bool failed, CommandFactory factory, out CommandFactory.CommandArguments parameters, DrawableObjectList list, out ICommand command)
        {
            // Test SelectCommand creation
            parameters = CommandFactory.GetCommandArgumentsObject();
            parameters.CommandType = CommandType.Select;
            parameters.DrawableObjectList = list;

            // Parameters isn't correct for a SelectCommand and should fail
            try
            {
                command = factory.CreateCommand(parameters);
                failed = false;
            }
            catch (Exception) { }
            Assert.IsTrue(failed);

            parameters.AffectedDrawableObject = list.RenderOrder[list.Count - 1];
            command = factory.CreateCommand(parameters);
            Assert.IsFalse(list.RenderOrder[list.Count - 1].Selected);
            command.Execute();
            Assert.IsTrue(list.RenderOrder[list.Count - 1].Selected);
            command.Undo();
            Assert.IsFalse(list.RenderOrder[list.Count - 1].Selected);
            return failed;
        }

        private static bool CheckScaleCommandCreation(bool failed, CommandFactory factory, out CommandFactory.CommandArguments parameters, DrawableObjectList list, out ICommand command)
        {
            // Test ScaleCommand creation
            parameters = CommandFactory.GetCommandArgumentsObject();
            parameters.CommandType = CommandType.Scale;
            parameters.DrawableObjectList = list;
            parameters.AffectedDrawableObject = list[7];

            // Parameters isn't correct for a ScaleCommand and should fail
            try
            {
                command = factory.CreateCommand(parameters);
                failed = false;
            }
            catch (Exception) { }
            Assert.IsTrue(failed);


            parameters.Size = new Size(50, 50);
            command = factory.CreateCommand(parameters);
            Assert.IsTrue(list[7].Size.Width == 1);
            Assert.IsTrue(list[7].Size.Height == 1);
            command.Execute();
            Assert.IsTrue(list[7].Size.Width == 50);
            Assert.IsTrue(list[7].Size.Height == 50);
            command.Undo();
            Assert.IsTrue(list[7].Size.Width == 1);
            Assert.IsTrue(list[7].Size.Height == 1);
            return failed;
        }

        private static bool CheckRemoveCommandCreation(bool failed, CommandFactory factory, out CommandFactory.CommandArguments parameters, DrawableObjectList list, out ICommand command)
        {
            // Test RemoveCommand creation
            parameters = CommandFactory.GetCommandArgumentsObject();
            parameters.CommandType = CommandType.Remove;
            parameters.DrawableObjectList = list;

            // Parameters isn't correct for a RemoveCommand and should fail
            try
            {
                command = factory.CreateCommand(parameters);
                failed = false;
            }
            catch (Exception) { }
            Assert.IsTrue(failed);

            parameters.AffectedDrawableObject = list.RenderOrder[list.Count - 1];
            command = factory.CreateCommand(parameters);
            int id = list.RenderOrder[list.Count - 1].Id;
            command.Execute();
            Assert.IsTrue(list.RenderOrder[list.Count - 1].Id != id);
            command.Undo();
            Assert.IsTrue(id == list.RenderOrder[list.Count - 1].Id);
            return failed;
        }

        private static bool CheckMoveCommandCreation(bool failed, CommandFactory factory, out CommandFactory.CommandArguments parameters, DrawableObjectList list, out ICommand command)
        {
            // Test MoveCommand creation
            parameters = CommandFactory.GetCommandArgumentsObject();
            parameters.CommandType = CommandType.Move;
            parameters.DrawableObjectList = list;

            // Parameters isn't correct for a MoveCommand and should fail
            try
            {
                command = factory.CreateCommand(parameters);
                failed = false;
            }
            catch (Exception) { }
            Assert.IsTrue(failed);
            parameters.Location = new Point(80, 80);
            parameters.AffectedDrawableObject = list[8];
            command = factory.CreateCommand(parameters);
            Assert.IsTrue(list[8].Location.X == 0 && list[8].Location.Y == 0);
            command.Execute();
            Assert.IsTrue(list[8].Location.X == 80 && list[8].Location.Y == 80);
            command.Undo();
            Assert.IsTrue(list[8].Location.X == 0 && list[8].Location.Y == 0);
            return failed;
        }

        private static bool CheckDuplicateCommandCreation(bool failed, CommandFactory factory, out CommandFactory.CommandArguments parameters, DrawableObjectList list, out ICommand command)
        {
            // Test DuplicateCommand creation
            parameters = CommandFactory.GetCommandArgumentsObject();
            parameters.CommandType = CommandType.Duplicate;
            parameters.DrawableObjectList = list;

            // Parameters isn't correct for a DuplicateCommand and should fail
            try
            {
                command = factory.CreateCommand(parameters);
                failed = false;
            }
            catch (Exception) { }
            Assert.IsTrue(failed);
            parameters.AffectedDrawableObject = list[6];
            command = factory.CreateCommand(parameters);
            int count = list.Count;
            command.Execute();
            Assert.IsTrue(list.Count == count + 1);
            Assert.IsTrue(list[6].Size.Width == list[count].Size.Width);
            Assert.IsTrue(list[6].Size.Height == list[count].Size.Height);
            Assert.IsTrue(list[6].Location.X == list[count].Location.X);
            Assert.IsTrue(list[6].ImageType == list[count].ImageType);
            command.Undo();
            Assert.IsTrue(list.Count == count);
            return failed;
        }

        private static bool CheckDeselectCommandCreation(bool failed, CommandFactory factory, out CommandFactory.CommandArguments parameters, DrawableObjectList list, out ICommand command)
        {
            // Test DeselectCommand creation
            parameters = CommandFactory.GetCommandArgumentsObject();

            // Parameters isn't correct for a DeselectCommand and should fail
            try
            {
                command = factory.CreateCommand(parameters);
                failed = false;
            }
            catch (Exception) { }
            Assert.IsTrue(failed);

            parameters.CommandType = CommandType.Deselect;
            parameters.DrawableObjectList = list;
            list[4].Select();
            for (int i = 0; i < list.Count; ++i)
            {
                if (i == 4)
                {
                    Assert.IsTrue(list[i].Selected);
                }
                else
                {
                    Assert.IsFalse(list[i].Selected);
                }
            }

            command = factory.CreateCommand(parameters);
            command.Execute();
            foreach (DrawableObject item in list)
            {
                Assert.IsFalse(item.Selected);
            }

            command.Undo();
            for (int i = 0; i < list.Count; ++i)
            {
                if (i == 4)
                {
                    Assert.IsTrue(list[i].Selected);
                }
                else
                {
                    Assert.IsFalse(list[i].Selected);
                }
            }

            return failed;
        }

        private static bool CheckBringToIndexCommandCreation(bool failed, CommandFactory factory, out CommandFactory.CommandArguments parameters, DrawableObjectList list, out ICommand command)
        {
            // Test BringToIndexCommand creation
            parameters = CommandFactory.GetCommandArgumentsObject();
            parameters.CommandType = CommandType.BringToIndex;
            parameters.DrawableObjectList = list;

            // Parameters isn't correct for a BringToIndexCommand and should fail
            try
            {
                command = factory.CreateCommand(parameters);
                failed = false;
            }
            catch (Exception) { }
            Assert.IsTrue(failed);

            parameters.StartIndex = 0;
            parameters.TargetIndex = 5;
            int id0 = list.RenderOrder[0].Id;
            int id5 = list.RenderOrder[5].Id;
            command = factory.CreateCommand(parameters);
            command.Execute();
            Assert.IsTrue(list.RenderOrder[4].Id == id5);
            Assert.IsTrue(list.RenderOrder[5].Id == id0);
            command.Undo();
            Assert.IsTrue(list.RenderOrder[5].Id == id5);
            Assert.IsTrue(list.RenderOrder[0].Id == id0);
            return failed;
        }

        private static bool CheckAddCommandCreation(bool failed, CommandFactory factory, out CommandFactory.CommandArguments parameters, DrawableObjectList list, out ICommand command, out DrawableObject drawableObject)
        {
            // Test AddCommand creation
            parameters = CommandFactory.GetCommandArgumentsObject();
            parameters.CommandType = CommandType.Add;
            drawableObject = DrawableObject.Create(ImageType.Mountain, new Point(10, 10), new Size(5, 5));
            parameters.DrawableObjectList = list;

            // Parameters isn't correct for an AddCommand and should fail
            try
            {
                command = factory.CreateCommand(parameters);
                failed = false;
            }
            catch (Exception) { }
            Assert.IsTrue(failed);

            parameters.AffectedDrawableObject = drawableObject;
            command = factory.CreateCommand(parameters);
            Assert.IsTrue(list.Count == 10);
            command.Execute();
            Assert.IsTrue(list.Count == 11);
            command.Undo();
            Assert.IsTrue(list.Count == 10);
            return failed;
        }

        /// <summary>
        /// Make sure that <see cref="Drawing"/> objects don't throw exceptions
        /// </summary>
        [TestMethod]
        public void TestDrawing()
        {
            bool succeeded = true;
            try
            {
                DrawableObjectList list = DrawableObjectList.Create();
                Bitmap bitmap = new Bitmap(100, 100);
                Drawing drawing = Drawing.Create(bitmap, list, new Size(100, 100));
                list.Add(DrawableObject.Create(ImageType.Mountain, new Point(10, 10), new Size(5, 5)));
                Image background = drawing.RenderedDrawing;

                for (int i = 0; i < 10; ++i)
                {
                    list.Add(DrawableObject.Create(ImageType.Mountain, new Point(10, 10), new Size(5, 5)));
                }

                list[5].Select();

                drawing.Size = new Size(50, 50);

                try
                {
                    Exception error = new Exception();
                    drawing.OnError(error);
                    succeeded = false;
                }
                catch (Exception) { }

                try
                {
                    drawing.OnCompleted();
                    succeeded = false;
                }
                catch (Exception) { }
            }
            catch (Exception)
            {
                succeeded = false;
            }

            Assert.IsTrue(succeeded);
        }

        [TestMethod]
        public void TestBackendWrapper()
        {
            BackendWrapper backend = BackendWrapper.Create(new Bitmap(500, 500), new Size(500, 500));

            Assert.IsTrue(backend.GetLocationOfSelectedItem() == null);
            Assert.IsTrue(backend.GetSizeOfSelectedItem() == null);

            try
            {
                backend.Add(ImageType.Mountain, new Point(50, 50), new Size(75, 75));
                backend.Select(new Point(60, 60));
                Assert.IsTrue(backend.GetSizeOfSelectedItem()?.Width == 75);
                Assert.IsTrue(backend.GetSizeOfSelectedItem()?.Width == 75);

                Assert.IsTrue(backend.GetLocationOfSelectedItem()?.X == 50);
                Assert.IsTrue(backend.GetLocationOfSelectedItem()?.Y == 50);

                backend.Move(new Point(30, 30));
                Assert.IsTrue(backend.GetLocationOfSelectedItem()?.X == 30);
                Assert.IsTrue(backend.GetLocationOfSelectedItem()?.Y == 30);

                backend.Duplicate();
                Assert.IsTrue(backend.GetLocationOfSelectedItem()?.X == 45);
                Assert.IsTrue(backend.GetLocationOfSelectedItem()?.Y == 45);

                backend.Scale(new Size(80, 80));
                Assert.IsTrue(backend.GetSizeOfSelectedItem()?.Width == 80);
                Assert.IsTrue(backend.GetSizeOfSelectedItem()?.Width == 80);

                backend.Undo();
                Assert.IsTrue(backend.GetSizeOfSelectedItem()?.Width == 75);
                Assert.IsTrue(backend.GetSizeOfSelectedItem()?.Width == 75);

                backend.BringToFront();
                backend.SendToBack();

                backend.Remove();
                Assert.IsTrue(backend.GetLocationOfSelectedItem() == null);

                // These shouldn't do anything (no exceptions should be thrown though)
                backend.Move(new Point(30, 30));
                backend.BringToFront();
                backend.SendToBack();
                backend.Scale(new Size(80, 80));

                backend.Undo();
                Assert.IsFalse(backend.GetLocationOfSelectedItem() == null);

                backend.Deselect();
                Assert.IsTrue(backend.GetLocationOfSelectedItem() == null);
                backend.Undo();
                Assert.IsFalse(backend.GetLocationOfSelectedItem() == null);

                backend.ClearAndStartNewDrawing(new Bitmap(500, 500), new Size(500, 500));
                Assert.IsTrue(backend.GetLocationOfSelectedItem() == null);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        /// <summary>
        /// Make sure the <see cref="DrawableObject"/> serializer
        /// can correctly serialize/deserialize things
        /// </summary>
        [TestMethod]
        public void TestSerializer()
        {
            try
            {
                DrawableObjectList list = DrawableObjectList.Create();
                list.Add(DrawableObject.Create(ImageType.Pine, new Point(10, 10), new Size(5, 5)));
                list.Add(DrawableObject.Create(ImageType.Pond, new Point(10, 11), new Size(5, 6)));
                list.Add(DrawableObject.Create(ImageType.Mountain, new Point(10, 12), new Size(5, 7)));
                string str = DrawableObjectSerializer.Serialize(list);

                DrawableObjectList list2 = DrawableObjectSerializer.Deserialize(str);

                Assert.IsTrue(list.Count == list2.Count);
                for (int i = 0; i < list2.Count; ++i)
                {
                    Assert.IsTrue(list.RenderOrder[i].Size.Width == list2.RenderOrder[i].Size.Width);
                    Assert.IsTrue(list.RenderOrder[i].Size.Height == list2.RenderOrder[i].Size.Height);
                    Assert.IsTrue(list.RenderOrder[i].Location.X == list2.RenderOrder[i].Location.X);
                    Assert.IsTrue(list.RenderOrder[i].Location.Y == list2.RenderOrder[i].Location.Y);
                    Assert.IsTrue(list.RenderOrder[i].ImageType == list2.RenderOrder[i].ImageType);
                    Assert.IsTrue(list.RenderOrder[i].Selected == list2.RenderOrder[i].Selected);
                }
            }
            catch (Exception)
            {
                Assert.Fail();
            }

            bool exceptionThrown = false;
            try
            {
                DrawableObjectList list = DrawableObjectSerializer.Deserialize("This won't work");
            }
            catch (Exception)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }

        /// <summary>
        /// Make sure that cloud storage is working
        /// </summary>
        [TestMethod]
        public void TestCloudStorage()
        {
            DrawableObjectList list = DrawableObjectList.Create();
            list.Add(DrawableObject.Create(ImageType.Pine, new Point(10, 10), new Size(5, 5)));
            list.Add(DrawableObject.Create(ImageType.Pond, new Point(10, 11), new Size(5, 6)));
            list.Add(DrawableObject.Create(ImageType.Mountain, new Point(10, 12), new Size(5, 7)));
            string listToJson = DrawableObjectSerializer.Serialize(list);
            Image bitmap = Image.FromFile("undo.png");

            int cloudFileCount = CloudManager.ListFiles().Count;
            string key = "testfile";
            bitmap.Save($"{key}.png");
            File.WriteAllText($"{key}.json", listToJson);
            CloudManager.Upload($"{key}.json", $"{key}.png", key);
            File.Delete($"{key}.json");
            File.Delete($"{key}.png");

            Assert.IsTrue(cloudFileCount + 1 == CloudManager.ListFiles().Count);

            CloudManager.Download($"{key}2.json", $"{key}2.png", key);
            Assert.IsTrue(File.Exists($"{key}2.json") && File.Exists($"{key}2.png"));

            string jsonToList = File.ReadAllText($"{key}2.json");
            DrawableObjectList newList = DrawableObjectSerializer.Deserialize(jsonToList);
            File.Delete($"{key}2.json");
            File.Delete($"{key}2.png");

            for (int i = 0; i < newList.Count; ++i)
            {
                Assert.IsTrue(newList.RenderOrder[i].ImageType == list.RenderOrder[i].ImageType);
                Assert.IsTrue(newList.RenderOrder[i].Selected == list.RenderOrder[i].Selected);
                Assert.IsTrue(newList.RenderOrder[i].Size.Width == list.RenderOrder[i].Size.Width);
                Assert.IsTrue(newList.RenderOrder[i].Size.Height == list.RenderOrder[i].Size.Height);
                Assert.IsTrue(newList.RenderOrder[i].Location.X == list.RenderOrder[i].Location.X);
                Assert.IsTrue(newList.RenderOrder[i].Location.Y == list.RenderOrder[i].Location.Y);
            }

            CloudManager.Delete(new List<string>() { key });
            Assert.IsTrue(cloudFileCount == CloudManager.ListFiles().Count);
        }

        [TestMethod]
        public void ZTempTest()
        {
            Bitmap bitmap = new Bitmap(50, 50);
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                using (SolidBrush brush = new SolidBrush(Color.Aqua))
                {
                    graphics.FillRectangle(brush, 0, 0, bitmap.Size.Width, bitmap.Size.Height);
                }
            }

            bitmap.Save("test.png", ImageFormat.Png);
        }
    }
}