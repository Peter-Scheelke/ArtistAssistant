//-----------------------------------------------------------------------
// <copyright file="IntrinsicState.cs" company="Peter Scheelke">
//      Copyright (c) Peter Scheelke. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace DrawableObject
{
    using System;
    using System.Drawing;

    /// <summary>
    /// The intrinsic state of a <see cref="DrawableObject"/>.
    /// </summary>
    internal class IntrinsicState : State
    {
        /// <summary>
        /// The <see cref="Image"/> contained in the intrinsic state
        /// </summary>
        private Image image;

        /// <summary>
        /// The <see cref="ImageType.ImageType"/> of the <see cref="System.Drawing.Image"/>
        /// contained in the <see cref="IntrinsicState"/> object
        /// </summary>
        private ImageType imageType;

        /// <summary>
        /// Initializes a new instance of the <see cref="IntrinsicState"/> class
        /// </summary>
        /// <param name="imageType">
        /// The <see cref="ImageType.ImageType"/> of the <see cref="System.Drawing.Image"/> 
        /// contained by the <see cref="IntrinsicState"/>
        /// </param>
        public IntrinsicState(ImageType imageType)
        {
            this.ImageType = imageType;
        }

        /// <summary>
        /// Gets the <see cref="System.Drawing.Image"/> contained by the <see cref="IntrinsicState"/>
        /// </summary>
        public override Image Image
        {
            get
            {
                return this.image;
            }
        }

        /// <summary>
        /// The <see cref="ImageType"/> of the <see cref=""/>
        /// </summary>
        public override ImageType ImageType
        {
            get
            {
                return this.imageType;
            }

            set
            {
                this.imageType = value;
                this.image = ImagePool.GetImage(this.ImageType);
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="Location"/> of the <see cref="IntrinsicState"/>.
        /// Given that neither of these really makes sense, it will either return a new default <see cref="Point"/>
        /// object or throw an exception explaining why setting this doesn't work.
        /// </summary>
        public override Point Location
        {
            get
            {
                return new Point();
            }

            set
            {
                throw new ApplicationException("Error: IntrinsicState.Location is immutable.");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="IntrinsicState"/> is selected.
        /// Given that neither of these really makes sense, it will either return false
        /// object or throw an exception explaining why setting this doesn't work.
        /// </summary>
        public override bool Selected
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets or sets the size of the <see cref="IntrinsicState"/>.
        /// Given that neither of these really makes sense, it will either return a new default <see cref="Size"/>
        /// object or throw an exception explaining why setting this doesn't work.
        /// </summary>
        public override Size Size
        {
            get
            {
                return new Size();
            }

            set
            {
                throw new ApplicationException("Error: IntrinsicState.Size is immutable.");
            }
        }

        /// <summary>
        /// Returns a new instance of the <see cref="IntrinsicState"/> class
        /// </summary>
        /// <param name="imageType">
        /// The <see cref="ImageType"/> of the <see cref="System.Drawing.Image"/> 
        /// contained by the <see cref="IntrinsicState"/>
        /// </param>
        /// <returns>A new instance of the <see cref="IntrinsicState"/> class</returns>
        public static IntrinsicState Create(ImageType imageType)
        {
            return new IntrinsicState(imageType);
        }

        /// <summary>
        /// Deselects the <see cref="IntrinsicState"/>. Except that doesn't make sense, so it throws an exception
        /// instead.
        /// </summary>
        /// <param name="graphics">The graphics that would be updated if doing this made any sense.</param>
        public override void Deselect(Graphics graphics)
        {
            throw new ApplicationException("Error: Cannot deselect an IntrinsicState object.");
        }

        /// <summary>
        /// Draws the <see cref="IntrinsicState"/>. Except that doesn't make any sense, so it throws an exception
        /// instead.
        /// </summary>
        /// <param name="graphics">The graphics that would be updated if doing this made any sense.</param>
        public override void Draw(Graphics graphics)
        {
            throw new ApplicationException("Error: Cannot draw an IntrinsicState object.");
        }

        /// <summary>
        /// Selects the <see cref="IntrinsicState"/>. Except that doesn't make any sense, so it throws an exception
        /// instead.
        /// </summary>
        /// <param name="graphics">The graphics that would be updated if doing this made any sense.</param>
        public override void Select(Graphics graphics)
        {
            throw new ApplicationException("Error: Cannot select an IntrinsicState object.");
        }
    }
}