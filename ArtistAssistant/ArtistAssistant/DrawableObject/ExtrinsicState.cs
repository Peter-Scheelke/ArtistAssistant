//-----------------------------------------------------------------------
// <copyright file="ExtrinsicState.cs" company="Peter Scheelke">
//      Copyright (c) Peter Scheelke. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ArtistAssistant.DrawableObject
{
    using System.Drawing;
    
    /// <summary>
    /// The extrinsic state of a <see cref="DrawableObject"/>
    /// </summary>
    internal class ExtrinsicState
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExtrinsicState"/> class
        /// </summary>
        /// <param name="imageType">
        /// The type of the image that the <see cref="DrawableObject"/> should reference in its
        /// <see cref="IntrinsicState"/>
        /// </param>
        /// <param name="location">The location of the <see cref="DrawableObject"/></param>
        /// <param name="size">The size of the <see cref="DrawableObject"/></param>
        public ExtrinsicState(ImageType imageType, Point location, Size size)
        {
            this.ImageType = imageType;
            this.Location = location;
            this.Size = size;
        }

        /// <summary>
        /// Gets or sets the <see cref="ImageType"/> of the <see cref="DrawableObject"/> containing
        /// the <see cref="ExtrinsicState"/> object
        /// </summary>
        public ImageType ImageType { get; set; }

        /// <summary>
        /// Gets or sets the location of the <see cref="DrawableObject"/> containing the
        /// <see cref="ExtrinsicState"/> 
        /// </summary>
        public Point Location { get; set; }

        /// <summary>
        /// Gets or sets the size of the <see cref="DrawableObject"/> containing the
        /// <see cref="ExtrinsicState"/> 
        /// </summary>
        public Size Size { get; set; }

        /// <summary>
        /// Returns a new instance of the <see cref="ExtrinsicState"/> class
        /// </summary>
        /// <param name="imageType">
        /// The type of the image that the <see cref="DrawableObject"/> should reference in its
        /// <see cref="IntrinsicState"/>
        /// </param>
        /// <param name="location">The location of the <see cref="DrawableObject"/></param>
        /// <param name="size">The size of the <see cref="DrawableObject"/></param>
        /// <returns>A new instance of the <see cref="ExtrinsicState"/> class</returns>
        public static ExtrinsicState Create(ImageType imageType, Point location, Size size)
        {
            return new ExtrinsicState(imageType, location, size);
        }
    }
}