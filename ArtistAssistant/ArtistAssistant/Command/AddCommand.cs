//-----------------------------------------------------------------------
// <copyright file="AddCommand.cs" company="Peter Scheelke">
//      Copyright (c) Peter Scheelke. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ArtistAssistant.Command
{
    using System.Drawing;
    using DrawableObject;

    /// <summary>
    /// A <see cref="ICommand"/> object that adds a <see cref="DrawableObject"/>
    /// to the list of <see cref="DrawableObject"/>s
    /// </summary>
    public class AddCommand : ICommand
    {
        /// <summary>
        /// The <see cref="DrawableObject"/> that will be added to the
        /// list of <see cref="DrawableObject"/>s
        /// </summary>
        private DrawableObject addedObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddCommand"/> class
        /// </summary>
        /// <param name="drawableObjectList">
        /// The list of <see cref="DrawableObject"/>s to which the 
        /// <see cref="DrawableObject"/> will be added
        /// </param>
        /// <param name="imageType">The <see cref="ImageType"/> of the <see cref="DrawableObject"/></param>
        /// <param name="location">The location of the <see cref="DrawableObject"/></param>
        /// <param name="size">The <see cref="Size"/> of the <see cref="DrawableObject"/></param>
        public AddCommand(DrawableObjectList drawableObjectList, ImageType imageType, Point location, Size size)
        {
            this.DrawableObjectList = drawableObjectList;
            this.addedObject = DrawableObject.Create(imageType, location, size);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AddCommand"/> class
        /// </summary>
        /// <param name="drawableObjectList">The list of <see cref="DrawableObject"/>s to which a new one is being added</param>
        /// <param name="addedObject">The <see cref="DrawableObject"/> being added to the list of <see cref="DrawableObject"/>s</param>
        public AddCommand(DrawableObjectList drawableObjectList, DrawableObject addedObject)
        {
            this.DrawableObjectList = drawableObjectList;
            this.addedObject = addedObject;
        }

        /// <summary>
        /// Gets or sets the list of <see cref="DrawableObject"/>s to which
        /// the <see cref="DrawableObject.DrawableObject"/> should be added
        /// </summary>
        public DrawableObjectList DrawableObjectList { get; set; }

        /// <summary>
        /// Creates a new instance of the <see cref="AddCommand"/> class
        /// </summary>
        /// <param name="drawableObjectList">
        /// The list of <see cref="DrawableObject"/>s to which the 
        /// <see cref="DrawableObject"/> will be added
        /// </param>
        /// <param name="imageType">The <see cref="ImageType"/> of the <see cref="DrawableObject"/></param>
        /// <param name="location">The location of the <see cref="DrawableObject"/></param>
        /// <param name="size">The <see cref="Size"/> of the <see cref="DrawableObject"/></param>
        /// <returns>A new instance of the <see cref="AddCommand"/> class</returns>
        public static AddCommand Create(DrawableObjectList drawableObjectList, ImageType imageType, Point location, Size size)
        {
            return new AddCommand(drawableObjectList, imageType, location, size);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="AddCommand"/> class
        /// </summary>
        /// <param name="drawableObjectList">The list of <see cref="DrawableObject"/>s to which a new one is being added</param>
        /// <param name="addedObject">The <see cref="DrawableObject"/> being added to the list of <see cref="DrawableObject"/>s</param>
        /// <returns>A new instance of the <see cref="AddCommand"/> class</returns>
        public static AddCommand Create(DrawableObjectList drawableObjectList, DrawableObject addedObject)
        {
            return new AddCommand(drawableObjectList, addedObject);
        }

        /// <summary>
        /// Executes the <see cref="AddCommand"/> by adding the <see cref="DrawableObject"/>
        /// to the list of <see cref="DrawableObject"/>s
        /// </summary>
        public void Execute()
        {
            this.DrawableObjectList.Add(this.addedObject);
        }

        /// <summary>
        /// Undoes the <see cref="AddCommand"/> by removing the <see cref="DrawableObject"/>
        /// from the list of <see cref="DrawableObject"/>s
        /// </summary>
        public void Undo()
        {
            this.DrawableObjectList.Remove(this.addedObject);
        }
    }
}
