/**
    Copyright 2012 Micah Lieske

    This file is part of SpiridiEngine.

    SpiridiEngine is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

    SpiridiEngine is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.

    You should have received a copy of the GNU General Public License along with Foobar. If not, see http://www.gnu.org/licenses/.
**/

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Spiridios.SpiridiEngine
{
    public class AnimatedSprite : Sprite
    {
        private const string XML_CONFIG_ROOT_ELEMENT = "AnimatedSprite";
        private const string XML_CONFIG_ANIMATION_ELEMENT = "Animation";
        private const string XML_CONFIG_FRAME_ELEMENT = "Frame";

        private struct FrameInfo
        {
            public FrameInfo(int frameNumber, double frameSeconds)
            {
                this.frameSeconds = frameSeconds;
                this.frameNumber = frameNumber;
            }
            public double frameSeconds;
            public int frameNumber;
        };

        private TileSet image = null;
        private Vector2 centerOffset;

        private Dictionary<string, List<FrameInfo>> animations = new Dictionary<string, List<FrameInfo>>();

        private int currentFrameIndex = 0;
        private int currentTile = 1;
        private string currentAnimation;
        private double currentFrameElapsedSeconds = 0;


        public AnimatedSprite(string imageName, int tileWidth, int tileHeight)
            : base()
        {
            CreateSprite(imageName, tileWidth, tileHeight);
        }

        public AnimatedSprite(string imageName, int tileWidth, int tileHeight, int startingFrame)
            : this(imageName, tileWidth, tileHeight)
        {
            this.currentFrameIndex = startingFrame;
        }

        private void CreateSprite(string imageName, int tileWidth, int tileHeight)
        {
            if (!SpiridiGame.ImageManagerInstance.ImageExists(imageName))
            {
                SpiridiGame.ImageManagerInstance.AddImage(imageName, imageName);
            }
            image = new TileSet(imageName, tileWidth, tileHeight);
            centerOffset = new Vector2(tileWidth / 2.0f, tileHeight / 2.0f);
        }

        public AnimatedSprite(string xmlAnimationFile)
            : base()
        {
            LoadXML(xmlAnimationFile);
        }

        private void LoadXML(string xmlAnimationName)
        {
            using (FileStream fileStream = new FileStream(xmlAnimationName, FileMode.Open))
            {
                using (XmlReader xmlReader = XmlReader.Create(fileStream))
                {

                     while (xmlReader.Read())
                    {
                        if (xmlReader.IsStartElement())
                        {
                            switch (xmlReader.Name)
                            {
                                case (AnimatedSprite.XML_CONFIG_ROOT_ELEMENT):
                                    int tileWidth = int.Parse(xmlReader.GetAttribute("tileWidth"));
                                    int tileHeight = int.Parse(xmlReader.GetAttribute("tileHeight"));
                                    string imageName = xmlReader.GetAttribute("image");
                                    CreateSprite(imageName, tileWidth, tileHeight);
                                    break;
                                case(AnimatedSprite.XML_CONFIG_ANIMATION_ELEMENT):
                                    LoadXMLAnimation(xmlReader);
                                    break;
                                default:
                                    throw new InvalidDataException(String.Format("Unsupported tag '{0}'", xmlReader.Name));
                            }
                        }
                    }
                }
            }
        }

        private void LoadXMLAnimation(XmlReader xmlReader)
        {
            double defaultFrameTime = 0.0;
            string animationName = "";
            do
            {
                switch (xmlReader.NodeType)
                {
                    case (XmlNodeType.Element):
                        switch (xmlReader.Name)
                        {
                            case (AnimatedSprite.XML_CONFIG_ANIMATION_ELEMENT):
                                string defaultFrameTimeString = xmlReader.GetAttribute("frameTime");
                                defaultFrameTime = defaultFrameTimeString == null ? 0.25 : double.Parse(defaultFrameTimeString);
                                animationName = xmlReader.GetAttribute("name");
                                break;
                            case (AnimatedSprite.XML_CONFIG_FRAME_ELEMENT):
                                LoadXMLFrame(xmlReader, animationName, defaultFrameTime);
                                break;
                            default:
                                throw new InvalidDataException(String.Format("TileImage: Unsupported node '{0}'", xmlReader.Name));
                        }
                        break;
                    case (XmlNodeType.EndElement):
                        if (xmlReader.Name == AnimatedSprite.XML_CONFIG_ANIMATION_ELEMENT)
                        {
                            return;
                        }
                        break;
                }
            } while (xmlReader.Read());
        }

        private void LoadXMLFrame(XmlReader xmlReader, string animationName, double defaultFrameTime)
        {
            do
            {
                switch (xmlReader.NodeType)
                {
                    case (XmlNodeType.Element):
                        switch (xmlReader.Name)
                        {
                            case (AnimatedSprite.XML_CONFIG_FRAME_ELEMENT):
                                int tileNumber = int.Parse(xmlReader.GetAttribute("tile"));
                                string frameTimeString = xmlReader.GetAttribute("frameTime");
                                double frameTime = frameTimeString == null ? defaultFrameTime : double.Parse(frameTimeString);
                                AddFrame(animationName, tileNumber, frameTime);
                                if (xmlReader.IsEmptyElement)
                                    return;
                                break;
                            default:
                                throw new InvalidDataException(String.Format("TileImage: Unsupported node '{0}'", xmlReader.Name));
                        }
                        break;
                    case (XmlNodeType.EndElement):
                        if (xmlReader.Name == AnimatedSprite.XML_CONFIG_FRAME_ELEMENT)
                        {
                            return;
                        }
                        break;
                }
            } while (xmlReader.Read());
        }

        public AnimatedSprite AddFrame(string animation, int frameNumber, double frameSeconds)
        {
            if (!animations.ContainsKey(animation))
            {
                animations.Add(animation, new List<FrameInfo>());
            }
            animations[animation].Add(new FrameInfo(frameNumber, frameSeconds));
            return this;
        }

        // TODO: most of these parameters should be PROPERTIES of the sprite, not parameters to the draw method.
        public override void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            Rectangle source = this.image.GetTileSourceRect(currentTile);
            //spriteBatch.Draw(this.image.Image, position + centerOffset, source, TintColor, Rotation, centerOffset, 1.0f, SpriteEffects.None, Layer);

            Rectangle destRect;
            destRect.X = (int)(position.X + centerOffset.X);
            destRect.Y = (int)(position.Y + centerOffset.Y);
            destRect.Width = (int)(this.image.TileWidth);
            destRect.Height = (int)(this.image.TileHeight);

            spriteBatch.Draw(this.image.Image, destRect, source, TintColor, Rotation, centerOffset, SpriteEffects.None, Layer);
        }

        public int CurrentFrame
        {
            get { return currentFrameIndex; }
            set
            {
                List<FrameInfo> frameInfos;
                if (currentAnimation != null && animations.TryGetValue(currentAnimation, out frameInfos))
                {
                    // TODO: make this a private SetCurrentFrame(value, frameInfos)
                    currentFrameIndex = value % frameInfos.Count;
                    FrameInfo currentFrameInfo = frameInfos[currentFrameIndex];
                    this.currentTile = currentFrameInfo.frameNumber;
                }
            }
        }

        public string CurrentAnimation
        {
            get { return this.currentAnimation; }
            set
            {
                if (this.currentAnimation != value)
                {
                    this.currentAnimation = value;
                    this.CurrentFrame = 0;
                }
            }
        }

        public AnimatedSprite SetCurrentAnimation(string currentAnimation)
        {
            this.CurrentAnimation = currentAnimation;
            return this;
        }

        public override Vector2 CenterOffset
        {
            get { return this.centerOffset; }
        }

        public override int Width
        {
            get { return image.TileWidth; }
        }

        public override int Height
        {
            get { return image.TileHeight; }
        }

        public override void Update(TimeSpan elapsedTime)
        {
            List<FrameInfo> frameInfos;
            if (currentAnimation != null && animations.TryGetValue(currentAnimation, out frameInfos))
            {
                if (currentFrameIndex < frameInfos.Count)
                {
                    currentFrameElapsedSeconds += elapsedTime.TotalSeconds;
                    FrameInfo currentFrameInfo = frameInfos[currentFrameIndex];
                    if (currentFrameElapsedSeconds > currentFrameInfo.frameSeconds)
                    {
                        currentFrameElapsedSeconds -= currentFrameInfo.frameSeconds;
                        currentFrameIndex++;
                        currentFrameIndex %= frameInfos.Count;

                        currentFrameInfo = frameInfos[currentFrameIndex];
                        this.currentTile = currentFrameInfo.frameNumber;
                    }
                }
            }
        }
    }
}
