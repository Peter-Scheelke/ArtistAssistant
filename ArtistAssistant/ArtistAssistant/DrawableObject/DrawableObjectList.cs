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
        /// Initializes a new instance of the <see cref="DrawableObjectList"/> class
        /// </summary>
        public DrawableObjectList()
        {
            this.observers = new List<IObserver<DrawableObjectList>>();
            this.unsubscribers = new List<IDisposable>();
        }

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
        /// The <see cref="DrawableObject"/> most recently added to the <see cref="DrawableObjectList"/>
        /// that contains the given <see cref="Point"/>
        /// </returns>
        public int GetIdFromLocation(Point location)
        {
            for (int i = this.Count - 1; i >= 0; --i)
            {
                Size size = this[i].Size;
                Point topLeft = this[i].Location;
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
                    return this[i].Id;
                }
            }

            return -1;
        }

        /// <summary>
        /// Adds a <see cref="DrawableObject"/> to the end of the <see cref="DrawableObjectList"/>
        /// </summary>
        /// <param name="item">The <see cref="DrawableObject"/> being added to the <see cref="DrawableObjectList"/></param>
        public new void Add(DrawableObject item)
        {
            this.unsubscribers.Add(item.Subscribe(this));
            base.Add(item);
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
            IDisposable unsubsriber = this.unsubscribers[index];
            this.unsubscribers.RemoveAt(index);
            unsubsriber.Dispose();
            base.Remove(item);
            this.Notify();
        }

        /// <summary>
        /// Removes the <see cref="DrawableObject"/> at the specified index from the <see cref="DrawableObjectList"/>
        /// </summary>
        /// <param name="index">The index of the <see cref="DrawableObject"/> that is being removed</param>
        public new void RemoveAt(int index)
        {
            IDisposable unsubsriber = this.unsubscribers[index];
            this.unsubscribers.RemoveAt(index);
            unsubsriber.Dispose();
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
            }

            foreach (IDisposable unsubscriber in unsubscribers)
            {
                this.unsubscribers.Remove(unsubscriber);
                unsubscriber.Dispose();
            }

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

            this.unsubscribers.Clear();
            base.Clear();
            this.Notify();
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
            foreach (IObserver<DrawableObjectList> observer in this.observers)
            {
                observer.OnNext(this);
            }
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
