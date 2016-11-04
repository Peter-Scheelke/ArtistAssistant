//-----------------------------------------------------------------------
// <copyright file="SelectCommand.cs" company="Peter Scheelke">
//      Copyright (c) Peter Scheelke. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ArtistAssistant.Command
{
    using DrawableObject;

    /// <summary>
    /// An <see cref="ICommand"/> that selects a <see cref="DrawableObject"/>
    /// from a list of <see cref="DrawableObject"/>s
    /// </summary>
    public class SelectCommand : ICommand
    {
        /// <summary>
        /// The <see cref="DrawableObject"/> being selected
        /// </summary>
        private DrawableObject selectedObject;

        /// <summary>
        /// The <see cref="DrawableObject"/> that was selected before the
        /// <see cref="SelectCommand"/> was executed
        /// </summary>
        private DrawableObject previouslySelectedObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectCommand"/> class
        /// </summary>
        /// <param name="drawableObjectList">
        /// The list of <see cref="DrawableObject"/>s whose selected <see cref="DrawableObject"/> 
        /// is changing
        /// </param>
        /// <param name="selectedObject">The <see cref="DrawableObject"/> being selected</param>
        public SelectCommand(DrawableObjectList drawableObjectList, DrawableObject selectedObject)
        {
            this.DrawableObjectList = drawableObjectList;
            this.selectedObject = selectedObject;
            this.previouslySelectedObject = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectCommand"/> class
        /// </summary>
        /// <param name="drawableObjectList">
        /// The list of <see cref="DrawableObject"/>s whose selected <see cref="DrawableObject"/> 
        /// is changing
        /// </param>
        /// <param name="objectId"> The Id of the <see cref="DrawableObject"/> being selected</param>
        public SelectCommand(DrawableObjectList drawableObjectList, int objectId)
        {
            this.DrawableObjectList = drawableObjectList;
            this.selectedObject = null;
            this.previouslySelectedObject = null;
            foreach (DrawableObject item in this.DrawableObjectList)
            {
                if (item.Id == objectId)
                {
                    this.selectedObject = item;
                    break;
                }
            }
        }

        /// <summary>
        /// Gets or sets the list of <see cref="DrawableObject"/>s whose selected <see cref="DrawableObject"/>
        /// is going to change
        /// </summary>
        public DrawableObjectList DrawableObjectList { get; set; }

        /// <summary>
        /// Creates a new instance of the <see cref="SelectCommand"/> class
        /// </summary>
        /// <param name="drawableObjectList">
        /// The list of <see cref="DrawableObject"/>s whose selected <see cref="DrawableObject"/>
        /// is changing</param>
        /// <param name="selectedObject">The <see cref="DrawableObject"/> being selected</param>
        /// <returns>A new instance of the <see cref="SelectCommand"/> class</returns>
        public static SelectCommand Create(DrawableObjectList drawableObjectList, DrawableObject selectedObject)
        {
            return new SelectCommand(drawableObjectList, selectedObject);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="SelectCommand"/> class
        /// </summary>
        /// <param name="drawableObjectList">
        /// The list of <see cref="DrawableObject"/>s whose selected <see cref="DrawableObject"/> 
        /// is changing
        /// </param>
        /// <param name="objectId"> The Id of the <see cref="DrawableObject"/> being selected</param>
        /// <returns>A new instance of the <see cref="SelectCommand"/> class</returns>
        public static SelectCommand Create(DrawableObjectList drawableObjectList, int objectId)
        {
            return new SelectCommand(drawableObjectList, objectId);
        }

        /// <summary>
        /// Executes the <see cref="SelectCommand"/>
        /// </summary>
        public void Execute()
        {
            foreach (DrawableObject item in this.DrawableObjectList)
            {
                if (item.Selected)
                {
                    this.previouslySelectedObject = item;
                    break;
                }
            }

            this.selectedObject.Select();
        }

        /// <summary>
        /// Undoes the <see cref="SelectCommand"/>
        /// </summary>
        public void Undo()
        {
            if (this.previouslySelectedObject != null)
            {
                this.previouslySelectedObject.Select();
            }
            else
            {
                this.selectedObject.Deselect();
            }
        }
    }
}