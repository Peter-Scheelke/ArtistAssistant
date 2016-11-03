//-----------------------------------------------------------------------
// <copyright file="DrawableObjectSerializer.cs" company="Peter Scheelke">
//      Copyright (c) Peter Scheelke. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ArtistAssistant.Serializer
{
    using System.Collections.Generic;
    using System.Drawing;
    using DrawableObject;
    using Newtonsoft.Json;

    /// <summary>
    /// Serializes <see cref="DrawableObject"/>s into JSON objects
    /// </summary>
    public class DrawableObjectSerializer
    {
        /// <summary>
        /// Serializes a <see cref="DrawableObjectList"/> into a JSON string
        /// </summary>
        /// <param name="drawableObjectList">The <see cref="DrawableObjectList"/> being serialized</param>
        /// <returns>A JSON serialization of the given <see cref="DrawableObjectList"/></returns>
        public static string Serialize(DrawableObjectList drawableObjectList)
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
                obj.ImageType = item.ImageType;
                objectsToSerialize.Add(obj);
            }

            return JsonConvert.SerializeObject(objectsToSerialize);
        }

        /// <summary>
        /// Deserializes a JSON string into a <see cref="DrawableObjectList"/>
        /// </summary>
        /// <param name="jsonString">The JSON string being deserialized</param>
        /// <returns>A <see cref="DrawableObjectList"/> from the deserialized string</returns>
        public static DrawableObjectList Deserialize(string jsonString)
        {
            List<SerializableDrawableObject> deserializedList = JsonConvert.DeserializeObject<List<SerializableDrawableObject>>(jsonString.ToString());
            DrawableObjectList list = DrawableObjectList.Create();

            foreach (var item in deserializedList)
            {
                Point location = new Point(item.XLocation, item.YLocation);
                Size size = new Size(item.Width, item.Height);
                DrawableObject drawableObject = DrawableObject.Create(item.ImageType, location, size);
                list.Add(drawableObject);
                if (item.Selected)
                {
                    drawableObject.Select();
                }
            }

            return list;
        }

        /// <summary>
        /// Simplified versions of the <see cref="DrawableObject"/> class
        /// that can easily be converted to/from JSON objects
        /// </summary>
        private class SerializableDrawableObject
        {
            /// <summary>
            /// Gets or sets a value indicating whether the <see cref="DrawableObject"/> was selected
            /// </summary>
            public bool Selected { get; set; }

            /// <summary>
            /// Gets or sets the X location of the <see cref="DrawableObject"/>
            /// </summary>
            public int XLocation { get; set; }

            /// <summary>
            /// Gets or sets the Y location of the <see cref="DrawableObject"/>
            /// </summary>
            public int YLocation { get; set; }

            /// <summary>
            /// Gets or sets the width of the <see cref="DrawableObject"/>
            /// </summary>
            public int Width { get; set; }

            /// <summary>
            /// Gets or sets the height of the <see cref="DrawableObject"/>
            /// </summary>
            public int Height { get; set; }

            /// <summary>
            /// Gets or sets the image type of the <see cref="DrawableObject"/>
            /// </summary>
            public ImageType ImageType { get; set; }
        }
    }
}
