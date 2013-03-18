/**
    Copyright 2013 Micah Lieske

    This file is part of SpiridiEngine.

    SpiridiEngine is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

    SpiridiEngine is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.

    You should have received a copy of the GNU General Public License along with SpiridiEngine. If not, see http://www.gnu.org/licenses/.
**/

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace Spiridios.SpiridiEngine
{

    public class ImageManager
    {
        Dictionary<string, Texture2D> images = new Dictionary<string, Texture2D>();
        ContentManager contentManager;

        internal ImageManager(ContentManager contentManager)
        {
            this.contentManager = contentManager;
        }

        /// <summary>
        /// Adds an image to the image manager. If the image already exits, then the existing image is used.
        /// </summary>
        /// <param name="imageName">The internal name for the image</param>
        /// <param name="imageSrc">The actual image name</param>
        /// <returns>The image just added.</returns>
        public Texture2D AddImage(string imageName, string imageSrc)
        {
            Texture2D tex;
            if (!this.images.TryGetValue(imageName, out tex))
            {
                // Load image
                tex = this.GetImageNoCache(imageSrc);
                this.images[imageName] = tex;
            }
            return tex;
        }

        /// <summary>
        /// Replaces an existing image in the image manager. If the image doesn't exist, it is added.
        /// </summary>
        /// <param name="imageName">The internal name for the image</param>
        /// <param name="imageSrc">The actual image name</param>
        /// <returns>The image just added.</returns>
        public Texture2D ReplaceImage(string imageName, string imageSrc)
        {
            Texture2D tex = this.GetImageNoCache(imageSrc);
            this.images[imageName] = tex;
            return tex;
        }

        private Texture2D GetImageNoCache(string imageSrc)
        {
            try
            {
                string correctedName = SpiridiGame.NormalizeFilename(imageSrc);
                Texture2D tex = this.contentManager.Load<Texture2D>(correctedName);
                return tex;
            }
            catch (Exception e)
            {
                throw new Exception(String.Format("Could not load texture '{0}' - {1}", imageSrc, e.Message));
            }
        }

        public Texture2D GetImage(string imageName)
        {
            return this.images[imageName];
        }

        public bool ImageExists(string imageName)
        {
            return this.images.ContainsKey(imageName);
        }

        internal void Clear()
        {
            this.images.Clear();
        }
    }
}
