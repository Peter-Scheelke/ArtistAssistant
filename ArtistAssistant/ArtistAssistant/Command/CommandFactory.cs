//-----------------------------------------------------------------------
// <copyright file="CommandFactory.cs" company="Peter Scheelke">
//      Copyright (c) Peter Scheelke. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ArtistAssistant.Command
{
    using System;
    using System.Drawing;
    using DrawableObject;

    /// <summary>
    /// Creates <see cref="ICommand"/> objects
    /// </summary>
    public class CommandFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandFactory"/> class
        /// </summary>
        public CommandFactory()
        {
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
        /// Creates a <see cref="CommandArguments"/> object
        /// </summary>
        /// <returns>A <see cref="CommandArguments"/> object</returns>
        public static CommandArguments GetCommandArgumentsObject()
        {
            return new CommandArguments();
        }

        /// <summary>
        /// Creates an <see cref="ICommand"/> object based on contents of the the given 
        /// <see cref="CommandArguments"/> object
        /// </summary>
        /// <param name="parameters">
        /// The parameters used to create the correct
        /// <see cref="ICommand"/> object
        /// </param>
        /// <returns>
        /// An <see cref="ICommand"/> object based on the contents 
        /// of the given <see cref="CommandArguments"/> object
        /// </returns>
        public ICommand CreateCommand(CommandArguments parameters)
        {
            if (this.CheckParameters(parameters) == false)
            {
                throw new Exception($"Error: Invalid parameters given for command type {parameters.CommandType}");
            }

            ICommand command;
            switch (parameters.CommandType)
            {
                case CommandType.Add:
                    command = AddCommand.Create(parameters.DrawableObjectList, parameters.AffectedDrawableObject);
                    break;
                case CommandType.BringToIndex:
                    command = BringToIndexCommand.Create(parameters.DrawableObjectList, (int)parameters.StartIndex, (int)parameters.TargetIndex);
                    break;
                case CommandType.Deselect:
                    command = DeselectCommand.Create(parameters.DrawableObjectList);
                    break;
                case CommandType.Duplicate:
                    command = DuplicateCommand.Create(parameters.DrawableObjectList, parameters.AffectedDrawableObject);
                    break;
                case CommandType.Move:
                    command = MoveCommand.Create(parameters.DrawableObjectList, parameters.AffectedDrawableObject, (Point)parameters.Location);
                    break;
                case CommandType.Remove:
                    DrawableObject removedObject = parameters.AffectedDrawableObject;
                    command = RemoveCommand.Create(parameters.DrawableObjectList, removedObject);
                    break;
                case CommandType.Scale:
                    command = ScaleCommand.Create(parameters.DrawableObjectList, parameters.AffectedDrawableObject, (Size)parameters.Size);
                    break;
                case CommandType.Select:
                    DrawableObject selectedObject = parameters.AffectedDrawableObject;
                    command = SelectCommand.Create(parameters.DrawableObjectList, selectedObject);
                    break;
                default:
                    throw new Exception($"Error: CommandType {parameters.CommandType} is invalid.");
            }

            return command;
        }

        /// <summary>
        /// Makes sure that the given <see cref="CommandArguments"/> object contains a valid 
        /// set of parameters to create an <see cref="ICommand"/> object
        /// </summary>
        /// <param name="parameters">The set of parameters being checked</param>
        /// <returns>Whether the given <see cref="CommandArguments"/> object is valid</returns>
        private bool CheckParameters(CommandArguments parameters)
        {
            if (parameters.DrawableObjectList == null)
            {
                return false;
            }

            // AddCommands each need to have a DrawableObject
            if (parameters.CommandType == CommandType.Add)
            {
                return parameters.AffectedDrawableObject != null;
            }

            // BringToIndexCommands each need to have a start index and a target index
            if (parameters.CommandType == CommandType.BringToIndex)
            {
                return parameters.StartIndex != null && parameters.TargetIndex != null;
            }

            // Each DeselectCommand just needs a list of DrawableObjects (which was already checked)
            if (parameters.CommandType == CommandType.Deselect)
            {
                return true;
            }

            // DuplicateCommands each need to have a DrawableObject to duplicate
            if (parameters.CommandType == CommandType.Duplicate)
            {
                return parameters.AffectedDrawableObject != null;
            }

            // MoveCommands each need to have a DrawableObject to move and
            // a location to move it to
            if (parameters.CommandType == CommandType.Move)
            {
                return parameters.AffectedDrawableObject != null && parameters.Location != null;
            }

            // RemoveCommands each need to have a DrawableObject to remove
            if (parameters.CommandType == CommandType.Remove)
            {
                return parameters.AffectedDrawableObject != null;
            }

            // ScaleCommands each need to have a Size to scale and a DrawableObject to scale
            if (parameters.CommandType == CommandType.Scale)
            {
                return parameters.Size != null && parameters.AffectedDrawableObject != null;
            }

            // SelectCommands each need to have a DrawableObject to select
            if (parameters.CommandType == CommandType.Select)
            {
                return parameters.AffectedDrawableObject != null;
            }

            return false;
        }

        /// <summary>
        /// A <see cref="CommandArguments"/> object is used to pass parameters to a <see cref="CommandFactory"/>
        /// </summary>
        public class CommandArguments
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
        }
    }
}
