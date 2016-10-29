//-----------------------------------------------------------------------
// <copyright file="RemoveCommand.cs" company="Peter Scheelke">
//      Copyright (c) Peter Scheelke. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ArtistAssistant.Command
{
    using ArtistAssistant.DrawableObject;

    /// <summary>
    /// A <see cref="ICommand"/> that removes a <see cref="DrawableObject"/>
    /// from the list of <see cref="DrawableObject"/>s
    /// </summary>
    public class RemoveCommand : ICommand
    {
        /// <summary>
        /// The <see cref="DrawableObject"/> being
        /// removed from the list of <see cref="DrawableObject"/>s
        /// </summary>
        private DrawableObject removedObject;

        /// <summary>
        /// The index of the removed <see cref="DrawableObject"/> in the list of <see cref="DrawableObject"/>s'
        /// render order
        /// </summary>
        private int originalIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveCommand"/> class
        /// </summary>
        /// <param name="drawableObjectList">
        /// The list of <see cref="DrawableObject"/>s from which a 
        /// <see cref="DrawableObject"/> is being removed
        /// </param>
        /// <param name="objectId">The Id of the object being removed</param>
        public RemoveCommand(DrawableObjectList drawableObjectList, int objectId)
        {
            this.originalIndex = -1;
            this.DrawableObjectList = drawableObjectList;
            for (int i = 0; i < this.DrawableObjectList.Count; ++i)
            {
                if (this.DrawableObjectList[i].Id == objectId)
                {
                    this.removedObject = this.DrawableObjectList[i];
                    this.originalIndex = i;
                    break;
                }
            }
        }

        /// <summary>
        /// Gets or sets the the list of <see cref="DrawableObject"/>s from which 
        /// <see cref="DrawableObject"/> will be removed
        /// </summary>
        public DrawableObjectList DrawableObjectList { get; set; }

        /// <summary>
        /// Creates a new instance of the <see cref="RemoveCommand"/> class
        /// </summary>
        /// <param name="drawableObjectList">
        /// The list of <see cref="DrawableObject"/>s from which a 
        /// <see cref="DrawableObject"/> is being removed
        /// </param>
        /// <param name="objectId">The Id of the object being removed</param>
        /// <returns>A new instance of the <see cref="RemoveCommand"/> class</returns>
        public static RemoveCommand Create(DrawableObjectList drawableObjectList, int objectId)
        {
            return new RemoveCommand(drawableObjectList, objectId);
        }

        /// <summary>
        /// Executes the <see cref="RemoveCommand"/>
        /// </summary>
        public void Execute()
        {
            if (this.removedObject != null)
            {
                this.DrawableObjectList.Remove(this.removedObject);
            }
        }

        /// <summary>
        /// Undoes the <see cref="RemoveCommand"/>
        /// </summary>
        public void Undo()
        {
            if (this.removedObject != null)
            {
                this.DrawableObjectList.Add(this.removedObject);
                this.DrawableObjectList.BringToIndex(this.DrawableObjectList.Count - 1, this.originalIndex);
            }
        }
    }
}
