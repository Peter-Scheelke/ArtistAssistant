//-----------------------------------------------------------------------
// <copyright file="DrawableObject.cs" company="Peter Scheelke">
//      Copyright (c) Peter Scheelke. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ArtistAssistant.DrawableObject
{
    using System.Drawing;

    /// <summary>
    /// A <see cref="DrawableObject"/> can be used to draw images on
    /// <see cref="Graphics"/> objects. This class is basically just
    /// a wrapper for the internal classes
    /// </summary>
    public class DrawableObject
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
        /// <param name="graphics">The graphics object that will be updated</param>
        public void Select(Graphics graphics)
        {
            this.state.Select(graphics);
        }

        /// <summary>
        /// Deselects the <see cref="DrawableObject"/>
        /// </summary>
        /// <param name="graphics">The graphics object that will be updated</param>
        public void Deselect(Graphics graphics)
        {
            this.state.Deselect(graphics);
        }

        /// <summary>
        /// Draws the <see cref="DrawableObject"/> on the given <see cref="Graphics"/> object
        /// </summary>
        /// <param name="graphics">The graphics object that will be updated</param>
        public void Draw(Graphics graphics)
        {
            this.state.Draw(graphics);
        }
    }
}
