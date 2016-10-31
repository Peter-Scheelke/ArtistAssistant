//-----------------------------------------------------------------------
// <copyright file="DrawingMode.cs" company="Peter Scheelke">
//      Copyright (c) Peter Scheelke. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ArtistAssistant
{
    /// <summary>
    /// The various drawing modes a <see cref="ArtistAssistantForm"/> can
    /// have (such as adding new items, moving existing items, etc.)
    /// </summary>
    public enum DrawingMode
    {
        /// <summary>
        /// Add a new item
        /// </summary>
        Add,

        /// <summary>
        /// Move an existing item
        /// </summary>
        Move,

        /// <summary>
        /// Select an existing item
        /// </summary>
        Select,

        /// <summary>
        /// No mode selected
        /// </summary>
        None
    }
}
