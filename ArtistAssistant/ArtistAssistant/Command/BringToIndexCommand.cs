//-----------------------------------------------------------------------
// <copyright file="BringToIndexCommand.cs" company="Peter Scheelke">
//      Copyright (c) Peter Scheelke. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ArtistAssistant.Command
{
    using DrawableObject;

    /// <summary>
    /// An <see cref="ICommand"/> that moves a <see cref="DrawableObject"/>'s
    /// index in a list of <see cref="DrawableObject"/>s to a different index
    /// </summary>
    public class BringToIndexCommand : ICommand
    {
        /// <summary>
        /// The original index in the list of <see cref="DrawableObject"/>s
        /// </summary>
        private int startIndex;

        /// <summary>
        /// The target index in the list of <see cref="DrawableObject"/>s
        /// </summary>
        private int targetIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="BringToIndexCommand"/> class
        /// </summary>
        /// <param name="drawableObjectList">The list of <see cref="DrawableObject"/>s whose render order is changing</param>
        /// <param name="startIndex">The start index of the <see cref="DrawableObject"/> whose index in the render order is changing</param>
        /// <param name="targetIndex">The final index that the <see cref="DrawableObject"/> should have</param>
        public BringToIndexCommand(DrawableObjectList drawableObjectList, int startIndex, int targetIndex)
        {
            this.DrawableObjectList = drawableObjectList;
            this.startIndex = startIndex;
            this.targetIndex = targetIndex;
        }

        /// <summary>
        /// Gets or sets the list of <see cref="DrawableObject"/>s whose render order will be changed
        /// </summary>
        public DrawableObjectList DrawableObjectList { get; set; }

        /// <summary>
        /// Creates a new instance of the <see cref="BringToIndexCommand"/> class
        /// </summary>
        /// <param name="drawableObjectList">The list of <see cref="DrawableObject"/>s whose render order is changing</param>
        /// <param name="startIndex">The start index of the <see cref="DrawableObject"/> whose index in the render order is changing</param>
        /// <param name="targetIndex">The final index that the <see cref="DrawableObject"/> should have</param>
        /// <returns>A new instance of the <see cref="BringToIndexCommand"/> class</returns>
        public static BringToIndexCommand Create(DrawableObjectList drawableObjectList, int startIndex, int targetIndex)
        {
            return new BringToIndexCommand(drawableObjectList, startIndex, targetIndex);
        }

        /// <summary>
        /// Executes the <see cref="BringToIndexCommand"/>
        /// </summary>
        public void Execute()
        {
            this.DrawableObjectList.BringToIndex(this.startIndex, this.targetIndex);
        }

        /// <summary>
        /// Undoes the <see cref="BringToIndexCommand"/>
        /// </summary>
        public void Undo()
        {
            this.DrawableObjectList.BringToIndex(this.targetIndex, this.startIndex);
        }
    }
}
