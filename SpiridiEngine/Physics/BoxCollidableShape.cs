using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Spiridios.SpiridiEngine
{
    public class BoxCollidableShape
    {
        private Rectangle rectangle;

        public BoxCollidableShape(Rectangle rectangle)
        {
            this.rectangle = rectangle;
        }

        public BoxCollidableShape(int x, int y, int width, int height)
            : this(new Rectangle(x,y, width, height))
        {
        }

        private BoxCollidableShape()
            : this(new Rectangle(0,0,0,0))
        {
        }

        public Rectangle Rectangle
        {
            get { return this.rectangle; }
            set { this.rectangle = value; }
        }

    }
}
