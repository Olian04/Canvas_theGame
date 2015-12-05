using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Canvas_theGame.src.Items
{
    class Barrier : src.Interfaces.Item
    {
        private Game1.okColors colorEnum;
        private AABB dimensions;
        private static Texture2D texture;

        public static void Init(ContentManager Content) {
            texture = Content.Load<Texture2D>("square.png");
        }

        public Barrier(Rectangle dimensions, Game1.okColors colorEnum) {
            this.dimensions = new AABB(dimensions);
            this.colorEnum = colorEnum;
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(SpriteBatch spriteBatch) {
            if (this.getColor() != Game1.getBackgroundColor() )
            {
                spriteBatch.Draw(texture, dimensions.getBoundingBox(), Game1.getAvailableColors()[colorEnum]);
            }
        }


        public Game1.okColors getColor()
        {
            return colorEnum;
        }
        public AABB getDimensions()
        {
            return dimensions;
        }

    }
}
