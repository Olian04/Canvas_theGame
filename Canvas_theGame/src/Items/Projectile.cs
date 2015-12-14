using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Canvas_theGame.src.Items
{
    public class Projectile : Interfaces.Item
    {
        private static Texture2D texture;
        private static Point size;

        public static void Init(ContentManager Content)
        {
            texture = Content.Load<Texture2D>("circle.png");
            size = new Point(3);
        }

        //STATIC ABOVE | DYNAMIC BELOW

        private AABB dimentions;
        private Vector2 velocity;
        private Game1.okColors colorEnum;

        public Projectile(Point position, Vector2 velocity, Game1.okColors colorEnum)
        {
            this.dimentions = new AABB(position, size);
            this.velocity = velocity;
            this.colorEnum = colorEnum;
        }


        public void Update(GameTime gameTime)
        {
            dimentions.alterPositionAdition(velocity);
        }

        private void UpdateColision()
        {
            Game1.getCurrentGameMode();
        }

        public void Draw1(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, dimentions.getBoundingBox(), Game1.getAvailableColors()[colorEnum]);
        }

        public void Draw2(SpriteBatch spriteBatch)
        {
        }
    }
}
