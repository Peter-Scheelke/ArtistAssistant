//-----------------------------------------------------------------------
// <copyright file="BackendWrapper.cs" company="Peter Scheelke">
//      Copyright (c) Peter Scheelke. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ArtistAssistant
{
    using System.Collections.Generic;
    using System.Drawing;
    using Command;
    using DrawableObject;

    /// <summary>
    /// A <see cref="BackendWrapper"/> wraps up a <see cref="Drawing"/>,
    /// list of <see cref="DrawableObject"/>s, etc in order to allow those objects
    /// to easily be used by the GUI
    /// </summary>
    public class BackendWrapper
    {
        /// <summary>
        /// A <see cref="Stack{T}"/> of <see cref="ICommand"/>
        /// objects used to easily undo changes
        /// </summary>
        private Stack<ICommand> undoStack;

        /// <summary>
        /// A list of <see cref="DrawableObject"/>s that are rendered
        /// in a <see cref="Drawing"/>
        /// </summary>
        private DrawableObjectList drawableObjectList;

        /// <summary>
        /// The <see cref="Drawing"/> that will be rendered to the GUI
        /// </summary>
        private Drawing drawing;

        /// <summary>
        /// A <see cref="CommandFactory"/> used to create the <see cref="ICommand"/>
        /// objects that are requested by the GUI
        /// </summary>
        private CommandFactory commandFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="BackendWrapper"/> class
        /// </summary>
        /// <param name="backgroundImage">The background image used in the <see cref="BackendWrapper"/>'s <see cref="Drawing"/></param>
        /// <param name="size">The <see cref="Size"/> the <see cref="BackendWrapper"/>'s <see cref="Drawing"/> should be</param>
        public BackendWrapper(Image backgroundImage, Size size)
        {
            this.drawableObjectList = DrawableObjectList.Create();
            this.drawing = Drawing.Create(backgroundImage, this.drawableObjectList, size);
            this.undoStack = new Stack<ICommand>();
            this.commandFactory = CommandFactory.Create();
        }

        /// <summary>
        /// Gets a graphical version of the <see cref="Drawing"/> object
        /// contained in the <see cref="Backe"/>
        /// </summary>
        public Image RenderedDrawing
        { 
            get
            {
                return this.drawing.RenderedDrawing;
            }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="BackendWrapper"/> class
        /// </summary>
        /// <param name="backgroundImage">The background image used in the <see cref="BackendWrapper"/>'s <see cref="Drawing"/></param>
        /// <param name="size">The <see cref="Size"/> the <see cref="BackendWrapper"/>'s <see cref="Drawing"/> should be</param>
        /// <returns>A new instance of the <see cref="BackendWrapper"/> class</returns>
        public static BackendWrapper Create(Image backgroundImage, Size size)
        {
            return new BackendWrapper(backgroundImage, size);
        }

        /// <summary>
        /// Adds a <see cref="DrawableObject.DrawableObject"/> to the <see cref="Drawing"/>
        /// contained in the <see cref="BackendWrapper"/>
        /// </summary>
        /// <param name="imageType">The type of image the <see cref="DrawableObject.DrawableObject"/> should have</param>
        /// <param name="location">The location of the <see cref="DrawableObject.DrawableObject"/></param>
        /// <param name="size">The size of the <see cref="DrawableObject.DrawableObject"/></param>
        public void Add(ImageType imageType, Point location, Size size)
        {
            DrawableObject.DrawableObject drawableObject = DrawableObject.DrawableObject.Create(imageType, location, size);
            var parameters = CommandFactory.GetCommandArgumentsObject();
            parameters.CommandType = CommandType.Add;
            parameters.DrawableObjectList = this.drawableObjectList;
            parameters.AffectedDrawableObject = drawableObject;
            this.ExecuteCommand(parameters);
        }

        /// <summary>
        /// Brings the currently selected <see cref="DrawableObject.DrawableObject"/>
        /// to the front of the <see cref="Drawing"/>
        /// </summary>
        public void BrintToFront()
        {
            var parameters = CommandFactory.GetCommandArgumentsObject();
            parameters.CommandType = CommandType.Deselect;
            parameters.DrawableObjectList = this.drawableObjectList;
            int startIndex = -1;

            for (int i = 0; i < this.drawableObjectList.Count; ++i)
            {
                if (this.drawableObjectList.RenderOrder[i].Selected)
                {
                    startIndex = i;
                    break;
                }
            }

            // If this isn't true, nothing was selected
            if (startIndex >= 0)
            {
                parameters.StartIndex = startIndex;
                parameters.TargetIndex = this.drawableObjectList.Count - 1;
                this.ExecuteCommand(parameters);
            }
        }

        /// <summary>
        /// Sends the currently selected <see cref="DrawableObject.DrawableObject"/>
        /// to the back of the <see cref="Drawing"/>
        /// </summary>
        public void SendToBack()
        {
            var parameters = CommandFactory.GetCommandArgumentsObject();
            parameters.CommandType = CommandType.Deselect;
            parameters.DrawableObjectList = this.drawableObjectList;
            int startIndex = -1;

            for (int i = 0; i < this.drawableObjectList.Count; ++i)
            {
                if (this.drawableObjectList.RenderOrder[i].Selected)
                {
                    startIndex = i;
                    break;
                }
            }

            // If this isn't true, nothing was selected
            if (startIndex >= 0)
            {
                parameters.StartIndex = startIndex;
                parameters.TargetIndex = 0;
                this.ExecuteCommand(parameters);
            }
        }

        /// <summary>
        /// Deselect whatever <see cref="DrawableObject.DrawableObject"/>
        /// is currently selected
        /// </summary>
        public void Deselect()
        {
            var parameters = CommandFactory.GetCommandArgumentsObject();
            parameters.CommandType = CommandType.Deselect;
            parameters.DrawableObjectList = this.drawableObjectList;

            /* TODO: Finish this */

            this.ExecuteCommand(parameters);
        }

        /// <summary>
        /// Duplicates the currently selected <see cref="DrawableObject.DrawableObject"/>
        /// with a small offset in location
        /// </summary>
        public void Duplicate()
        {
            var parameters = CommandFactory.GetCommandArgumentsObject();
            parameters.CommandType = CommandType.Duplicate;
            parameters.DrawableObjectList = this.drawableObjectList;

            /* TODO: Finish this */

            this.ExecuteCommand(parameters);
        }

        /// <summary>
        /// Moves the currently selected <see cref="DrawableObject.DrawableObject"/>
        /// to the given location
        /// </summary>
        public void Move(Point location)
        {
            var parameters = CommandFactory.GetCommandArgumentsObject();
            parameters.CommandType = CommandType.Move;
            parameters.DrawableObjectList = this.drawableObjectList;

            /* TODO: Finish this */

            this.ExecuteCommand(parameters);
        }

        /// <summary>
        /// Removes the currently selected <see cref="DrawableObject.DrawableObject"/>
        /// from the <see cref="Drawing"/>
        /// </summary>
        public void Remove()
        {
            var parameters = CommandFactory.GetCommandArgumentsObject();
            parameters.CommandType = CommandType.Remove;
            parameters.DrawableObjectList = this.drawableObjectList;

            /* TODO: Finish this */

            this.ExecuteCommand(parameters);
        }

        /// <summary>
        /// Scales the currently selected <see cref="DrawableObject.DrawableObject"/>
        /// to the new <see cref="Size"/>
        /// </summary>
        /// <param name="size">The new <see cref="Size"/> of the currently selected <see cref="DrawableObject.DrawableObject"/></param>
        public void Scale(Size size)
        {
            var parameters = CommandFactory.GetCommandArgumentsObject();
            parameters.CommandType = CommandType.Scale;
            parameters.DrawableObjectList = this.drawableObjectList;

            /* TODO: Finish this */

            this.ExecuteCommand(parameters);
        }

        /// <summary>
        /// Selects the topmost <see cref="DrawableObject.DrawableObject"/> at the given location
        /// in the drawing
        /// </summary>
        /// <param name="location">The location of the <see cref="DrawableObject.DrawableObject"/> to be selected</param>
        public void Select(Point location)
        {
            var parameters = CommandFactory.GetCommandArgumentsObject();
            parameters.CommandType = CommandType.Select;
            parameters.DrawableObjectList = this.drawableObjectList;
            
            /* TODO: Finish this */

            this.ExecuteCommand(parameters);
        }

        /// <summary>
        /// Undoes the last <see cref="ICommand"/> to be added to the undo stack
        /// </summary>
        public void Undo()
        {
            ICommand commandToUndo = this.undoStack.Pop();
            commandToUndo.Undo();
        }

        // Other things to create:
        // Way to resize drawing?
        // Way to get size of selected item

        /// <summary>
        /// Creates and executes an <see cref="ICommand"/> based on the given
        /// <see cref="CommandFactory.CommandArguments"/> object. The command is
        /// then added to the undo stack
        /// </summary>
        /// <param name="parameters">The parameters used to create the <see cref="ICommand"/> object</param>
        private void ExecuteCommand(CommandFactory.CommandArguments parameters)
        {
            ICommand command = this.commandFactory.CreateCommand(parameters);
            command.Execute();
            this.undoStack.Push(command);
        }
    }
}
