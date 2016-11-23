//-----------------------------------------------------------------------
// <copyright file="CommandFactory.cs" company="Peter Scheelke">
//      Copyright (c) Peter Scheelke. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ArtistAssistant.Command.Factory
{
    using System.Collections.Generic;
    using Commands;
    using FactoryModes;

    /// <summary>
    /// Creates <see cref="ICommand"/> objects
    /// </summary>
    public class CommandFactory
    {
        /// <summary>
        /// The different modes the <see cref="CommandFactory"/> can be in
        /// Each one creates a different type of <see cref="ICommand"/>
        /// </summary>
        private Dictionary<CommandType, FactoryMode> factoryModes;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandFactory"/> class
        /// </summary>
        public CommandFactory()
        {
            this.factoryModes = new Dictionary<CommandType, FactoryMode>();
            this.factoryModes.Add(CommandType.Add, new AddMode());
            this.factoryModes.Add(CommandType.BringToIndex, new BringToIndexMode());
            this.factoryModes.Add(CommandType.Deselect, new DeselectMode());
            this.factoryModes.Add(CommandType.Duplicate, new DuplicateMode());
            this.factoryModes.Add(CommandType.Move, new MoveMode());
            this.factoryModes.Add(CommandType.Remove, new RemoveMode());
            this.factoryModes.Add(CommandType.Scale, new ScaleMode());
            this.factoryModes.Add(CommandType.Select, new SelectMode());
        }

        /// <summary>
        /// Creates a new instance of the <see cref="CommandFactory"/> class
        /// </summary>
        /// <returns>A new instance of the <see cref="CommandFactory"/> class</returns>
        public static CommandFactory Create()
        {
            return new CommandFactory();
        }

        /// <summary>
        /// Creates an <see cref="ICommand"/> object based on contents of the the given 
        /// <see cref="CommandParameters"/> object
        /// </summary>
        /// <param name="parameters">
        /// The parameters used to create the correct
        /// <see cref="ICommand"/> object
        /// </param>
        /// <returns>
        /// An <see cref="ICommand"/> object based on the contents 
        /// of the given <see cref="CommandParameters"/> object
        /// </returns>
        public ICommand CreateCommand(CommandParameters parameters)
        {
            return this.factoryModes[parameters.CommandType].CreateCommand(parameters);
        }
    }
}