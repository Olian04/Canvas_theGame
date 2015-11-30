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
        private Game1.okColors colorEnum;
        private AABB dimensions;
        private static Texture2D texture;
        private float visablePulse;

        public static void Init(ContentManager Content) {
            texture = Content.Load<Texture2D>("square.png");
        }

        public Barrier(Rectangle dimensions, Game1.okColors colorEnum) {
            this.dimensions = new AABB(dimensions);
            this.colorEnum = colorEnum;
        }

        public void Update() {

            if (visablePulse < 0.01f)
                visablePulse = 0;
            else
                visablePulse -= 0.01f;
                
        }

        public void pulseVisable() {
            visablePulse = 1;
        }

        public void Draw(SpriteBatch spriteBatch) {
            if (this.getColor() == Game1.getPrimraryColor())
            {
                spriteBatch.Draw(texture, dimensions.getBoundingBox(), Game1.getAvailableColors()[colorEnum]);
            }
            else if (visablePulse > 0)
            {
                spriteBatch.Draw(texture, dimensions.getBoundingBox(), Color.Lerp(Color.TransparentBlack, Game1.getAvailableColors()[colorEnum], visablePulse));
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
