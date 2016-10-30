//-----------------------------------------------------------------------
// <copyright file="Drawing.cs" company="Peter Scheelke">
//      Copyright (c) Peter Scheelke. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ArtistAssistant.DrawableObject
{
    using System;
    using System.Drawing;

    /// <summary>
    /// A <see cref="Drawing"/> creates an image from the contents of a
    /// <see cref="DrawableObjectList"/>
    /// </summary>
    public class Drawing : IObserver<DrawableObjectList>
    {
        /// <summary>
        /// Allows the <see cref="Drawing"/> to unsubscribe from its <see cref="DrawableObjectList"/>
        /// </summary>
        private IDisposable unsubscriber;

        /// <summary>
        /// The list of <see cref="DrawableObject"/>s that will be rendered onto
        /// the background image
        /// </summary>
        private DrawableObjectList drawableObjectList;

        /// <summary>
        /// The rendered version of the <see cref="Drawing"/>
        /// </summary>
        private Image renderedDrawing;

        /// <summary>
        /// The size of the <see cref="Drawing"/>
        /// </summary>
        private Size size;

        /// <summary>
        /// Initializes a new instance of the <see cref="Drawing"/> class
        /// </summary>
        /// <param name="backgroundImage">The <see cref="Drawing"/>'s background image</param>
        /// <param name="drawableObjectList">
        /// The list of <see cref="DrawableObject"/>s that will be rendered onto
        /// the background image
        /// </param>
        /// <param name="size">The size of the <see cref="Drawing"/></param>
        public Drawing(Image backgroundImage, DrawableObjectList drawableObjectList, Size size)
        {
            this.BackgroundImage = (Bitmap)backgroundImage.Clone();
            this.drawableObjectList = drawableObjectList;
            this.size = size;
            this.unsubscriber = this.drawableObjectList.Subscribe(this);
            this.SelectionPen = Pens.Black;
            this.Render();
        }

        /// <summary>
        /// Gets or sets the background image of the drawing
        /// </summary>
        public Bitmap BackgroundImage { get; set; }

        /// <summary>
        /// Gets or sets the size of the <see cref="Drawing"/>
        /// </summary>
        public Size Size
        {
            get
            {
                return this.size;
            }

            set
            {
                this.size = value;
                this.renderedDrawing = null;
                this.Render();
            }
        }

        /// <summary>
        /// Gets the rendered version of the <see cref="Drawing"/>
        /// </summary>
        public Bitmap RenderedDrawing
        {
            get
            {
                return (Bitmap)this.renderedDrawing;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="Pen"/> used to draw a box around selected items
        /// in the <see cref="Drawing"/>
        /// </summary>
        public Pen SelectionPen { get; set; }

        /// <summary>
        /// Creates a new instance of the <see cref="Drawing"/> class
        /// </summary>
        /// <param name="backgroundImage">The <see cref="Drawing"/>'s background image</param>
        /// <param name="drawableObjectList">
        /// The list of <see cref="DrawableObject"/>s that will be rendered onto
        /// the background image
        /// </param>
        /// /// <param name="size">The size of the <see cref="Drawing"/></param>
        /// <returns>A new <see cref="Drawing"/> object</returns>
        public static Drawing Create(Image backgroundImage, DrawableObjectList drawableObjectList, Size size)
        {
            return new Drawing(backgroundImage, drawableObjectList, size);
        }

        /// <summary>
        /// Disposes the <see cref="Drawing"/>
        /// </summary>
        public void Dispose()
        {
            this.unsubscriber.Dispose();
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
        /// <param name="error">The parameter is not used</param>
        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Notifies the <see cref="Drawing"/> that its <see cref="DrawableObjectList"/>
        /// has changed
        /// </summary>
        /// <param name="value">The <see cref="drawableObjectList"/> that changed</param>
        public void OnNext(DrawableObjectList value)
        {
            this.Render();
        }

        /// <summary>
        /// Renders the <see cref="Drawing"/> onto <see cref="renderedDrawing"/>
        /// </summary>
        private void Render()
        {
            if (this.renderedDrawing == null)
            {
                this.renderedDrawing = new Bitmap(this.Size.Width, this.Size.Height);
            }

            using (Graphics graphics = Graphics.FromImage(this.renderedDrawing))
            {
                graphics.DrawImage(this.BackgroundImage, 0, 0, this.Size.Width, this.Size.Height);
                foreach (DrawableObject item in this.drawableObjectList.RenderOrder)
                {
                    item.Draw(graphics);
                }

                foreach (DrawableObject item in this.drawableObjectList.RenderOrder)
                {
                    if (item.Selected)
                    {
                        Point topLeft = item.Location;
                        Point topRight = new Point(topLeft.X + item.Size.Width, topLeft.Y);
                        Point bottomRight = new Point(topRight.X, topRight.Y + item.Size.Height);
                        Point bottomLeft = new Point(topLeft.X, bottomRight.Y);

                        graphics.DrawLine(this.SelectionPen, topLeft, topRight);
                        graphics.DrawLine(this.SelectionPen, topRight, bottomRight);
                        graphics.DrawLine(this.SelectionPen, bottomRight, bottomLeft);
                        graphics.DrawLine(this.SelectionPen, bottomLeft, topLeft);
                    }
                }
            }
        }
    }
}
