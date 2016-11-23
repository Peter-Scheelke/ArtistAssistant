//-----------------------------------------------------------------------
// <copyright file="ScaleMode.cs" company="Peter Scheelke">
//      Copyright (c) Peter Scheelke. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ArtistAssistant.Command.Factory.FactoryModes
{
    using System.Drawing;
    using Commands;

    /// <summary>
    /// The <see cref="FactoryMode"/> that creates <see cref="ScaleCommand"/>s
    /// </summary>
    public class ScaleMode : FactoryMode
    {
        /// <summary>
        /// Creates a new <see cref="ScaleCommand"/>
        /// </summary>
        /// <param name="parameters">The parameters used to create the <see cref="ScaleCommand"/></param>
        /// <returns>A new <see cref="ScaleCommand"/></returns>
        protected override ICommand Create(CommandParameters parameters)
        {
            return new ScaleCommand(parameters.DrawableObjectList, parameters.AffectedDrawableObject, (Size)parameters.Size);
        }

        /// <summary>
        /// Makes sure that the proper data was given in the parameters to create an <see cref="ScaleCommand"/>
        /// </summary>
        /// <param name="parameters">The <see cref="CommandParameters"/> to be used to create an <see cref="ScaleCommand"/></param>
        /// <returns>Whether the given <see cref="CommandParameters"/> object was valid</returns>
        protected override bool ValidateParameters(CommandParameters parameters)
        {
            if (parameters.DrawableObjectList == null)
            {
                return false;
            }

            if (parameters.AffectedDrawableObject == null)
            {
                return false;
            }

            if (parameters.Size == null)
            {
                return false;
            }

            return true;
        }
    }
}