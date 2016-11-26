//-----------------------------------------------------------------------
// <copyright file="ClippingArea.cs" company="Peter Scheelke">
//      Copyright (c) Peter Scheelke. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ArtistAssistant.DrawableObject
{
    using System.Collections.Generic;
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

        /// <summary>
        /// Returns whether the given location and size will clip with the <see cref="ClippingArea"/>
        /// </summary>
        /// <param name="location">The location being investigated</param>
        /// <param name="size">The size being investigated</param>
        /// <returns>Whether the given location and size will clip with the <see cref="ClippingArea"/></returns>
        public bool DoesClip(Point location, Size size)
        {
            List<Point> corners = new List<Point>();
            Point topLeft = location;
            corners.Add(topLeft);
            Point topRight = new Point(location.X + size.Width, location.Y);
            corners.Add(topRight);
            Point bottomLeft = new Point(location.X, location.Y + size.Height);
            corners.Add(bottomLeft);
            Point bottomRight = new Point(topRight.X, bottomLeft.Y);
            corners.Add(bottomRight);

            foreach (Point corner in corners)
            {
                if (this.IsInArea(corner))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns whether the given point is in the <see cref="ClippingArea"/>
        /// </summary>
        /// <param name="point">The <see cref="Point"/> being checked</param>
        /// <returns>Whether the given point is in the <see cref="ClippingArea"/></returns>
        private bool IsInArea(Point point)
        {
            int minX = this.Location.X;
            int maxX = this.Location.X + this.Size.Width;
            int minY = this.Location.Y;
            int maxY = this.Location.Y + this.Size.Height;

            if (point.X < minX || point.X > maxX)
            {
                return false;
            }

            if (point.Y < minY || point.Y > maxY)
            {
                return false;
            }

            return true;
        }
    }
}
