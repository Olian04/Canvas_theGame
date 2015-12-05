using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Canvas_theGame.src.Items;

namespace Canvas_theGame.src.GameModes
{
    class EndlessRunner : Interfaces.GameMode
    {
        private static Player player;
        private static Point playerSize;

        public EndlessRunner()
        {
            player = new Player(new Rectangle(new Point(/* Empty point; start pos is set in resetLevel. */), playerSize), Game1.getPrimraryColor(), Game1.getSecondaryColor() );
        }

        public static void Init(ContentManager Content)
        {
            playerSize = new Point(15, 25);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            player.Draw(spriteBatch);

            foreach (Barrier b in Level.getBarriers())
            {
                b.Draw(spriteBatch);
            }

            foreach (ColorBlob c in Level.getColorBlobs())
            {
                c.Draw(spriteBatch);
            }

            spriteBatch.End();
        }

        public void Update(GameTime gameTime)
        {
            foreach (Barrier b in Level.getBarriers())
                b.Update(gameTime);

            foreach (ColorBlob c in Level.getColorBlobs())
            {
                if (player.getDimensions().Intersects(c.getDimensions()))
                {
                    Game1.setPrimaryColor(c.getColor());
                    Level.removeFromColorBlobs(c);
                    break;
                }
            }
        }

        public void PlayerOutOfBounds()
        {
        }

        public Player getPlayer()
        {
            return player;
        }
    }
}
