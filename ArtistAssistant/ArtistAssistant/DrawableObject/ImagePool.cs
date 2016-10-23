//-----------------------------------------------------------------------
// <copyright file="ImagePool.cs" company="Peter Scheelke">
//      Copyright (c) Peter Scheelke. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ArtistAssistant.DrawableObject
{
    using System.Collections.Generic;
    using System.Drawing;

    public class ImagePool
    {
        private static ImagePool uniqueInstance;

        private List<Image> pool;

        private ImagePool()
        {
            this.pool = new List<Image>();
        }
    }
}
