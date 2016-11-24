//-----------------------------------------------------------------------
// <copyright file="ClippingArea.cs" company="Peter Scheelke">
//      Copyright (c) Peter Scheelke. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ArtistAssistant.DrawableObject
{
    using System.Drawing;

    /// <summary>
    /// A <see cref="ClippingArea"/> is an area that needs to be redrawn
    /// because something within the area changed
    /// </summary>
    public class ClippingArea
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClippingArea"/> class
        /// </summary>
        /// <param name="location">The top left corner of the <see cref="ClippingArea"/></param>
        /// <param name="size">The <see cref="Size"/> of the <see cref="ClippingArea"/></param>
        public ClippingArea(Point location, Size size)
        {
            this.Location = new Point(location.X, location.Y);
            this.Size = new Size(size.Width, size.Height);
        }

        /// <summary>
        /// Gets or sets the top left corner of the <see cref="ClippingArea"/>
        /// </summary>
        public Point Location { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Size"/> of the <see cref="ClippingArea"/>
        /// </summary>
        public Size Size { get; set; }
    }
}
