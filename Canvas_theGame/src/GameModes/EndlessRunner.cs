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
    public class EndlessRunner : Interfaces.GameMode
    {
        private static Player player;
        private static Point playerSize, startPos;
        private static int barrierSize, scrollSpeed, groundHeight, minBetweenBarriers;
            
            
        private List<Barrier> ground;
        private List<Barrier> barriers;
        private List<ColorBlob> colorBlobs;
        private Game1.okColors groundColor, barrierColor, barrierBlockColor;
        private int betweenBerriers;

        public EndlessRunner()
        {
            groundColor = Game1.getPrimraryColor();
            barrierColor = Game1.getPrimraryColor();
            barrierBlockColor = Game1.getSecondaryColor();

            player = new Player(new Rectangle(startPos, playerSize), Game1.getPrimraryColor(), Game1.getSecondaryColor(), false);
            ground = new List<Barrier>();
            barriers = new List<Barrier>();
            colorBlobs = new List<ColorBlob>();

            betweenBerriers = 1000;

            setUp();
        }

        public static void Init(ContentManager Content)
        {
            playerSize = new Point(15, 25);
            startPos = new Point(400, 500);
            barrierSize = 20;
            groundHeight = 600;
            scrollSpeed = -10;
            minBetweenBarriers = 300;
        }

        private void setUp()
        {
            groundColor = Game1.getPrimraryColor();
            for (int i = 0; i < Game1.getScreenSize().X / barrierSize + 1; i++)
            {
                addGround(new Point(i * barrierSize, groundHeight));
            }
        }

        private void addGround(Point position)
        {
            ground.Add(new Barrier(new Rectangle(position.X, position.Y, barrierSize, barrierSize), groundColor));
        }

        private void addBarrier(Point position, Point _barrierSize)
        {
            barriers.Add(new Barrier(new Rectangle( position.X, position.Y, _barrierSize.X, _barrierSize.Y), barrierColor ) );
            barriers.Add(new Barrier(new Rectangle( position.X, 0, _barrierSize.X, groundHeight - _barrierSize.Y), barrierBlockColor ) );

            barrierBlockColor = Game1.getBackgroundColor();
            groundColor = barrierBlockColor;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            foreach (Barrier b in ground)
            {
                b.Draw1(spriteBatch);
            }
            foreach (Barrier b in ground)
            {
                b.Draw2(spriteBatch);
            }

            foreach (Barrier b in barriers)
            {
                b.Draw1(spriteBatch);
            }
            foreach (Barrier b in barriers)
            {
                b.Draw2(spriteBatch);
            }

            foreach (ColorBlob c in colorBlobs)
            {
                c.Draw1(spriteBatch);
            }

            player.Draw(spriteBatch);

            spriteBatch.End();
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < ground.Count; i++)
            {
                Barrier b = ground[i];
                b.Update(gameTime);
                b.getDimensions().alterPositionAdition(new Vector2(scrollSpeed, 0));
                if (b.getDimensions().getBoundingBox().Right < 0)
                {
                    ground.Remove(b);
                    addGround(new Point(Game1.getScreenSize().X, groundHeight));
                    i--;
                }
            }

            for (int i = 0; i < barriers.Count; i++)
            {
                Barrier b = barriers[i];
                b.Update(gameTime);
                b.getDimensions().alterPositionAdition(new Vector2(scrollSpeed, 0));
                if (b.getDimensions().getBoundingBox().Right < 0)
                {
                    barriers.Remove(b);

                    if (betweenBerriers > minBetweenBarriers)
                    {
                        betweenBerriers -= 20;
                    }
                    else
                    {
                        betweenBerriers = minBetweenBarriers;
                    }

                    i--;
                }
            }

            if (barriers.Count > 0)
            {
                if (Game1.getScreenSize().X - barriers[barriers.Count - 1].getDimensions().getBoundingBox().X > betweenBerriers)
                {
                    addBarrier(new Point(Game1.getScreenSize().X, groundHeight - 100), new Point(20, 100));
                }
            }
            else
            {
                addBarrier(new Point(Game1.getScreenSize().X, groundHeight - 100), new Point(20, 100));
            }

            foreach (ColorBlob c in colorBlobs)
            {
                if (player.getDimensions().Intersects(c.getDimensions()))
                {
                    Game1.setPrimaryColor(c.getColor());
                    colorBlobs.Remove(c);
                    break;
                }
            }

            player.Update(combinedLists(), Game1.ks, Game1.oldks);
        }

        private List<Barrier> combinedLists()
        {
            List<Barrier> holder = new List<Barrier>();

            foreach (Barrier b in barriers)
                holder.Add(b);
            foreach (Barrier b in ground)
                holder.Add(b);

            return holder;
        }

        public void PlayerOutOfBounds()
        {
            player.setPosition(startPos.ToVector2());
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
