//-----------------------------------------------------------------------
// <copyright file="BringToIndexMode.cs" company="Peter Scheelke">
//      Copyright (c) Peter Scheelke. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ArtistAssistant.Command.Factory.FactoryModes
{
    using Commands;

    /// <summary>
    /// The <see cref="FactoryMode"/> that creates <see cref="BringToIndexCommand"/>s
    /// </summary>
    public class BringToIndexMode : FactoryMode
    {
        /// <summary>
        /// Creates a new <see cref="BringToIndexCommand"/>
        /// </summary>
        /// <param name="parameters">The parameters used to create the <see cref="BringToIndexCommand"/></param>
        /// <returns>A new <see cref="BringToIndexCommand"/></returns>
        protected override ICommand Create(CommandParameters parameters)
        {
            return new BringToIndexCommand(parameters.DrawableObjectList, (int)parameters.StartIndex, (int)parameters.TargetIndex);
        }

        /// <summary>
        /// Makes sure that the proper data was given in the parameters to create an <see cref="BringToIndexCommand"/>
        /// </summary>
        /// <param name="parameters">The <see cref="CommandParameters"/> to be used to create an <see cref="BringToIndexCommand"/></param>
        /// <returns>Whether the given <see cref="CommandParameters"/> object was valid</returns>
        protected override bool ValidateParameters(CommandParameters parameters)
        {
            if (parameters.DrawableObjectList == null)
            {
                return false;
            }

            if (parameters.StartIndex == null)
            {
                return false;
            }

            if (parameters.TargetIndex == null)
            {
                return false;
            }

            return true;
        }
    }
}