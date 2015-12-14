using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using Canvas_theGame.src.Items;
using Canvas_theGame.src.Interfaces;

namespace Canvas_theGame.src.GameModes
{
    public class StoryMode : Interfaces.GameMode
    {
        private static Player player;
        private static Point playerSize;

        static Level.levels currentLevel;

        public StoryMode() {
            currentLevel = Level.levels.DEMO1;
            player = new Player(new Rectangle(new Point(/* Empty point; start pos is set in resetLevel. */), playerSize), Game1.getPrimraryColor(), Game1.getSecondaryColor());
            PlayerOutOfBounds();
        }

        public static void Init(ContentManager Content)
        {
            playerSize = new Point(15, 25);
        }

        public void Update(GameTime gameTime) {

            foreach (Barrier b in Level.getBarriers())
                b.Update(gameTime);

            foreach (ColorBlob c in Level.getColorBlobs())
            {
                if (player.getDimensions().Intersects(c.getDimensions()))
                {
                    Game1.setPrimaryColor(c.getColor() );
                    Level.removeFromColorBlobs(c);
                    break;
                }
            }

            player.Update(Level.getBarriers(), Game1.ks, Game1.oldks);

            Level.Update(player.getDimensions());
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            player.Draw(spriteBatch);

            foreach (Barrier b in Level.getBarriers())
            {
                b.Draw1(spriteBatch);
            }

            foreach (Barrier b in Level.getBarriers())
            {
                b.Draw2(spriteBatch);
            }

            foreach (ColorBlob c in Level.getColorBlobs())
            {
                c.Draw1(spriteBatch);
            }

            spriteBatch.End();

        }

        public void PlayerOutOfBounds()
        {
            Level.loadLevel(currentLevel);
            resetLevelStatic();
        }

        public static void resetLevelStatic()
        {
            player.setPosition(Level.getStartPos());
            Game1.setPrimaryColor(Level.getOriginalPrimraryColor() );
            Game1.setSecondaryColor(Level.getOriginalSecondaryColor() );
            Game1.setBackgroundColor(Level.getOriginalBackgroundColor() );
        }

        public Player getPlayer()
        {
            return player;
        }

        public void addProjectile(Projectile projectile)
        {
        }
    }
}
