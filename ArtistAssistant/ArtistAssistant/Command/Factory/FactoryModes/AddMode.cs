//-----------------------------------------------------------------------
// <copyright file="AddMode.cs" company="Peter Scheelke">
//      Copyright (c) Peter Scheelke. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ArtistAssistant.Command.Factory.FactoryModes
{
    using Commands;

    /// <summary>
    /// The <see cref="FactoryMode"/> that creates <see cref="AddCommand"/>s
    /// </summary>
    public class AddMode : FactoryMode
    {
        /// <summary>
        /// Creates a new <see cref="AddCommand"/>
        /// </summary>
        /// <param name="parameters">The parameters used to create the <see cref="AddCommand"/></param>
        /// <returns>A new <see cref="AddCommand"/></returns>
        protected override ICommand Create(CommandParameters parameters)
        {
            return AddCommand.Create(parameters.DrawableObjectList, parameters.AffectedDrawableObject);
        }

        /// <summary>
        /// Makes sure that the proper data was given in the parameters to create an <see cref="AddCommand"/>
        /// </summary>
        /// <param name="parameters">The <see cref="CommandParameters"/> to be used to create an <see cref="AddCommand"/></param>
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