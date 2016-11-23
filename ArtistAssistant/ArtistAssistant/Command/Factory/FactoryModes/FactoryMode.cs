//-----------------------------------------------------------------------
// <copyright file="FactoryMode.cs" company="Peter Scheelke">
//      Copyright (c) Peter Scheelke. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ArtistAssistant.Command.Factory.FactoryModes
{
    using System;
    using Commands;

    /// <summary>
    /// Each different type of <see cref="FactoryMode"/> will create a different type of
    /// <see cref="ICommand"/>. There will be a mode for each type of <see cref="ICommand"/>
    /// </summary>
    public abstract class FactoryMode
    {
        /// <summary>
        /// Creates a new <see cref="ICommand"/> based on the type of <see cref="FactoryMode"/>
        /// </summary>
        /// <param name="parameters">An object containing the data to create the <see cref="ICommand"/></param>
        /// <returns>A new <see cref="ICommand"/> created from the parameter object</returns>
        public ICommand CreateCommand(CommandParameters parameters)
        {
            if (!this.ValidateParameters(parameters))
            {
                throw new Exception($"Error: Command parameters for {parameters.CommandType.ToString()} were invalid");
            }
            else
            {
                return this.Create(parameters);
            }
        }

        /// <summary>
        /// Creates a new <see cref="ICommand"/> object based on the given parameters
        /// </summary>
        /// <param name="parameters">The parameters used to create the <see cref="ICommand"/></param>
        /// <returns>A new <see cref="ICommand"/> created from the given parameters</returns>
        protected abstract ICommand Create(CommandParameters parameters);

        /// <summary>
        /// Makes sure that the parameter object contains the correct data to create an <see cref="ICommand"/>
        /// </summary>
        /// <param name="parameters">The <see cref="CommandParameters"/> object being validated</param>
        /// <returns>Whether the parameter object contains the proper data to create an <see cref="ICommand"/></returns>
        protected abstract bool ValidateParameters(CommandParameters parameters);
    }
}
