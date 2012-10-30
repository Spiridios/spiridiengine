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
        /// Adds an image to the image manager
        /// </summary>
        /// <param name="imageName">The internal name for the image</param>
        /// <param name="imageSrc">The actual image name</param>
        public Texture2D AddImage(string imageName, string imageSrc)
        {
            if (this.images.ContainsKey(imageName))
            {
                throw new Exception(String.Format("An image named '{0}' already exists.", imageName));
            }
            // Load image
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

        internal void Clear()
        {
            this.images.Clear();
        }
    }
}
