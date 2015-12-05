using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Canvas_theGame.src.Items
{
    class ColorBlob : src.Interfaces.Item
    {
        private AABB dimensions;
        private static Texture2D texture;
        private Game1.okColors colorEnum;

        public static void Init(ContentManager Content)
        {
            texture = Content.Load<Texture2D>("circle.png");
        }

        public ColorBlob(Rectangle dimensions, Game1.okColors colorEnum)
        {
            this.dimensions = new AABB(dimensions);
            this.colorEnum = colorEnum;
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, dimensions.getBoundingBox(), Game1.getAvailableColors()[colorEnum]);
        }

        public Game1.okColors getColor() {
            return colorEnum;
        }
        public AABB getDimensions()
        {
            return dimensions;
        }

    }
}
