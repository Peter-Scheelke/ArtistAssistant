//-----------------------------------------------------------------------
// <copyright file="State.cs" company="Peter Scheelke">
//      Copyright (c) Peter Scheelke. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ArtistAssistant.DrawableObject
{
    using System.Drawing;

    /// <summary>
    /// An abstract class that defines the interface to the state of the <see cref="DrawableObject"/>
    /// </summary>
    internal abstract class State
    {
        /// <summary>
        /// Gets or sets the <see cref="ImageType"/> of the <see cref="DrawableObject"/>
        /// </summary>
        public abstract ImageType ImageType { get; set; }

        /// <summary>
        /// Gets the <see cref="State"/>'s <see cref="Image"/>
        /// </summary>
        public abstract Image Image { get; }

        /// <summary>
        /// Gets or sets the location of the <see cref="DrawableObject"/>
        /// </summary>
        public abstract Point Location { get; set; }

        /// <summary>
        /// Gets or sets the size of the <see cref="DrawableObject"/>
        /// </summary>
        public abstract Size Size { get; set; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="DrawableObject"/> is selected
        /// </summary>
        public abstract bool Selected { get; }

        /// <summary>
        /// Selects the <see cref="DrawableObject"/>
        /// </summary>
        /// <param name="graphics">The graphics object that will be updated</param>
        public abstract void Select(Graphics graphics);

        /// <summary>
        /// Deselects the <see cref="DrawableObject"/>
        /// </summary>
        /// <param name="graphics">The graphics object that will be updated</param>
        public abstract void Deselect(Graphics graphics);

        /// <summary>
        /// Draws the <see cref="DrawableObject"/> on the given <see cref="Graphics"/> object
        /// </summary>
        /// <param name="graphics">The graphics object that will be updated</param>
        public abstract void Draw(Graphics graphics);
    }
}