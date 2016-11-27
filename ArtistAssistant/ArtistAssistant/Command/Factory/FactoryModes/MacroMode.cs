//-----------------------------------------------------------------------
// <copyright file="MacroMode.cs" company="Peter Scheelke">
//      Copyright (c) Peter Scheelke. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ArtistAssistant.Command.Factory.FactoryModes
{
    using Commands;

    /// <summary>
    /// The <see cref="FactoryMode"/> that creates <see cref="MacroCommand"/>s
    /// </summary>
    public class MacroMode : FactoryMode
    {
        /// <summary>
        /// Creates a new <see cref="MacroCommand"/> from the given parameters
        /// </summary>
        /// <param name="parameters">A <see cref="CommandParameters"/> object used to create <see cref="MacroCommand"/>s</param>
        /// <returns>A new <see cref="MacroCommand"/> from the given parameters</returns>
        protected override ICommand Create(CommandParameters parameters)
        {
            return MacroCommand.Create(parameters.DrawableObjectList, parameters.Commands);
        }

        /// <summary>
        /// Ensures that a <see cref="CommandParameters"/> object contains the correct data
        /// to create <see cref="MacroCommand"/>s
        /// </summary>
        /// <param name="parameters">The <see cref="CommandParameters"/> object being checked</param>
        /// <returns>Whether the given parameters are valid</returns>
        protected override bool ValidateParameters(CommandParameters parameters)
        {
            if (parameters.DrawableObjectList == null)
            {
                return false;
            }

            if (parameters.Commands == null)
            {
                return false;
            }

            return true;
        }
    }
}
