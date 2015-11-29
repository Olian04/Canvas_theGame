using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Canvas_theGame.src
{
    class ColorBlob
    {
        private Color color;
        private AABB dimensions;
        private static Texture2D texture;
        private Game1.okColors colorEnum;

        public static void Init(ContentManager Content)
        {
            texture = Content.Load<Texture2D>("circle.png");
        }

        public ColorBlob(Rectangle dimensions, Color color, Game1.okColors colorEnum)
        {
            this.dimensions = new AABB(dimensions);
            this.color = color;
            this.colorEnum = colorEnum;
        }


        public void Update()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, dimensions.getBoundingBox(), color);
        }


        public Color getColor()
        {
            return color;
        }
        public Game1.okColors getColorEnum() {
            return colorEnum;
        }
        public AABB getDimensions()
        {
            return dimensions;
        }
    }
}
