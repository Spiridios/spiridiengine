﻿/**
    Copyright 2013 Micah Lieske

    This file is part of SpiridiEngine.

    SpiridiEngine is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

    SpiridiEngine is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.

    You should have received a copy of the GNU General Public License along with SpiridiEngine. If not, see http://www.gnu.org/licenses/.
**/

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Spiridios.SpiridiEngine.Physics;

namespace Spiridios.SpiridiEngine.Scene
{
    public class TileMapLayer : SceneLayer
    {
        private const string TILED_ROOT_ELEMENT = "map";
        private const string TILED_LAYER_ELEMENT = "layer";
        public const string COLLIDABLE_TAG = "Layer Collision";

        private SpiridiGame game;
        private string collisionLayerName = null;
        private List<SubsetImage> layerTileImages = null;
        private TileSetCollection tileSet;
        // Width/Height in tiles
        private int layerWidth;
        private int layerHeight;
        private int tileWidth;
        private int tileHeight;

        public TileMapLayer(SpiridiGame game, Scene scene, TileSetCollection tileSet, XmlReader mapLayerReader)
            : base(scene)
        {
            this.game = game;
            this.tileSet = tileSet;
            LoadTiledLayer(mapLayerReader);
        }

        public string CollisionLayerName
        {
            get { return this.collisionLayerName; }
        }

        public bool HasCollisionLayer
        {
            get { return this.collisionLayerName != null; }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (this.Visible)
            {
                Draw(tileSet, spriteBatch, this.Scene.Camera);
            }
        }

        private void Draw(TileSetCollection tileSet, SpriteBatch spriteBatch, Camera camera)
        {
            SortActors();

            List<Actor>.Enumerator actorEnumerator = Actors.GetEnumerator();
            Actor currentActor = null;
            if (actorEnumerator.MoveNext())
            {
                currentActor = actorEnumerator.Current;
            }
            Rectangle sourceRect = new Rectangle(0,0,this.tileWidth, this.tileHeight);
            Rectangle destRect = new Rectangle(0,0,this.tileWidth, this.tileHeight);

            int cameraTranslateY = camera.TranslateY(0);
            int y = cameraTranslateY;
            int cameraTranslateX = camera.TranslateX(0);
            int x = cameraTranslateX;
            int maxX = cameraTranslateX + (layerWidth * tileWidth);
            foreach (SubsetImage image in layerTileImages)
            {
                if (currentActor != null && (currentActor.Position.Y + cameraTranslateY) < (y + tileHeight))
                {
                    // TODO: some kind of actor culling
                    currentActor.Draw(spriteBatch);
                    if (actorEnumerator.MoveNext())
                    {
                        currentActor = actorEnumerator.Current;
                    }
                    else
                    {
                        currentActor = null;
                    }
                }

                if (image != null)
                {
                    destRect.X = x;
                    destRect.Y = y;
                    if (destRect.X >= -tileWidth && destRect.Y >= -tileHeight && destRect.X < SpiridiGame.Instance.WindowWidth && destRect.Y < SpiridiGame.Instance.WindowHeight)
                    {
                        image.DrawImpl(spriteBatch, sourceRect, destRect, Color.White, 0.0f, 1.0f);
                    }
                }

                x += tileWidth;
                if (x >= maxX)
                {
                    y += tileHeight;
                    x = cameraTranslateX;
                }
            }

            // Draw any undrawn actors.
            while(actorEnumerator.MoveNext())
            {
                actorEnumerator.Current.Draw(spriteBatch);
            }
        }

        private Collidable GetCollidableFromPosition(Vector2 position)
        {
            Collidable collidable = new Collidable();
            int tilex = (int)(position.X) / tileWidth;
            int tiley = (int)(position.Y) / tileHeight;
            int index = tiley * this.layerWidth + tilex;
            // TODO: This can get an argument out of range.
            Image image = this.layerTileImages[index];
            if (image != null)
            {
                collidable.Tag = COLLIDABLE_TAG;
                collidable.BoxCollidableShape = new BoxCollidableShape(new Rectangle(tilex * tileWidth, tiley * tileHeight, tileWidth, tileHeight));
                collidable.ImageCollidableShape = new ImageCollidableShape(new Vector2(tilex * tileWidth, tiley * tileHeight), image);
            }
            return collidable;
        }


