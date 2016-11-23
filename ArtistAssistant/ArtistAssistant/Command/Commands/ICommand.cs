//-----------------------------------------------------------------------
// <copyright file="ICommand.cs" company="Peter Scheelke">
//      Copyright (c) Peter Scheelke. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ArtistAssistant.Command.Commands
{
    using DrawableObject;

    /// <summary>
    /// The interface used to create commands
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Gets or sets the list of <see cref="DrawableObject"/>s
        /// </summary>
        DrawableObjectList DrawableObjectList { get; set; }

        /// <summary>
        /// Execute the command
        /// </summary>
        void Execute();

        /// <summary>
        /// Undo the command
        /// </summary>
        void Undo();
    }
}
