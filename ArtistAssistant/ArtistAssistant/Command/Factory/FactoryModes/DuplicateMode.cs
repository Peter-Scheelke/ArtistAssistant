//-----------------------------------------------------------------------
// <copyright file="DuplicateMode.cs" company="Peter Scheelke">
//      Copyright (c) Peter Scheelke. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ArtistAssistant.Command.Factory.FactoryModes
{
    using Commands;

    /// <summary>
    /// The <see cref="FactoryMode"/> that creates <see cref="DuplicateCommand"/>s
    /// </summary>
    public class DuplicateMode : FactoryMode
    {
        /// <summary>
        /// Creates a new <see cref="DuplicateCommand"/>
        /// </summary>
        /// <param name="parameters">The parameters used to create the <see cref="DuplicateCommand"/></param>
        /// <returns>A new <see cref="DuplicateCommand"/></returns>
        protected override ICommand Create(CommandParameters parameters)
        {
            return new DuplicateCommand(parameters.DrawableObjectList, parameters.AffectedDrawableObject);
        }

        /// <summary>
        /// Makes sure that the proper data was given in the parameters to create an <see cref="DuplicateCommand"/>
        /// </summary>
        /// <param name="parameters">The <see cref="CommandParameters"/> to be used to create an <see cref="DuplicateCommand"/></param>
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

            return true;
        }
    }
}