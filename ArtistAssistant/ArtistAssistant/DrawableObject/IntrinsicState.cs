//-----------------------------------------------------------------------
// <copyright file="IntrinsicState.cs" company="Peter Scheelke">
//      Copyright (c) Peter Scheelke. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ArtistAssistant.DrawableObject
{
    using System;
    using System.Drawing;

    internal class IntrinsicState : State
    {
        public Image Image { get; set; }

        public override ImageType ImageType
        {
            get
            {
                return this.ImageType;
            }

            set
            {
                //this.ImageType;
            }
        }

        public override Point Location
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override bool Selected
        {
            get
            {
                return false;
            }

            set
            {
                throw new ApplicationException("Error: IntrinsicState.Selected is immutable.");
            }
        }

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

        public override void Deselect(Graphics graphics)
        {
            throw new ApplicationException("Error: Cannot deselect an IntrinsicState object.");
        }

        public override void Draw(Graphics graphics)
        {
            throw new ApplicationException("Error: Cannot draw an IntrinsicState object.");
        }

        public override void Select(Graphics graphics)
        {
            throw new ApplicationException("Error: Cannot select an IntrinsicState object.");
        }
    }
}
