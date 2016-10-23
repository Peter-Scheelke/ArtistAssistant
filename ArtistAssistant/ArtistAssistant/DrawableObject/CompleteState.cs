//-----------------------------------------------------------------------
// <copyright file="CompleteState.cs" company="Peter Scheelke">
//      Copyright (c) Peter Scheelke. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ArtistAssistant.DrawableObject
{
    using System.Drawing;

    /// <summary>
    /// A <see cref="State"/> object that contains both intrinsic and extrinsic state
    /// </summary>
    internal class CompleteState : State
    {
        /// <summary>
        /// The extrinsic state of the <see cref="CompleteState"/>
        /// </summary>
        private ExtrinsicState extrinsicState;

        /// <summary>
        /// The intrinsic state of the <see cref="CompleteState"/>
        /// </summary>
        private State intrinsicState;

        /// <summary>
        /// A value indicating whether the <see cref="CompleteState"/> is currently selected
        /// </summary>
        private bool selected;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompleteState"/> class
        /// </summary>
        /// <param name="intrinsicState">The intrinsic state of the <see cref="CompleteState"/></param>
        /// <param name="extrinsicState">The extrinsic state of the <see cref="CompleteState"/></param>
        public CompleteState(State intrinsicState, ExtrinsicState extrinsicState)
        {
            this.selected = false;
            this.extrinsicState = extrinsicState;
            this.intrinsicState = intrinsicState;
        }

        /// <summary>
        /// Gets the <see cref="System.Drawing.Image"/> contained in the <see cref="CompleteState"/>
        /// Comes from the intrinsic state of the object
        /// </summary>
        public override Image Image
        {
            get
            {
                return this.intrinsicState.Image;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="ImageType.ImageType"/> of <see cref="System.Drawing.Image"/>
        /// </summary>
        public override ImageType ImageType
        {
            get
            {
                return this.intrinsicState.ImageType;
            }

            set
            {
                this.intrinsicState.ImageType = value;
            }
        }

        /// <summary>
        /// Gets or sets the location of the <see cref="CompleteState"/> object
        /// Comes from the extrinsic state
        /// </summary>
        public override Point Location
        {
            get
            {
                return this.extrinsicState.Location;
            }

            set
            {
                this.extrinsicState.Location = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="CompleteState"/> is selected
        /// </summary>
        public override bool Selected
        {
            get
            {
                return this.selected;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="Size"/> of the <see cref="CompleteState"/>
        /// Comes from the extrinsic state
        /// </summary>
        public override Size Size
        {
            get
            {
                return this.extrinsicState.Size;
            }

            set
            {
                this.extrinsicState.Size = value;
            }
        }

        /// <summary>
        /// Creates a new <see cref="CompleteState"/> object
        /// </summary>
        /// <param name="intrinsicState">The intrinsic state of the <see cref="CompleteState"/></param>
        /// <param name="extrinsicState">The extrinsic state of the <see cref="CompleteState"/></param>
        /// <returns>A new <see cref="CompleteState"/> object</returns>
        public static CompleteState Create(State intrinsicState, ExtrinsicState extrinsicState)
        {
            return new CompleteState(intrinsicState, extrinsicState);
        }

        /// <summary>
        /// Deselects the <see cref="CompleteState"/>
        /// </summary>
        /// <param name="graphics">
        /// The <see cref="Graphics"/> object that will be changed to reflect that the <see cref="CompleteState"/>
        /// is no longer selected
        /// </param>
        public override void Deselect(Graphics graphics)
        {
            // Not finished yet
            this.selected = false;
        }

        /// <summary>
        /// Draws the <see cref="CompleteState"/> on the given <see cref="Graphics"/> object
        /// </summary>
        /// <param name="graphics">The <see cref="Graphics"/> object on which the <see cref="CompleteState"/> sill be drawn</param>
        public override void Draw(Graphics graphics)
        {
        }

        /// <summary>
        /// Selects the <see cref="CompleteState"/>
        /// </summary>
        /// <param name="graphics">
        /// The <see cref="Graphics"/> object that will be changed to reflect that the <see cref="CompleteState"/>
        /// is selected
        /// </param>
        public override void Select(Graphics graphics)
        {
            // Not finished yet
            this.selected = true;
        }
    }
}
