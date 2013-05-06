/**
    Copyright 2013 Micah Lieske

    This file is part of SpiridiEngine.

    SpiridiEngine is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

    SpiridiEngine is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.

    You should have received a copy of the GNU General Public License along with SpiridiEngine. If not, see http://www.gnu.org/licenses/.
**/

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Spiridios.SpiridiEngine
{
    public class AnimatedImage : Image, Updatable
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

        //TODO: do we need both?
        private TileSet tileSet = null;
        private SubsetImage image;

        private Dictionary<string, List<FrameInfo>> animations = new Dictionary<string, List<FrameInfo>>();

        private int currentFrameIndex = 0;
        private int currentTile = 1;
        private string currentAnimation;
        private double currentFrameElapsedSeconds = 0;

        public AnimatedImage(string imageName, int tileWidth, int tileHeight)
            : base()
        {
            CreateImage(imageName, tileWidth, tileHeight);
        }

        public AnimatedImage(string imageName, int tileWidth, int tileHeight, int startingFrame)
            : this(imageName, tileWidth, tileHeight)
        {
            this.currentFrameIndex = startingFrame;
        }

        private void CreateImage(string imageName, int tileWidth, int tileHeight)
        {
            SpiridiGame.ImageManagerInstance.AddImage(imageName, imageName);
            tileSet = new TileSet(imageName, tileWidth, tileHeight);
            this.image = new SubsetImage(new TextureImage(tileSet.Texture), Rectangle.Empty);
            //this.Origin = new Vector2(tileWidth / 2.0f, tileHeight / 2.0f);
        }

        public AnimatedImage(string xmlAnimationFile)
            : base()
        {
            LoadXML(xmlAnimationFile);
        }

        private void LoadXML(string xmlAnimationName)
        {
            using (FileStream fileStream = new FileStream(SpiridiGame.Instance.NormalizeFilenameSystem(xmlAnimationName), FileMode.Open))
            {
                using (XmlReader xmlReader = XmlReader.Create(fileStream))
                {

                     while (xmlReader.Read())
                    {
                        if (xmlReader.IsStartElement())
                        {
                            switch (xmlReader.Name)
                            {
                                case (AnimatedImage.XML_CONFIG_ROOT_ELEMENT):
                                    int tileWidth = int.Parse(xmlReader.GetAttribute("tileWidth"));
                                    int tileHeight = int.Parse(xmlReader.GetAttribute("tileHeight"));
                                    string imageName = xmlReader.GetAttribute("image");
                                    CreateImage(imageName, tileWidth, tileHeight);
                                    break;
                                case (AnimatedImage.XML_CONFIG_ANIMATION_ELEMENT):
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
                            case (AnimatedImage.XML_CONFIG_ANIMATION_ELEMENT):
                                string defaultFrameTimeString = xmlReader.GetAttribute("frameTime");
                                defaultFrameTime = defaultFrameTimeString == null ? 0.25 : double.Parse(defaultFrameTimeString);
                                animationName = xmlReader.GetAttribute("name");
                                break;
                            case (AnimatedImage.XML_CONFIG_FRAME_ELEMENT):
                                LoadXMLFrame(xmlReader, animationName, defaultFrameTime);
                                break;
                            default:
                                throw new InvalidDataException(String.Format("TileImage: Unsupported node '{0}'", xmlReader.Name));
                        }
                        break;
                    case (XmlNodeType.EndElement):
                        if (xmlReader.Name == AnimatedImage.XML_CONFIG_ANIMATION_ELEMENT)
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
                            case (AnimatedImage.XML_CONFIG_FRAME_ELEMENT):
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
                        if (xmlReader.Name == AnimatedImage.XML_CONFIG_FRAME_ELEMENT)
                        {
                            return;
                        }
                        break;
                }
            } while (xmlReader.Read());
        }

        public AnimatedImage AddFrame(string animation, int frameNumber, double frameSeconds)
        {
            if (!animations.ContainsKey(animation))
            {
                animations.Add(animation, new List<FrameInfo>());
            }
            animations[animation].Add(new FrameInfo(frameNumber, frameSeconds));
            return this;
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
                    this.image.SourceRectangle = this.tileSet.GetTileSourceRect(currentTile);
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

        public AnimatedImage SetCurrentAnimation(string currentAnimation)
        {
            this.CurrentAnimation = currentAnimation;
            return this;
        }

        public override int Width
        {
            get { return tileSet.TileWidth; }
        }

        public override int Height
        {
            get { return tileSet.TileHeight; }
        }

        public void Update(TimeSpan elapsedTime)
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
                        this.image.SourceRectangle = this.tileSet.GetTileSourceRect(currentTile);
                    }
                }
            }
        }

        protected override void DrawImpl(SpriteBatch spriteBatch, Rectangle source, Rectangle destination, Color tintColor, float rotation, float layer)
        {
            this.image.Draw(spriteBatch, source, destination, tintColor, rotation, layer);
        }

        public override Color GetPixel(int x, int y)
        {
            return this.image.GetPixel(x, y);
        }

        public override Color GetPixel(Point point)
        {
            return this.image.GetPixel(point);
        }
    }
}
