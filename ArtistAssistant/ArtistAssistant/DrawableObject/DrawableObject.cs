//-----------------------------------------------------------------------
// <copyright file="DrawableObject.cs" company="Peter Scheelke">
//      Copyright (c) Peter Scheelke. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ArtistAssistant.DrawableObject
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;

    /// <summary>
    /// A <see cref="DrawableObject"/> can be used to draw images on
    /// <see cref="Graphics"/> objects. This class is basically just
    /// a wrapper for the internal classes
    /// </summary>
    public class DrawableObject : IObservable<DrawableObject>
    {
        /// <summary>
        /// The number of <see cref="DrawableObject"/>s that have been created
        /// Used to create a unique identification number for each <see cref="DrawableObject"/>
        /// </summary>
        private static int drawableObjectCount = 0;

        /// <summary>
        /// The <see cref="State"/> of the <see cref="DrawableObject"/>
        /// </summary>
        private State state;

        /// <summary>
        /// A list of the <see cref="IObserver{T}"/>s that are observing the <see cref="DrawableObject"/>
        /// </summary>
        private List<IObserver<DrawableObject>> observers;

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawableObject"/> class.
        /// </summary>
        /// <param name="imageType">
        /// The <see cref="ImageType"/> of the <see cref="Image"/> the <see cref="DrawableObject"/>
        /// will draw.
        /// </param>
        /// <param name="location">The location of the <see cref="DrawableObject"/></param>
        /// <param name="size">The size of the <see cref="DrawableObject"/></param>
        public DrawableObject(ImageType imageType, Point location, Size size)
        {
            this.observers = new List<IObserver<DrawableObject>>();
            StateFactory factory = new StateFactory();
            this.state = factory.Create(imageType, location, size);
            this.Id = DrawableObject.drawableObjectCount;
            ++DrawableObject.drawableObjectCount;
        }

        /// <summary>
        /// Gets a unique identifier for the <see cref="DrawableObject"/>
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Gets or sets the <see cref="ImageType"/> of the <see cref="DrawableObject"/>
        /// </summary>
        public ImageType ImageType
        {
            get
            {
                return this.state.ImageType;
            }

            set
            {
                this.state.ImageType = value;
                this.Notify();
            }
        }

        /// <summary>
        /// Gets the <see cref="Image"/> from the <see cref="DrawableObject"/>'s <see cref="State"/>
        /// </summary>
        public Image Image
        {
            get
            {
                return this.state.Image;
            }
        }

        /// <summary>
        /// Gets or sets the location of the <see cref="DrawableObject"/>
        /// </summary>
        public Point Location
        {
            get
            {
                return this.state.Location;
            }

            set
            {
                this.state.Location = value;
                this.Notify();
            }
        }

        /// <summary>
        /// Gets or sets the size of the <see cref="DrawableObject"/>
        /// </summary>
        public Size Size
        {
            get
            {
                return this.state.Size;
            }

            set
            {
                this.state.Size = value;
                this.Notify();
            }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="DrawableObject"/> is selected
        /// </summary>
        public bool Selected
        {
            get
            {
                return this.state.Selected;
            }
        }

        /// <summary>
        /// Creates a new <see cref="DrawableObject"/> object
        /// </summary>
        /// <param name="imageType">
        /// The <see cref="ImageType"/> of the <see cref="Image"/> the <see cref="DrawableObject"/>
        /// will draw.
        /// </param>
        /// <param name="location">The location of the <see cref="DrawableObject"/></param>
        /// <param name="size">The size of the <see cref="DrawableObject"/></param>
        /// <returns>A new <see cref="DrawableObject"/> object</returns>
        public static DrawableObject Create(ImageType imageType, Point location, Size size)
        {
            return new DrawableObject(imageType, location, size);
        }

        /// <summary>
        /// Selects the <see cref="DrawableObject"/>
        /// </summary>
        public void Select()
        {
            this.state.Select();
            this.Notify();
        }

        /// <summary>
        /// Deselects the <see cref="DrawableObject"/>
        /// </summary>
        public void Deselect()
        {
            this.state.Deselect();
            this.Notify();
        }

        /// <summary>
        /// Draws the <see cref="DrawableObject"/> on the given <see cref="Graphics"/> object
        /// </summary>
        /// <param name="graphics">The graphics object that will be updated</param>
        public void Draw(Graphics graphics)
        {
            this.state.Draw(graphics);
        }

        /// <summary>
        /// Subscribes the given <see cref="IObserver{T}"/> to the <see cref="DrawableObject"/>,
        /// and the returns a <see cref="IDisposable"/> object that can be used to unsubscribe
        /// </summary>
        /// <param name="observer">The object that wants to observe the <see cref="DrawableObject"/></param>
        /// <returns>An <see cref="IDisposable"/> object that can be used to unsubscribe from the <see cref="DrawableObject"/></returns>
        public IDisposable Subscribe(IObserver<DrawableObject> observer)
        {
            this.observers.Add(observer);
            Unsubscriber unsubsciber = new Unsubscriber(this.observers, observer);
            return unsubsciber;
        }

        /// <summary>
        /// Notify all observers that the <see cref="DrawableObject"/> has been updated
        /// </summary>
        private void Notify()
        {
            foreach (IObserver<DrawableObject> observer in this.observers)
            {
                observer.OnNext(this);
            }
        }

        /// <summary>
        /// Used to unsubscribe from the <see cref="DrawableObject"/>
        /// </summary>
        private class Unsubscriber : IDisposable
        {
            /// <summary>
            /// A list of the <see cref="IObserver{T}"/> objects that are observing
            /// the <see cref="DrawableObject"/>
            /// </summary>
            private List<IObserver<DrawableObject>> observers;

            /// <summary>
            /// The object that can unsubscribe itself from this object.
            /// </summary>
            private IObserver<DrawableObject> observer;

            /// <summary>
            /// Initializes a new instance of the <see cref="Unsubscriber"/> class
            /// </summary>
            /// <param name="observers">The list of <see cref="IObserver{T}"/> objects that the <see cref="DrawableObject"/> keeps</param>
            /// <param name="observer">The <see cref="IObserver{T}"/> that is unsubscribing itself</param>
            public Unsubscriber(List<IObserver<DrawableObject>> observers, IObserver<DrawableObject> observer)
            {
                this.observers = observers;
                this.observer = observer;
            }

            /// <summary>
            /// Unsubscribes the <see cref="IObserver{T}"/> object from the <see cref="DrawableObject"/>
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
