using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Canvas_theGame.src.Items
{
    public class Barrier : src.Interfaces.Item
    {
        private Game1.okColors colorEnum;
        private AABB dimensions;
        private static Texture2D squareTexture;
        private static Texture2D circleTexture;
        private static int dotSizeReduct;
        public static bool dotHiddenPlatforms;

        public static void Init(ContentManager Content) {
            squareTexture = Content.Load<Texture2D>("square.png");
            circleTexture = Content.Load<Texture2D>("circle.png");
            dotHiddenPlatforms = false;
            dotSizeReduct = 4;
        }

        public Barrier(Rectangle dimensions, Game1.okColors colorEnum) {
            this.dimensions = new AABB(dimensions);
            this.colorEnum = colorEnum;
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw1(SpriteBatch spriteBatch) {
            if (this.getColor() != Game1.getBackgroundColor())
            {
                spriteBatch.Draw(squareTexture, dimensions.getBoundingBox(), Game1.getAvailableColors()[colorEnum]);
            }
        }

        public void Draw2(SpriteBatch spriteBatch) {
            if (dotHiddenPlatforms && this.getColor() == Game1.getBackgroundColor())
            {
                spriteBatch.Draw(circleTexture,
                    new Rectangle(dimensions.getBoundingBox().X + dimensions.getBoundingBox().Width / 2, dimensions.getBoundingBox().Y + dimensions.getBoundingBox().Height / 2, dimensions.getBoundingBox().Width / dotSizeReduct, dimensions.getBoundingBox().Height / dotSizeReduct),
                    Color.Lerp(Game1.getAvailableColors()[colorEnum], Game1.getAvailableColors()[Game1.getPrimraryColor()], 0.2f));
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
