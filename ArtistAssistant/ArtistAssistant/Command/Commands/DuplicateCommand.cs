//-----------------------------------------------------------------------
// <copyright file="DuplicateCommand.cs" company="Peter Scheelke">
//      Copyright (c) Peter Scheelke. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ArtistAssistant.Command.Commands
{
    using DrawableObject;

    /// <summary>
    /// An <see cref="ICommand"/> that duplicates a <see cref="DrawableObject"/>
    /// and adds it to a list of <see cref="DrawableObject"/>s
    /// </summary>
    public class DuplicateCommand : ICommand
    {
        /// <summary>
        /// The duplicate of the <see cref="DrawableObject"/>
        /// </summary>
        private DrawableObject duplicate;

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateCommand"/> class
        /// </summary>
        /// <param name="drawableObjectlist">
        /// The list of <see cref="DrawableObject"/>s to which the 
        /// duplicate will be added
        /// </param>
        /// <param name="toBeDuplicated">The <see cref="DrawableObject"/> that will be duplicated</param>
        public DuplicateCommand(DrawableObjectList drawableObjectlist, DrawableObject toBeDuplicated)
        {
            this.DrawableObjectList = drawableObjectlist;
            this.duplicate = DrawableObject.Create(toBeDuplicated.ImageType, toBeDuplicated.Location, toBeDuplicated.Size);
        }

        /// <summary>
        /// Gets or sets the list of <see cref="DrawableObject"/>s to which the
        /// <see cref="DuplicateCommand"/> will add a duplicate of a <see cref="DrawableObject"/>
        /// </summary>
        public DrawableObjectList DrawableObjectList { get; set; }

        /// <summary>
        /// Creates a new instance of the <see cref="DuplicateCommand"/> class
        /// </summary>
        /// <param name="drawableObjectlist">
        /// The list of <see cref="DrawableObject"/>s to which the 
        /// duplicate will be added
        /// </param>
        /// <param name="toBeDuplicated">The <see cref="DrawableObject"/> that will be duplicated</param>
        /// <returns>A new instance of the <see cref="DuplicateCommand"/> class</returns>
        public static DuplicateCommand Create(DrawableObjectList drawableObjectlist, DrawableObject toBeDuplicated)
        {
            return new DuplicateCommand(drawableObjectlist, toBeDuplicated);
        }

        /// <summary>
        /// Executes the <see cref="DuplicateCommand"/>
        /// </summary>
        public void Execute()
        {
            this.DrawableObjectList.Add(this.duplicate);
        }

        /// <summary>
        /// Undoes the <see cref="DuplicateCommand"/>
        /// </summary>
        public void Undo()
        {
            if (this.DrawableObjectList.Contains(this.duplicate))
            {
                this.DrawableObjectList.Remove(this.duplicate);
            }
        }
    }
}
