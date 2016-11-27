//-----------------------------------------------------------------------
// <copyright file="MacroCommand.cs" company="Peter Scheelke">
//      Copyright (c) Peter Scheelke. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ArtistAssistant.Command.Commands
{
    using System.Collections.Generic;
    using DrawableObject;

    /// <summary>
    /// A <see cref="MacroCommand"/> is an <see cref="ICommand"/> that contains several other
    /// <see cref="ICommand"/>s. This allows those commands to be executed or undone at the same time
    /// </summary>
    public class MacroCommand : ICommand
    {
        /// <summary>
        /// A list of the <see cref="ICommand"/>s that will be executed when the <see cref="MacroCommand"/> executes
        /// </summary>
        private List<ICommand> commands;

        /// <summary>
        /// Initializes a new instance of the <see cref="MacroCommand"/> class
        /// </summary>
        /// <param name="drawableObjectList">The list of <see cref="DrawableObject"/>s that will be affected by the <see cref="MacroCommand"/></param>
        /// <param name="commands">The <see cref="ICommand"/>s that will be executed on the given list of <see cref="DrawableObject"/>s</param>
        public MacroCommand(DrawableObjectList drawableObjectList, List<ICommand> commands)
        {
            this.commands = commands;
            this.DrawableObjectList = drawableObjectList;
        }

        /// <summary>
        /// Gets or sets the list of <see cref="DrawableObject"/>s that will be affected by executing the <see cref="MacroCommand"/>
        /// </summary>
        public DrawableObjectList DrawableObjectList { get; set; }

        /// <summary>
        /// Creates a new instance of the <see cref="MacroCommand"/> class
        /// </summary>
        /// <param name="drawableObjectList">The list of <see cref="DrawableObject"/>s that will be affected by the <see cref="MacroCommand"/></param>
        /// <param name="commands">The <see cref="ICommand"/>s that will be executed on the given list of <see cref="DrawableObject"/>s</param>
        /// <returns>A new instance of the <see cref="MacroCommand"/> class</returns>
        public static MacroCommand Create(DrawableObjectList drawableObjectList, List<ICommand> commands)
        {
            return new MacroCommand(drawableObjectList, commands);
        }

        /// <summary>
        /// Executes the <see cref="MacroCommand"/>
        /// </summary>
        public void Execute()
        {
            if (this.commands != null)
            {
                foreach (ICommand command in this.commands)
                {
                    command.Execute();
                }
            }
        }

        /// <summary>
        /// Undoes the <see cref="MacroCommand"/>
        /// </summary>
        public void Undo()
        {
            if (this.commands != null)
            {
                for (int i = this.commands.Count - 1; i >= 0; --i)
                {
                    this.commands[i].Undo();
                }
            }
        }
    }
}
