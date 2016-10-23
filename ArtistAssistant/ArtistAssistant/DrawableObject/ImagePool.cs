//-----------------------------------------------------------------------
// <copyright file="ImagePool.cs" company="Peter Scheelke">
//      Copyright (c) Peter Scheelke. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ArtistAssistant.DrawableObject
{
    using System.Collections.Generic;
    using System.Drawing;
    using Properties;

    /// <summary>
    /// A pool of all the images that can be used for <see cref="DrawableObject"/>s.
    /// The <see cref="ImagePool"/> is a singleton.
    /// </summary>
    public class ImagePool
    {
        /// <summary>
        /// The single instance of the <see cref="ImagePool"/> class
        /// </summary>
        private static ImagePool uniqueInstance = null;

        /// <summary>
        /// An object used to make access to the <see cref="ImagePool"/> thread safe
        /// </summary>
        private static object poolLock = new object();

        /// <summary>
        /// A <see cref="Dictionary{TKey, TValue}"/> contained in <see cref="uniqueInstance"/>
        /// that contains the images that have been requested so far.
        /// </summary>
        private Dictionary<ImageType, Image> pool;

        /// <summary>
        /// Prevents a default instance of the <see cref="ImagePool"/> class from being created
        /// </summary>
        private ImagePool()
        {
            this.pool = new Dictionary<ImageType, Image>();
        }

        /// <summary>
        /// Gets the number of images currently contained in the image pool.
        /// </summary>
        public static int Count
        {
            get
            {
                if (uniqueInstance == null)
                {
                    return 0;
                }
                else
                {
                    return uniqueInstance.pool.Count;
                }
            }
        }

        /// <summary>
        /// Gets the image that corresponds to the given <see cref="ImageType"/>
        /// </summary>
        /// <param name="imageType">The <see cref="ImageType"/> corresponding to an image resource</param>
        /// <returns>
        /// An <see cref="Image"/> object containing an image created from a resource object based
        /// on the requested <see cref="ImageType"/>
        /// </returns>
        public static Image GetImage(ImageType imageType)
        {
            lock (ImagePool.poolLock)
            {
                if (ImagePool.uniqueInstance == null)
                {
                    ImagePool.uniqueInstance = new ImagePool();
                }

                if (!uniqueInstance.pool.ContainsKey(imageType))
                {
                    string imageName = imageType.ToString();
                    uniqueInstance.pool.Add(imageType, (Image)Resources.ResourceManager.GetObject(imageName));
                }

                return ImageWrapper.Create(uniqueInstance.pool[imageType]).Image;
            }
        }

        /// <summary>
        /// Wraps an <see cref="Image"/> to make it read only
        /// </summary>
        private class ImageWrapper
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ImageWrapper"/> class
            /// </summary>
            /// <param name="image">The <see cref="Image"/> being wrapped</param>
            public ImageWrapper(Image image)
            {
                this.Image = image;
            }

            /// <summary>
            /// Gets the <see cref="Image"/> being wrapped
            /// </summary>
            public Image Image { get; private set; }

            /// <summary>
            /// Creates a new instance of the <see cref="ImageWrapper"/> class
            /// </summary>
            /// <param name="image">The <see cref="Image"/> being wrapped</param>
            /// <returns>A new instance of the <see cref="ImageWrapper"/> class</returns>
            public static ImageWrapper Create(Image image)
            {
                return new ImageWrapper(image);
            }
        }
    }
}