//-----------------------------------------------------------------------
// <copyright file="ScaleCommand.cs" company="Peter Scheelke">
//      Copyright (c) Peter Scheelke. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ArtistAssistant.Command.Commands
{
    using System.Drawing;
    using DrawableObject;

    /// <summary>
    /// An <see cref="ICommand"/> that changes the size of a <see cref="DrawableObject"/>
    /// </summary>
    public class ScaleCommand : ICommand
    {
        /// <summary>
        /// The <see cref="DrawableObject"/> that is being scaled
        /// </summary>
        private DrawableObject scaledObject;

        /// <summary>
        /// The size of the <see cref="DrawableObject"/> before being scaled
        /// </summary>
        private Size? previousSize;

        /// <summary>
        /// The new size of the <see cref="DrawableObject"/>
        /// </summary>
        private Size newSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScaleCommand"/> class
        /// </summary>
        /// <param name="drawableObjectList">
        /// The list of <see cref="DrawableObject"/>s that contains the 
        /// one being scaled
        /// </param>
        /// <param name="scaledObject">The <see cref="DrawableObject"/> being scaled</param>
        /// <param name="newSize">The new size of the <see cref="DrawableObject"/></param>
        public ScaleCommand(DrawableObjectList drawableObjectList, DrawableObject scaledObject, Size newSize)
        {
            this.DrawableObjectList = drawableObjectList;
            this.scaledObject = scaledObject;
            this.newSize = newSize;
            this.previousSize = null;
        }

        /// <summary>
        /// Gets or sets the list of <see cref="DrawableObject"/>s that contains the
        /// <see cref="DrawableObject"/> whose size is changing
        /// </summary>
        public DrawableObjectList DrawableObjectList { get; set; }

        /// <summary>
        /// Creates a new instance of the <see cref="ScaleCommand"/> class
        /// </summary>
        /// <param name="drawableObjectList">
        /// The list of <see cref="DrawableObject"/>s that contains the 
        /// one being scaled
        /// </param>
        /// <param name="scaledObject">The <see cref="DrawableObject"/> being scaled</param>
        /// <param name="newSize">The new size of the <see cref="DrawableObject"/></param>
        /// <returns>A new instance of the <see cref="ScaleCommand"/> class</returns>
        public static ScaleCommand Create(DrawableObjectList drawableObjectList, DrawableObject scaledObject, Size newSize)
        {
            return new ScaleCommand(drawableObjectList, scaledObject, newSize);
        }

        /// <summary>
        /// Executes the <see cref="ScaleCommand"/>
        /// </summary>
        public void Execute()
        {
            this.previousSize = this.scaledObject.Size;
            this.scaledObject.Size = this.newSize;
        }

        /// <summary>
        /// Undoes the <see cref="ScaleCommand"/>
        /// </summary>
        public void Undo()
        {
            if (this.previousSize != null)
            {
                this.scaledObject.Size = (Size)this.previousSize;
            }
        }
    }
}
