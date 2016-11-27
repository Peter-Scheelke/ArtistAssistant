//-----------------------------------------------------------------------
// <copyright file="DrawableObjectList.cs" company="Peter Scheelke">
//      Copyright (c) Peter Scheelke. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ArtistAssistant.DrawableObject
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;

    /// <summary>
    /// A list of <see cref="DrawableObject"/>s. It subscribes to those objects, and notifies
    /// any observers when those objects change
    /// </summary>
    public class DrawableObjectList : List<DrawableObject>, IObserver<DrawableObject>, IObservable<DrawableObjectList>
    {
        /// <summary>
        /// A list of the <see cref="IObserver{T}"/>s that are observing the <see cref="DrawableObjectList"/>
        /// </summary>
        private List<IObserver<DrawableObjectList>> observers;

        /// <summary>
        /// A List of the <see cref="IDisposable"/>s that are used to unsubscribe from observed <see cref="DrawableObject"/>s
        /// </summary>
        private List<IDisposable> unsubscribers;

        /// <summary>
        /// Keeps track of whether the <see cref="DrawableObjectList"/>
        /// is changing which element is currently selected
        /// </summary>
        private bool isChangingState;

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawableObjectList"/> class
        /// </summary>
        public DrawableObjectList()
        {
            this.observers = new List<IObserver<DrawableObjectList>>();
            this.unsubscribers = new List<IDisposable>();
            this.RenderOrder = new List<DrawableObject>();
            this.isChangingState = false;
            this.SelectedObject = null;
            this.ClippingAreaBuffer = new List<ClippingArea>();
        }

        /// <summary>
        /// Gets a list of the <see cref="ClippingArea"/>s that have changed since the last time observers were notified
        /// </summary>
        public List<ClippingArea> ClippingAreaBuffer { get; private set; }

        /// <summary>
        /// Gets the order in which the <see cref="DrawableObject"/>s in the <see cref="DrawableObjectList"/>
        /// should be rendered
        /// </summary>
        public List<DrawableObject> RenderOrder { get; private set; }

        /// <summary>
        /// Gets the <see cref="DrawableObject"/> that is currently selected
        /// </summary>
        public DrawableObject SelectedObject { get; private set; }

        /// <summary>
        /// Creates a new <see cref="DrawableObjectList"/> object
        /// </summary>
        /// <returns>A new <see cref="DrawableObjectList"/> object</returns>
        public static DrawableObjectList Create()
        {
            return new DrawableObjectList();
        }

        /// <summary>
        /// Gets the Id of the <see cref="DrawableObject"/> that contains the given
        /// <see cref="Point"/>. The Id will correspond to the most recently added
        /// item in the <see cref="DrawableObjectList"/>.
        /// </summary>
        /// <param name="location">The <see cref="Point"/> being checked</param>
        /// <returns>
        /// The Id of the <see cref="DrawableObject"/> that contains the given <see cref="Point"/>
        /// and is nearest the top of the render order of the list of <see cref="DrawableObject"/>s 
        /// </returns>
        public int GetIdFromLocation(Point location)
        {
            for (int i = this.Count - 1; i >= 0; --i)
            {
                Size size = this.RenderOrder[i].Size;
                Point topLeft = this.RenderOrder[i].Location;
                bool inXDimension = false;
                bool inYDimension = false;
                if (location.X >= topLeft.X && location.X <= topLeft.X + size.Width)
                {
                    inXDimension = true;
                }

                if (location.Y >= topLeft.Y && location.Y <= topLeft.Y + size.Height)
                {
                    inYDimension = true;
                }

                if (inXDimension && inYDimension)
                {
                    return this.RenderOrder[i].Id;
                }
            }

            return -1;
        }

        /// <summary>
        /// Gets the <see cref="DrawableObject"/> that contains the give
        /// <see cref="Point"/>. The Id will correspond to the most recently
        /// added item in the <see cref="DrawableObjectList"/>.
        /// </summary>
        /// <param name="location">The <see cref="Point"/> being checked</param>
        /// <returns>
        /// The <see cref="DrawableObject"/> that contains the given <see cref="Point"/>
        /// and is nearest the top of the render order of the list of <see cref="DrawableObject"/>s 
        /// </returns>
        public DrawableObject GetObjectFromLocation(Point location)
        {
            for (int i = this.Count - 1; i >= 0; --i)
            {
                Size size = this.RenderOrder[i].Size;
                Point topLeft = this.RenderOrder[i].Location;
                bool inXDimension = false;
                bool inYDimension = false;
                if (location.X >= topLeft.X && location.X <= topLeft.X + size.Width)
                {
                    inXDimension = true;
                }

                if (location.Y >= topLeft.Y && location.Y <= topLeft.Y + size.Height)
                {
                    inYDimension = true;
                }

                if (inXDimension && inYDimension)
                {
                    return this.RenderOrder[i];
                }
            }

            return null;
        }

        /// <summary>
        /// Adds a <see cref="DrawableObject"/> to the end of the <see cref="DrawableObjectList"/>
        /// </summary>
        /// <param name="item">The <see cref="DrawableObject"/> being added to the <see cref="DrawableObjectList"/></param>
        public new void Add(DrawableObject item)
        {
            this.unsubscribers.Add(item.Subscribe(this));
            base.Add(item);
            this.RenderOrder.Add(item);
            this.ClippingAreaBuffer.Add(new ClippingArea(item.Location, item.Size));
            this.Notify();
        }

        /// <summary>
        /// Adds the <see cref="DrawableObject"/>s of the specified collection to the end of the <see cref="DrawableObjectList"/>
        /// </summary>
        /// <param name="collection">The collection being added to the list</param>
        public new void AddRange(IEnumerable<DrawableObject> collection)
        {
            foreach (DrawableObject drawableObject in collection)
            {
                this.unsubscribers.Add(drawableObject.Subscribe(this));
            }

            this.RenderOrder.AddRange(collection);
            base.AddRange(collection);
            this.Notify();
        }

        /// <summary>
        /// Inserts a <see cref="DrawableObject"/> into the <see cref="DrawableObjectList"/>
        /// at the specified list
        /// </summary>
        /// <param name="index">The index where the <see cref="DrawableObject"/> should be inserted</param>
        /// <param name="item">The <see cref="DrawableObject"/> being inserted</param>
        public new void Insert(int index, DrawableObject item)
        {
            this.unsubscribers.Insert(index, item.Subscribe(this));
            this.RenderOrder.Add(item);
            base.Insert(index, item);
            this.Notify();
        }

        /// <summary>
        /// Inserts the <see cref="DrawableObject"/>s of a collection into the <see cref="DrawableObjectList"/>
        /// at the specified index
        /// </summary>
        /// <param name="index">The index where the collection should be inserted</param>
        /// <param name="collection">The collection being inserted</param>
        public new void InsertRange(int index, IEnumerable<DrawableObject> collection)
        {
            List<IDisposable> unsubscribers = new List<IDisposable>();
            foreach (DrawableObject item in collection)
            {
                unsubscribers.Add(item.Subscribe(this));
            }

            this.unsubscribers.InsertRange(index, unsubscribers);
            this.RenderOrder.AddRange(collection);
            base.InsertRange(index, collection);
            this.Notify();
        }

        /// <summary>
        /// Removes the first occurrence of a specific <see cref="DrawableObject"/>
        /// from the <see cref="DrawableObjectList"/>
        /// </summary>
        /// <param name="item">The <see cref="DrawableObject"/> being removed</param>
        public new void Remove(DrawableObject item)
        {
            int index = this.IndexOf(item);
            this.ClippingAreaBuffer.Add(new ClippingArea(item.Location, item.Size));
            IDisposable unsubsriber = this.unsubscribers[index];
            this.unsubscribers.RemoveAt(index);
            unsubsriber.Dispose();
            this.RenderOrder.Remove(item);
            base.Remove(item);
            this.Notify();
        }

        /// <summary>
        /// Removes the <see cref="DrawableObject"/> at the specified index from the <see cref="DrawableObjectList"/>
        /// </summary>
        /// <param name="index">The index of the <see cref="DrawableObject"/> that is being removed</param>
        public new void RemoveAt(int index)
        {
            this.ClippingAreaBuffer.Add(new ClippingArea(this[index].Location, this[index].Size));
            IDisposable unsubsriber = this.unsubscribers[index];
            this.unsubscribers.RemoveAt(index);
            unsubsriber.Dispose();
            this.RenderOrder.Remove(this[index]);
            base.RemoveAt(index);
            this.Notify();
        }

        /// <summary>
        /// Removes all the <see cref="DrawableObject"/>s that match the conditions
        /// specified by the predicate
        /// </summary>
        /// <param name="match">The predicate being used to find the <see cref="DrawableObject"/></param>
        public new void RemoveAll(Predicate<DrawableObject> match)
        {
            List<DrawableObject> matches = this.FindAll(match);
            List<IDisposable> unsubscribers = new List<IDisposable>();
            foreach (DrawableObject objectMatch in matches)
            {
                int index = this.IndexOf(objectMatch);
                unsubscribers.Add(this.unsubscribers[index]);
                this.ClippingAreaBuffer.Add(new ClippingArea(objectMatch.Location, objectMatch.Size));
            }

            foreach (IDisposable unsubscriber in unsubscribers)
            {
                this.unsubscribers.Remove(unsubscriber);
                unsubscriber.Dispose();
            }

            this.RenderOrder.RemoveAll(match);
            base.RemoveAll(match);
            this.Notify();
        }

        /// <summary>
        /// Removes a range of <see cref="DrawableObject"/>s from the <see cref="DrawableObjectList"/>
        /// </summary>
        /// <param name="index">The index of the first <see cref="DrawableObject"/>s to be removed</param>
        /// <param name="count">The number of <see cref="DrawableObject"/>s to remove</param>
        public new void RemoveRange(int index, int count)
        {
            for (int i = index; i < index + count; ++i)
            {
                this.unsubscribers[i].Dispose();
                this.RenderOrder.Remove(this[i]);
                this.ClippingAreaBuffer.Add(new ClippingArea(this[i].Location, this[i].Size));
            }

            this.unsubscribers.RemoveRange(index, count);
            base.RemoveRange(index, count);
            this.Notify();
        }

        /// <summary>
        /// Removes all <see cref="DrawableObject"/>s from the <see cref="DrawableObjectList"/>
        /// </summary>
        public new void Clear()
        {
            foreach (IDisposable unsubsciber in this.unsubscribers)
            {
                unsubsciber.Dispose();
            }

            int maxWidth = 0;
            int maxHeight = 0;
            foreach (DrawableObject drawableObject in this)
            {
                if (drawableObject.Size.Width > maxWidth)
                {
                    maxWidth = drawableObject.Size.Width;
                }

                if (drawableObject.Size.Height > maxHeight)
                {
                    maxHeight = drawableObject.Size.Height;
                }
            }

            this.ClippingAreaBuffer.Add(new ClippingArea(new Point(0, 0), new Size(maxWidth, maxHeight)));
            this.unsubscribers.Clear();
            this.RenderOrder.Clear(); 
            base.Clear();
            this.Notify();
        }

        /// <summary>
        /// Moves the <see cref="DrawableObject"/> at the given index in <see cref="RenderOrder"/> to the target index
        /// </summary>
        /// <param name="currentIndex">The current index of the <see cref="DrawableObject"/></param>
        /// <param name="targetIndex">The target index of the <see cref="DrawableObject"/></param>
        public void BringToIndex(int currentIndex, int targetIndex)
        {
            if (currentIndex >= this.Count || currentIndex < 0)
            {
                return;
            }

            if (targetIndex >= this.Count || targetIndex < 0)
            {
                return;
            }

            DrawableObject temp = this.RenderOrder[currentIndex];

            while (true)
            {
                if (targetIndex < currentIndex)
                {
                    // Shift things to the right until the target index is free
                    --currentIndex;
                    this.RenderOrder[currentIndex + 1] = this.RenderOrder[currentIndex];
                    this.ClippingAreaBuffer.Add(new ClippingArea(this.RenderOrder[currentIndex + 1].Location, this.RenderOrder[currentIndex + 1].Size));
                }
                else if (targetIndex > currentIndex)
                {
                    // Shift stuff left until the target index is free
                    ++currentIndex;
                    this.RenderOrder[currentIndex - 1] = this.RenderOrder[currentIndex];
                    this.ClippingAreaBuffer.Add(new ClippingArea(this.RenderOrder[currentIndex - 1].Location, this.RenderOrder[currentIndex - 1].Size));
                }
                else
                {
                    this.RenderOrder[currentIndex] = temp;
                    this.ClippingAreaBuffer.Add(new ClippingArea(this.RenderOrder[currentIndex].Location, this.RenderOrder[currentIndex].Size));
                    this.Notify();
                    break;
                }
            }
        }

        /// <summary>
        /// Not used
        /// </summary>
        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not used
        /// </summary>
        /// <param name="error">Parameter not used</param>
        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Notifies the <see cref="DrawableObjectList"/>
        /// that an observed <see cref="DrawableObject"/>
        /// has been updated
        /// </summary>
        /// <param name="value">The <see cref="DrawableObject"/> that has been updated</param>
        public void OnNext(DrawableObject value)
        {
            if (this.isChangingState)
            {
                return;
            }

            foreach (ClippingArea area in value.ClippingAreaBuffer)
            {
                this.ClippingAreaBuffer.Add(area);
            }

            // This makes sure that only one element can be selected
            // at a time (isChangingState prevents OnNext from calling
            // itself in an infinite loop).
            if (value.Selected)
            {
                this.isChangingState = true;
                for (int i = 0; i < this.Count; ++i)
                {
                    if (this[i].Id != value.Id)
                    {
                        if (this[i].Selected)
                        {
                            this.ClippingAreaBuffer.Add(new ClippingArea(this[i].Location, this[i].Size));
                            this[i].Deselect();
                        }
                    }
                }

                this.isChangingState = false;
                this.SelectedObject = value;
            }
            else
            {
                this.SelectedObject = null;
            }

            foreach (IObserver<DrawableObjectList> observer in this.observers)
            {
                observer.OnNext(this);
            }
        }

        /// <summary>
        /// Subscribes the given <see cref="IObserver{T}"/> to the <see cref="DrawableObjectList"/>,
        /// and the returns an <see cref="IDisposable"/> object that can be used to unsubscribe
        /// </summary>
        /// <param name="observer">The object that wants to observe the <see cref="DrawableObjectList"/></param>
        /// <returns>An <see cref="IDisposable"/> object that can be used to unsubscribe from the <see cref="DrawableObjectList"/></returns>
        public IDisposable Subscribe(IObserver<DrawableObjectList> observer)
        {
            this.observers.Add(observer);
            Unsubscriber unsubsciber = new Unsubscriber(this.observers, observer);
            return unsubsciber;
        }

        /// <summary>
        /// Notify all observers that the <see cref="DrawableObjectList"/> has been updated
        /// </summary>
        private void Notify()
        {
            if (this.IndexOf(this.SelectedObject) == -1)
            {
                this.SelectedObject = null;
            }

            foreach (IObserver<DrawableObjectList> observer in this.observers)
            {
                observer.OnNext(this);
            }

            this.ClippingAreaBuffer.Clear();
        }

        /// <summary>
        /// Used to unsubscribe from the <see cref="DrawableObjectList"/>
        /// </summary>
        private class Unsubscriber : IDisposable
        {
            /// <summary>
            /// A list of the <see cref="IObserver{T}"/> objects that are observing
            /// the <see cref="DrawableObject"/>
            /// </summary>
            private List<IObserver<DrawableObjectList>> observers;

            /// <summary>
            /// The object that can unsubscribe using this object.
            /// </summary>
            private IObserver<DrawableObjectList> observer;

            /// <summary>
            /// Initializes a new instance of the <see cref="Unsubscriber"/> class
            /// </summary>
            /// <param name="observers">The list of <see cref="IObserver{T}"/> objects that the <see cref="DrawableObjectList"/> keeps</param>
            /// <param name="observer">The <see cref="IObserver{T}"/> that is unsubscribing itself</param>
            public Unsubscriber(List<IObserver<DrawableObjectList>> observers, IObserver<DrawableObjectList> observer)
            {
                this.observers = observers;
                this.observer = observer;
            }

            /// <summary>
            /// Unsubscribes the <see cref="IObserver{T}"/> object from the <see cref="DrawableObjectList"/>
            /// </summary>
            public void Dispose()
            {
                if (this.observer != null && this.observers.Contains(this.observer))
                {
                    this.observers.Remove(this.observer);
                }
            }
        }
    }
}
