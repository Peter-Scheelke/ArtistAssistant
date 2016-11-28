//-----------------------------------------------------------------------
// <copyright file="Drawing.cs" company="Peter Scheelke">
//      Copyright (c) Peter Scheelke. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ArtistAssistant.DrawableObject
{
    using System;
    using System.Collections.Generic;
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
        /// The background image of the <see cref="Drawing"/>
        /// </summary>
        private Bitmap backgroundImage;

        /// <summary>
        /// The background image drawn at the size of the <see cref="Drawing"/>
        /// </summary>
        private Bitmap correctlySizedBackgroundImage;

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
            this.backgroundImage = (Bitmap)backgroundImage.Clone();
            this.drawableObjectList = drawableObjectList;
            this.size = size;
            this.unsubscriber = this.drawableObjectList.Subscribe(this);
            this.SelectionPen = Pens.Black;
            this.correctlySizedBackgroundImage = new Bitmap(this.size.Width, this.size.Height);
            using (Graphics graphics = Graphics.FromImage(this.correctlySizedBackgroundImage))
            {
                graphics.DrawImage(this.BackgroundImage, 0, 0, this.Size.Width, this.Size.Height);
            }

            this.Render();
        }

        /// <summary>
        /// Gets the background image of the drawing
        /// </summary>
        public Bitmap BackgroundImage
        {
            get
            {
                return this.backgroundImage;
            }
        }

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
            bool shouldRenderBackground = false;
            if (this.renderedDrawing == null)
            {
                this.renderedDrawing = new Bitmap(this.Size.Width, this.Size.Height);
                shouldRenderBackground = true;
            }

            using (Graphics graphics = Graphics.FromImage(this.renderedDrawing))
            {
                if (shouldRenderBackground)
                {
                    graphics.DrawImage(this.BackgroundImage, 0, 0, this.Size.Width, this.Size.Height);
                }

                this.RenderDrawableObjects(graphics);

                if (this.drawableObjectList.SelectedObject != null)
                {
                    this.DrawSelectionBox(graphics);
                }
            }
        }

        /// <summary>
        /// Render all of the <see cref="DrawableObject"/>s in the <see cref="DrawableObjectList"/>
        /// onto the <see cref="Drawing"/>'s rendered drawing
        /// </summary>
        /// <param name="graphics">The <see cref="Graphics"/> object used to do the rendering</param>
        private void RenderDrawableObjects(Graphics graphics)
        {
            Dictionary<int, DrawableObject> drawableObjectsToRedraw = new Dictionary<int, DrawableObject>();
            Queue<ClippingArea> areasToRedraw = new Queue<ClippingArea>();
            foreach (ClippingArea area in this.drawableObjectList.ClippingAreaBuffer)
            {
                areasToRedraw.Enqueue(area);
            }

            while (areasToRedraw.Count > 0)
            {
                ClippingArea area = areasToRedraw.Dequeue();
                foreach (DrawableObject drawableObject in this.drawableObjectList)
                {
                    if (!drawableObjectsToRedraw.ContainsKey(drawableObject.Id))
                    {
                        if (area.DoesClip(drawableObject.Location, drawableObject.Size))
                        {
                            drawableObjectsToRedraw.Add(drawableObject.Id, drawableObject);
                            areasToRedraw.Enqueue(new ClippingArea(drawableObject.Location, drawableObject.Size));
                        }
                    }
                }
            }

            foreach (DrawableObject drawableObject in drawableObjectsToRedraw.Values)
            {
                this.ClearClippingArea(graphics, new ClippingArea(drawableObject.Location, drawableObject.Size));
            }

            foreach (ClippingArea area in this.drawableObjectList.ClippingAreaBuffer)
            {
                this.ClearClippingArea(graphics, area);
            }

            foreach (DrawableObject drawableObject in this.drawableObjectList.RenderOrder)
            {
                if (drawableObjectsToRedraw.ContainsKey(drawableObject.Id))
                {
                    drawableObject.Draw(graphics);
                }
            }
        }

        /// <summary>
        /// Draw a selection box around the currently selected <see cref="DrawableObject"/>
        /// </summary>
        /// <param name="graphics">The <see cref="Graphics"/> object used to draw the box</param>
        private void DrawSelectionBox(Graphics graphics)
        {
            DrawableObject item = this.drawableObjectList.SelectedObject;
            Point topLeft = item.Location;
            Point topRight = new Point(topLeft.X + item.Size.Width, topLeft.Y);
            Point bottomRight = new Point(topRight.X, topRight.Y + item.Size.Height);
            Point bottomLeft = new Point(topLeft.X, bottomRight.Y);

            graphics.DrawLine(this.SelectionPen, topLeft, topRight);
            graphics.DrawLine(this.SelectionPen, topRight, bottomRight);
            graphics.DrawLine(this.SelectionPen, bottomRight, bottomLeft);
            graphics.DrawLine(this.SelectionPen, bottomLeft, topLeft);
        }

        /// <summary>
        /// Draw the background over the given <see cref="ClippingArea"/>
        /// </summary>
        /// <param name="graphics">The <see cref="Graphics"/> doing the drawing</param>
        /// <param name="area">The <see cref="ClippingArea"/> being drawn over</param>
        private void ClearClippingArea(Graphics graphics, ClippingArea area)
        {
            int width = area.Size.Width + (int)this.SelectionPen.Width;
            int height = area.Size.Height + (int)this.SelectionPen.Width;
            Rectangle cloneRect;

            if (area.Location.X + width > this.Size.Width)
            {
                width -= area.Location.X + width - this.Size.Width;
            }

            if (area.Location.Y + height > this.Size.Height)
            {
                height -= area.Location.Y + height - this.Size.Height;
            }

            cloneRect = new Rectangle(area.Location.X, area.Location.Y, width, height);
            System.Drawing.Imaging.PixelFormat format = this.correctlySizedBackgroundImage.PixelFormat;
            Bitmap cloneBitmap = this.correctlySizedBackgroundImage.Clone(cloneRect, format);
            graphics.DrawImage(cloneBitmap, area.Location.X, area.Location.Y);
        }
    }
}