        internal List<Collidable> GetCollidables(Rectangle actorBounds)
        {
            List<Collidable> collidables = new List<Collidable>();
            collidables.Add(GetCollidableFromPosition(new Vector2(actorBounds.X, actorBounds.Y)));
            collidables.Add(GetCollidableFromPosition(new Vector2(actorBounds.X, actorBounds.Bottom)));
            collidables.Add(GetCollidableFromPosition(new Vector2(actorBounds.Right, actorBounds.Y)));
            collidables.Add(GetCollidableFromPosition(new Vector2(actorBounds.Right, actorBounds.Bottom)));
            return collidables;
        }

        internal Collidable GetCollidable(Rectangle bounds)
        {
            Rectangle boundingRectangle = Rectangle.Empty;

            Collidable collidable = null;
            int tilex1 = bounds.X / tileWidth;
            int tiley1 = bounds.Y / tileHeight;
            int tilex2 = (bounds.Right -1) / tileWidth;
            int tiley2 = (bounds.Bottom - 1) / tileHeight;

            boundingRectangle.X = tilex1 * tileWidth;
            boundingRectangle.Y = tiley1 * tileHeight;
            int boundingWidthTiles = tilex2 - tilex1 + 1;
            int boundingHeightTiles = tiley2 - tiley1 + 1;
            boundingRectangle.Width = (boundingWidthTiles) * tileWidth;
            boundingRectangle.Height = (boundingHeightTiles) * tileHeight;

            List<Image> tileImages = new List<Image>();
            // TODO: this can be out-of-bounds.
            for (int y = tiley1; y <= tiley2; y++)
            {
                for (int x = tilex1; x <= tilex2; x++)
                {
                    int index = y * this.layerWidth + x;
                    tileImages.Add(this.layerTileImages[index]);
                }
            }

            Image image = new TileMapImage(tileImages, tileWidth, tileHeight, boundingWidthTiles, boundingHeightTiles);
            collidable = new Collidable();
            collidable.Tag = COLLIDABLE_TAG;
            collidable.BoxCollidableShape = new BoxCollidableShape(boundingRectangle); 
            collidable.ImageCollidableShape = new ImageCollidableShape(new Vector2(boundingRectangle.X, boundingRectangle.Y), image);
            return collidable;
        }

        private PositionedImage GetImageFromPosition(int x, int y)
        {
            int tilex = x/tileWidth;
            int tiley = y/tileHeight;
            Vector2 position = new Vector2(tilex * tileWidth, tiley * tileHeight);
            int index = tiley * this.layerWidth + tilex;
            return new PositionedImage(this.layerTileImages[index], position);
        }

        public static List<TileMapLayer> LoadTiledMap(SpiridiGame game, Scene scene, string tiledFile)
        {
            TileSet tileSet = null;
            TileSetCollection tileImageSet = new TileSetCollection();
            int tileWidth = 0;
            int tileHeight = 0;

            List<TileMapLayer> layers = new List<TileMapLayer>();
            using (FileStream fileStream = new FileStream(game.NormalizeFilenameSystem(tiledFile), FileMode.Open))
            {
                using (XmlReader xmlReader = XmlReader.Create(fileStream))
                {

                    while (xmlReader.Read())
                    {
                        if (xmlReader.IsStartElement())
                        {
                            switch (xmlReader.Name)
                            {
                                case (TileMapLayer.TILED_ROOT_ELEMENT):
                                    tileWidth = int.Parse(xmlReader.GetAttribute("tilewidth"));
                                    tileHeight = int.Parse(xmlReader.GetAttribute("tileheight"));
                                    break;
                                case (TileSet.TILED_TILESET_ELEMENT):
                                    //string tilesetName = TileSet.ParseTiledTilesetName(xmlReader);
                                    int startTileId = int.Parse(xmlReader.GetAttribute("firstgid"));
                                    tileSet = new TileSet(xmlReader);
                                    tileImageSet.AddTileSet(tileSet, startTileId);
                                    break;
                                case (TileMapLayer.TILED_LAYER_ELEMENT):
                                    TileMapLayer mapLayer = new TileMapLayer(game, scene, tileImageSet, xmlReader);
                                    mapLayer.tileWidth = tileWidth;
                                    mapLayer.tileHeight = tileHeight;
                                    layers.Add(mapLayer);
                                    break;
                                case ("objectgroup"):
                                    break; //ignore it for now.
                                default:
                                    throw new InvalidDataException(String.Format("Unsupported tag '{0}'", xmlReader.Name));
                            }
                        }
                    }
                }
            }
            return layers;
        }

