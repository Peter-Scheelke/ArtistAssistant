//-----------------------------------------------------------------------
// <copyright file="CommandParameters.cs" company="Peter Scheelke">
//      Copyright (c) Peter Scheelke. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ArtistAssistant.Command.Factory
{
    using System.Collections.Generic;
    using System.Drawing;
    using Commands;
    using DrawableObject;

    /// <summary>
    /// A <see cref="CommandParameters"/> object is used to pass parameters to a <see cref="CommandFactory"/>
    /// </summary>
    public class CommandParameters
    {
        /// <summary>
        /// Gets or sets the type of <see cref="ICommand"/> the <see cref="CommandFactory"/> should create
        /// </summary>
        public CommandType CommandType { get; set; }

        /// <summary>
        /// Gets or sets the list of <see cref="AffectedDrawableObject"/>s that the <see cref="CommandFactory"/>
        /// should create the <see cref="ICommand"/> for
        /// </summary>
        public DrawableObjectList DrawableObjectList { get; set; } = null;

        /// <summary>
        /// Gets or sets the <see cref="DrawableObject"/> that will be affected by the <see cref="ICommand"/>
        /// created by the <see cref="CommandFactory"/>
        /// </summary>
        public DrawableObject AffectedDrawableObject { get; set; } = null;

        /// <summary>
        /// Gets or sets a <see cref="Point"/> that the <see cref="CommandFactory"/> should use to create
        /// the <see cref="ICommand"/>
        /// </summary>
        public Point? Location { get; set; } = null;

        /// <summary>
        /// Gets or sets a <see cref="Size"/> that the <see cref="CommandFactory"/> should use to create the
        /// <see cref="ICommand"/>
        /// </summary>
        public Size? Size { get; set; } = null;

        /// <summary>
        /// Gets or sets an integer that is used to create an <see cref="ICommand"/> object
        /// that changes the render order of the list of <see cref="DrawableObject"/>s
        /// </summary>
        public int? StartIndex { get; set; } = null;

        /// <summary>
        /// Gets or sets an integer that is used to create an <see cref="ICommand"/> object
        /// that changes the render order of the list of <see cref="DrawableObject"/>s
        /// </summary>
        public int? TargetIndex { get; set; } = null;

        /// <summary>
        /// Gets or sets a list of <see cref="ICommand"/>s used to create <see cref="MacroCommand"/>s
        /// </summary>
        public List<ICommand> Commands { get; set; } = null;

        /// <summary>
        /// Creates a <see cref="CommandParameters"/> object
        /// </summary>
        /// <returns>A <see cref="CommandParameters"/> object</returns>
        public static CommandParameters Create()
        {
            return new CommandParameters();
        }
    }
}