//-----------------------------------------------------------------------
// <copyright file="MoveCommand.cs" company="Peter Scheelke">
//      Copyright (c) Peter Scheelke. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ArtistAssistant.Command
{
    using System.Drawing;
    using DrawableObject;

    /// <summary>
    /// An <see cref="ICommand"/> that moves a <see cref="DrawableObject"/> from one
    /// location to another
    /// </summary>
    public class MoveCommand : ICommand
    {
        /// <summary>
        ///  The <see cref="DrawableObject"/> that will be moved
        /// </summary>
        private DrawableObject movedObject;

        /// <summary>
        /// The previous location of the <see cref="DrawableObject"/>
        /// that was moved
        /// </summary>
        private Point? previousLocation;

        /// <summary>
        /// The location to which the <see cref="DrawableObject"/> will be moved
        /// </summary>
        private Point newLocation;

        /// <summary>
        /// Initializes a new instance of the <see cref="MoveCommand"/> class
        /// </summary>
        /// <param name="drawableObjectList">
        /// A list of <see cref="DrawableObject"/>s, one of which will 
        /// be moved by the <see cref="MoveCommand"/></param>
        /// <param name="movedObject">The <see cref="DrawableObject"/> that will be moved</param>
        /// <param name="newLocation">The location to which the <see cref="DrawableObject"/> will move</param>
        public MoveCommand(DrawableObjectList drawableObjectList, DrawableObject movedObject, Point newLocation)
        {
            this.previousLocation = null;
            this.DrawableObjectList = drawableObjectList;
            this.movedObject = movedObject;
            this.newLocation = newLocation;
        }

        /// <summary>
        /// Gets or sets the list of <see cref="DrawableObject"/>s containing the <see cref="DrawableObject"/>
        /// that will be moved
        /// </summary>
        public DrawableObjectList DrawableObjectList { get; set; }

        /// <summary>
        /// Creates a new instance of the <see cref="MoveCommand"/> class
        /// </summary>
        /// <param name="drawableObjectList">
        /// A list of <see cref="DrawableObject"/>s, one of which will 
        /// be moved by the <see cref="MoveCommand"/></param>
        /// <param name="movedObject">The <see cref="DrawableObject"/> that will be moved</param>
        /// <param name="newLocation">The location to which the <see cref="DrawableObject"/> will move</param>
        /// <returns>A new instance of the <see cref="MoveCommand"/> class</returns>
        public static MoveCommand Create(DrawableObjectList drawableObjectList, DrawableObject movedObject, Point newLocation)
        {
            return new MoveCommand(drawableObjectList, movedObject, newLocation);
        }

        /// <summary>
        /// Executes the <see cref="MoveCommand"/>
        /// </summary>
        public void Execute()
        {
            this.previousLocation = this.movedObject.Location;
            this.movedObject.Location = this.newLocation;
        }

        /// <summary>
        /// Undoes the <see cref="MoveCommand"/>
        /// </summary>
        public void Undo()
        {
            if (this.previousLocation != null)
            {
                this.movedObject.Location = new Point((int)this.previousLocation?.X, (int)this.previousLocation?.Y);
            }
        }
    }
}
