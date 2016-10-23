//-----------------------------------------------------------------------
// <copyright file="CommandType.cs" company="Peter Scheelke">
//      Copyright (c) Peter Scheelke. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ArtistAssistant.Command
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
        /// A command that removes a <see cref="DrawableObject"/>
        /// </summary>
        Remove,
        Select,
        Deselect,
        DeselectAll,
        Scale,
        Move,
        Duplicate
    }
}
