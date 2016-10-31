//-----------------------------------------------------------------------
// <copyright file="Serializer.cs" company="Peter Scheelke">
//      Copyright (c) Peter Scheelke. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ArtistAssistant.Serializer
{
    using System;
    using System.Collections.Generic;
    using DrawableObject;
    using Newtonsoft.Json;

    /// <summary>
    /// Serializes <see cref="DrawableObject"/>s into JSON objects
    /// </summary>
    class DrawableObjectSerializer
    {
        public static void Serialize(DrawableObjectList drawableObjectList)
        {
            List<SerializableDrawableObject> objectsToSerialize = new List<SerializableDrawableObject>();
            foreach (DrawableObject item in drawableObjectList.RenderOrder)
            {
                SerializableDrawableObject obj = new SerializableDrawableObject();
                obj.Selected = item.Selected;
                obj.XLocation = item.Location.X;
                obj.YLocation = item.Location.Y;
                obj.Width = item.Size.Width;
                obj.Height = item.Size.Height;
                obj.ImageType = item.ImageType.ToString();
                objectsToSerialize.Add(obj);
            }

            JsonConvert.SerializeObject(objectsToSerialize);
        }

        /// <summary>
        /// Simplified versions of the <see cref="DrawableObject"/> class
        /// that can easily be converted to/from JSON objects
        /// </summary>
        private class SerializableDrawableObject
        {
            /// <summary>
            /// Whether the <see cref="DrawableObject"/> was selected
            /// </summary>
            public bool Selected { get; set; }

            /// <summary>
            /// The X location of the <see cref="DrawableObject"/>
            /// </summary>
            public int XLocation { get; set; }

            /// <summary>
            /// The Y location of the <see cref="DrawableObject"/>
            /// </summary>
            public int YLocation { get; set; }

            /// <summary>
            /// The width of the <see cref="DrawableObject"/>
            /// </summary>
            public int Width { get; set; }

            /// <summary>
            /// The height of the <see cref="DrawableObject"/>
            /// </summary>
            public int Height { get; set; }

            /// <summary>
            /// The image type of the <see cref="DrawableObject"/>
            /// </summary>
            public string ImageType { get; set; }
        }
    }
}
