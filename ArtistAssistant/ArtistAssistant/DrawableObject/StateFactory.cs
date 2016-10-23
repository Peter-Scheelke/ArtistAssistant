//-----------------------------------------------------------------------
// <copyright file="StateFactory.cs" company="Peter Scheelke">
//      Copyright (c) Peter Scheelke. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace DrawableObject
{
    using System.Collections.Generic;
    using System.Drawing;

    /// <summary>
    /// Creates <see cref="State"/> objects
    /// </summary>
    internal class StateFactory
    {
        /// <summary>
        /// A <see cref="Dictionary{TKey, TValue}"/> containing the <see cref="IntrinsicState"/>
        /// objects that have been created so far.
        /// </summary>
        private static Dictionary<ImageType, IntrinsicState> intrinsicStates;

        /// <summary>
        /// Creates a new <see cref="State"/> object
        /// </summary>
        /// <param name="imageType">The <see cref="ImageType"/> of the <see cref="State"/>'s <see cref="Image"/></param>
        /// <param name="location">The location of the <see cref="State"/></param>
        /// <param name="size">The <see cref="Size"/> of the state</param>
        /// <returns>A new <see cref="State"/> object</returns>
        public virtual State Create(ImageType imageType, Point location, Size size)
        {
            if (StateFactory.intrinsicStates == null)
            {
                StateFactory.intrinsicStates = new Dictionary<ImageType, IntrinsicState>();
            }

            if (!StateFactory.intrinsicStates.ContainsKey(imageType))
            {
                StateFactory.intrinsicStates.Add(imageType, IntrinsicState.Create(imageType));
            }

            IntrinsicState intrinsicState = StateFactory.intrinsicStates[imageType];
            ExtrinsicState extrinsicState = ExtrinsicState.Create(location, size);
            return CompleteState.Create(intrinsicState, extrinsicState);
        }
    }
}
