//-----------------------------------------------------------------------
// <copyright file="DeselectMode.cs" company="Peter Scheelke">
//      Copyright (c) Peter Scheelke. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ArtistAssistant.Command.Factory.FactoryModes
{
    using Commands;

    /// <summary>
    /// The <see cref="FactoryMode"/> that creates <see cref="DeselectCommand"/>s
    /// </summary>
    public class DeselectMode : FactoryMode
    {
        /// <summary>
        /// Creates a new <see cref="DeselectCommand"/>
        /// </summary>
        /// <param name="parameters">The parameters used to create the <see cref="DeselectCommand"/></param>
        /// <returns>A new <see cref="DeselectCommand"/></returns>
        protected override ICommand Create(CommandParameters parameters)
        {
            return new DeselectCommand(parameters.DrawableObjectList);
        }

        /// <summary>
        /// Makes sure that the proper data was given in the parameters to create an <see cref="DeselectCommand"/>
        /// </summary>
        /// <param name="parameters">The <see cref="CommandParameters"/> to be used to create an <see cref="DeselectCommand"/></param>
        /// <returns>Whether the given <see cref="CommandParameters"/> object was valid</returns>
        protected override bool ValidateParameters(CommandParameters parameters)
        {
            if (parameters.DrawableObjectList == null)
            {
                return false;
            }

            return true;
        }
    }
}