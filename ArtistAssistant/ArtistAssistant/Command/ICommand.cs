﻿//-----------------------------------------------------------------------
// <copyright file="ICommand.cs" company="Peter Scheelke">
//      Copyright (c) Peter Scheelke. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ArtistAssistant.Command
{
    /// <summary>
    /// The interface used to create commands
    /// </summary>
    public interface ICommand
    {
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
