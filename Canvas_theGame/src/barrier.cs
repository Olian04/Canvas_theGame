using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Canvas_theGame.src
{
    class Barrier
    {
        private Color color;
        private AABB dimensions;
        private static Texture2D texture;

        public static void Init(ContentManager Content) {
            texture = Content.Load<Texture2D>("square.png");
        }

        public Barrier(Rectangle dimensions, Color color) {
            this.dimensions = new AABB(dimensions);
            this.color = color;
        }

        public void Update() {
                
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, dimensions.getBoundingBox(), color);
        }



        public Color getColor()
        {
            return color;
        }
        public AABB getDimensions()
        {
            return dimensions;
        }
    }
}
