//-----------------------------------------------------------------------
// <copyright file="DeselectCommand.cs" company="Peter Scheelke">
//      Copyright (c) Peter Scheelke. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ArtistAssistant.Command.Commands
{
    using DrawableObject;

    /// <summary>
    /// An <see cref="ICommand"/> that deselects the currently
    /// selected <see cref="DrawableObject"/>
    /// </summary>
    public class DeselectCommand : ICommand
    {
        /// <summary>
        /// The <see cref="DrawableObject"/> that was deselected
        /// </summary>
        private DrawableObject deselectedObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeselectCommand"/> class
        /// </summary>
        /// <param name="drawableObjectList">
        /// The list of <see cref="DrawableObject"/>s whose selected
        /// <see cref="DrawableObject"/> will be deselected</param>
        public DeselectCommand(DrawableObjectList drawableObjectList)
        {
            this.DrawableObjectList = drawableObjectList;
            this.deselectedObject = null;
        }

        /// <summary>
        /// Gets or sets the list of <see cref="DrawableObject"/>s whose selected
        /// <see cref="DrawableObject"/> will be deselected
        /// </summary>
        public DrawableObjectList DrawableObjectList { get; set; }

        /// <summary>
        /// Creates a new instance of the <see cref="DeselectCommand"/> class
        /// </summary>
        /// <param name="drawableObjectList">
        /// The list of <see cref="DrawableObject"/>s whose selected
        /// <see cref="DrawableObject"/> will be deselected</param>
        /// <returns>A new instance of the <see cref="DeselectCommand"/> class</returns>
        public static DeselectCommand Create(DrawableObjectList drawableObjectList)
        {
            return new DeselectCommand(drawableObjectList);
        }

        /// <summary>
        /// Executes the <see cref="DeselectCommand"/>
        /// </summary>
        public void Execute()
        {
            foreach (DrawableObject item in this.DrawableObjectList)
            {
                if (item.Selected)
                {
                    this.deselectedObject = item;
                    break;
                }
            }

            if (this.deselectedObject != null)
            {
                this.deselectedObject.Deselect();
            }
        }

        /// <summary>
        /// Undoes the <see cref="DeselectCommand"/>
        /// </summary>
        public void Undo()
        {
            if (this.deselectedObject != null)
            {
                this.deselectedObject.Select();
            }
        }
    }
}
