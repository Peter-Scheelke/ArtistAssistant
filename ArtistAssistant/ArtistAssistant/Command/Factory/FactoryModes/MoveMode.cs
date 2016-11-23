//-----------------------------------------------------------------------
// <copyright file="MoveMode.cs" company="Peter Scheelke">
//      Copyright (c) Peter Scheelke. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ArtistAssistant.Command.Factory.FactoryModes
{
    using System.Drawing;
    using Commands;

    /// <summary>
    /// The <see cref="FactoryMode"/> that creates <see cref="MoveCommand"/>s
    /// </summary>
    public class MoveMode : FactoryMode
    {
        /// <summary>
        /// Creates a new <see cref="MoveCommand"/>
        /// </summary>
        /// <param name="parameters">The parameters used to create the <see cref="MoveCommand"/></param>
        /// <returns>A new <see cref="MoveCommand"/></returns>
        protected override ICommand Create(CommandParameters parameters)
        {
            return new MoveCommand(parameters.DrawableObjectList, parameters.AffectedDrawableObject, (Point)parameters.Location);
        }

        /// <summary>
        /// Makes sure that the proper data was given in the parameters to create an <see cref="MoveCommand"/>
        /// </summary>
        /// <param name="parameters">The <see cref="CommandParameters"/> to be used to create an <see cref="MoveCommand"/></param>
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

            if (parameters.Location == null)
            {
                return false;
            }

            return true;
        }
    }
}