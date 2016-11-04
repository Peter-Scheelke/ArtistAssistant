//-----------------------------------------------------------------------
// <copyright file="RemoveCommand.cs" company="Peter Scheelke">
//      Copyright (c) Peter Scheelke. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ArtistAssistant.Command
{
    using DrawableObject;

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
        /// Tracks whether the <see cref="DrawableObject"/> being removed was selected at the time it was removed
        /// </summary>
        private bool wasSelected;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveCommand"/> class
        /// </summary>
        /// <param name="drawableObjectList">
        /// The list of <see cref="DrawableObject"/>s from which a 
        /// <see cref="DrawableObject"/> is being removed
        /// </param>
        /// <param name="removedObject">The <see cref="DrawableObject"/> being removed</param>
        public RemoveCommand(DrawableObjectList drawableObjectList, DrawableObject removedObject)
        {
            this.DrawableObjectList = drawableObjectList;
            this.removedObject = removedObject;
            this.originalIndex = this.DrawableObjectList.RenderOrder.IndexOf(this.removedObject);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveCommand"/> class
        /// </summary>
        /// <param name="drawableObjectList">
        /// The list of <see cref="DrawableObject"/>s from which a 
        /// <see cref="DrawableObject"/> is being removed
        /// </param>
        /// <param name="objectId">The Id of the <see cref="DrawableObject"/> being removed</param>
        public RemoveCommand(DrawableObjectList drawableObjectList, int objectId)
        {
            this.originalIndex = -1;
            this.DrawableObjectList = drawableObjectList;
            this.removedObject = null;
            for (int i = 0; i < this.DrawableObjectList.RenderOrder.Count; ++i)
            {
                if (this.DrawableObjectList.RenderOrder[i].Id == objectId)
                {
                    this.removedObject = this.DrawableObjectList.RenderOrder[i];
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
        /// <param name="drawableObjectlist">
        /// The list of <see cref="DrawableObject"/>s from which a 
        /// <see cref="DrawableObject"/> is being removed
        /// </param>
        /// <param name="removedObject">The <see cref="DrawableObject"/> being removed</param>
        /// <returns>A new instance of the <see cref="RemoveCommand"/> class</returns>
        public static RemoveCommand Create(DrawableObjectList drawableObjectlist, DrawableObject removedObject)
        {
            return new RemoveCommand(drawableObjectlist, removedObject);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="RemoveCommand"/> class
        /// </summary>
        /// <param name="drawableObjectList">
        /// The list of <see cref="DrawableObject"/>s from which a 
        /// <see cref="DrawableObject"/> is being removed
        /// </param>
        /// <param name="objectId">The Id of the <see cref="DrawableObject"/> being removed</param>
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
            if (this.removedObject != null && this.DrawableObjectList.Contains(this.removedObject))
            {
                this.wasSelected = this.removedObject.Selected;
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
                if (this.wasSelected)
                {
                    this.removedObject.Select();
                }
            }
        }
    }
}