        private void LoadTiledLayer(XmlReader xmlReader)
        {
            do
            {
                switch (xmlReader.NodeType)
                {
                    case (XmlNodeType.Element):
                        switch (xmlReader.Name)
                        {
                            case (TileMapLayer.TILED_LAYER_ELEMENT):
                                this.Name = xmlReader.GetAttribute("name");
                                layerHeight = int.Parse(xmlReader.GetAttribute("height"));
                                layerWidth = int.Parse(xmlReader.GetAttribute("width"));
                                String visibleAttribute = xmlReader.GetAttribute("visible");
                                if (visibleAttribute != null)
                                {
                                    this.Visible = int.Parse(visibleAttribute) != 0;
                                }
                                break;
                            case ("data"):
                                string encoding = xmlReader.GetAttribute("encoding");
                                if (encoding != "base64")
                                {
                                    throw new InvalidDataException(String.Format("Unsupported encoding {0}", encoding));
                                }

                                string compression = xmlReader.GetAttribute("compression");
                                if (!String.IsNullOrEmpty(compression) && compression != "gzip")
                                {
                                    throw new InvalidDataException(String.Format("Unsupported compression {0}", compression));
                                }

                                string layerString = xmlReader.ReadElementContentAsString().Trim();
                                byte[] rawLayer = Convert.FromBase64String(layerString);

                                int size = layerWidth * layerHeight;
                                layerTileImages = new List<SubsetImage>(size);

                                Stream layerStream = new MemoryStream(rawLayer);
                                if (compression == "gzip")
                                {
                                    layerStream = new GZipStream(layerStream, CompressionMode.Decompress);
                                }

                                using (BinaryReader layerReader = new BinaryReader(layerStream))
                                {
                                    for (int i = 0; i < size; i++)
                                    {
                                        int tileId = layerReader.ReadInt32();
                                        layerTileImages.Add(this.tileSet.GetImage(tileId));
                                    }
                                }
                                break;
                            case ("properties"):
                                ReadProperties(xmlReader);
                                break;
                            default:
                                throw new InvalidOperationException(string.Format("TileImage: Unsupported node '{0}'", xmlReader.Name));
                        }
                        break;
                    case (XmlNodeType.EndElement):
                        if (xmlReader.Name == TILED_LAYER_ELEMENT)
                        {
                            return;
                        }
                        break;
                }
            } while (xmlReader.Read());
        }

        private void ReadProperties(XmlReader xmlReader)
        {
            do
            {
                switch (xmlReader.NodeType)
                {
                    case (XmlNodeType.Element):
                        switch (xmlReader.Name)
                        {
                            case ("properties"):
                                break;
                            case ("property"):
                                string propertyName = xmlReader.GetAttribute("name");
                                switch (propertyName)
                                {
                                    case ("CollisionLayer"):
                                        this.collisionLayerName = xmlReader.GetAttribute("value");
                                        break;
                                    case ("Visible"):
                                        this.Visible = Boolean.Parse(xmlReader.GetAttribute("value"));
                                        break;
                                    default:
                                        throw new InvalidOperationException(string.Format("TileImage: Unsupported node '{0}'", xmlReader.Name));
                                }
                                break;
                            default:
                                throw new InvalidOperationException(string.Format("TileImage: Unsupported node '{0}'", xmlReader.Name));
                        }
                        break;
                    case (XmlNodeType.EndElement):
                        if (xmlReader.Name == "properties")
                        {
                            return;
                        }
                        break;
                }
            } while (xmlReader.Read());
        }
    }
}
