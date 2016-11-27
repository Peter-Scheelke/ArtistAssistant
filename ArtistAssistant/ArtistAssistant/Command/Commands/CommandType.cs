//-----------------------------------------------------------------------
// <copyright file="CommandType.cs" company="Peter Scheelke">
//      Copyright (c) Peter Scheelke. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ArtistAssistant.Command.Commands
{
    /// <summary>
    /// An enumeration containing the types of commands that
    /// implement <see cref="ICommand"/>
    /// </summary>
    public enum CommandType
    {
        /// <summary>
        /// A command that adds a new <see cref="DrawableObject.DrawableObject"/>
        /// </summary>
        Add,

        /// <summary>
        /// A command that removes a <see cref="DrawableObject.DrawableObject"/>
        /// </summary>
        Remove,

        /// <summary>
        /// A command that selects a <see cref="DrawableObject.DrawableObject"/>
        /// </summary>
        Select,

        /// <summary>
        /// A command that deselects a <see cref="DrawableObject.DrawableObject"/>
        /// </summary>
        Deselect,

        /// <summary>
        /// A command that scales a <see cref="DrawableObject.DrawableObject"/>
        /// </summary>
        Scale,

        /// <summary>
        /// A command the moves a <see cref="DrawableObject.DrawableObject"/>
        /// </summary>
        Move,

        /// <summary>
        /// A command the duplicates a <see cref="DrawableObject.DrawableObject"/>
        /// </summary>
        Duplicate,

        /// <summary>
        /// A command that changes the order in which <see cref="DrawableObject.DrawableObject"/>s
        /// are rendered
        /// </summary>
        BringToIndex,

        /// <summary>
        /// A command made up of sub-commands, all of which will be executed/undone at once
        /// </summary>
        Macro
    }
}
