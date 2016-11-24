//-----------------------------------------------------------------------
// <copyright file="SelectMode.cs" company="Peter Scheelke">
//      Copyright (c) Peter Scheelke. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ArtistAssistant.Command.Factory.FactoryModes
{
    using Commands;

    /// <summary>
    /// The <see cref="FactoryMode"/> that creates <see cref="SelectCommand"/>s
    /// </summary>
    public class SelectMode : FactoryMode
    {
        /// <summary>
        /// Creates a new <see cref="SelectCommand"/>
        /// </summary>
        /// <param name="parameters">The parameters used to create the <see cref="SelectCommand"/></param>
        /// <returns>A new <see cref="SelectCommand"/></returns>
        protected override ICommand Create(CommandParameters parameters)
        {
            return SelectCommand.Create(parameters.DrawableObjectList, parameters.AffectedDrawableObject);
        }

        /// <summary>
        /// Makes sure that the proper data was given in the parameters to create an <see cref="SelectCommand"/>
        /// </summary>
        /// <param name="parameters">The <see cref="CommandParameters"/> to be used to create an <see cref="SelectCommand"/></param>
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