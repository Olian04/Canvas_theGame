using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Canvas_theGame.src.GameModes
{
    class MainMenu : src.Interfaces.GameMode
    {
        private static Player player;
        private static Texture2D text_STORY, text_ENDLESSRUN, text_ENDLESSJUMP, ground_texture;
        private AABB storyButton, endlessRunButton, endlessJumpButton;
        private List<Items.Barrier> barriers;
        private Point startPos;

        public MainMenu()
        {
            barriers = new List<Items.Barrier>();

            endlessJumpButton = new AABB(new Rectangle(300, 400, 100, 100));
            storyButton = new AABB(new Rectangle(500, 400, 100, 100));
            endlessRunButton = new AABB(new Rectangle(700, 400, 100, 100));
            barriers.Add(new Items.Barrier(new Rectangle(100, 600, 900, 20), Game1.getPrimraryColor() ) );

            startPos = new Point(535, 550);
            Rectangle holder = new Rectangle(startPos, new Point(15, 25));
            player = new Player(holder, Game1.getPrimraryColor(), Game1.getSecondaryColor());

        }

        public static void Init(ContentManager Content)
        {
            text_ENDLESSJUMP = Content.Load<Texture2D>("text/text_ENDLESSJUMP");
            text_STORY = Content.Load<Texture2D>("text/text_STORY");
            text_ENDLESSRUN = Content.Load<Texture2D>("text/text_ENDLESSRUN");
            ground_texture = Content.Load<Texture2D>("square.png");
        }

        public void Update(GameTime gameTime)
        {
            if (player.getDimensions().Intersects(storyButton))
            {
                Game1.changeGameMode("storyMode");
            }
            else if (player.getDimensions().Intersects(endlessRunButton))
            {
                Game1.changeGameMode("endlessRun");
            }
            else if (player.getDimensions().Intersects(endlessJumpButton))
            {
                Game1.changeGameMode("endlessJump");
            }

            player.Update(barriers, Game1.ks, Game1.oldks);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(text_STORY, storyButton.getBoundingBox(), Game1.getAvailableColors()[Game1.getBackgroundColor()]);
            spriteBatch.Draw(text_ENDLESSRUN, endlessRunButton.getBoundingBox(), Game1.getAvailableColors()[Game1.getBackgroundColor()]);
            spriteBatch.Draw(text_ENDLESSJUMP, endlessJumpButton.getBoundingBox(), Game1.getAvailableColors()[Game1.getBackgroundColor()]);

            foreach (Items.Barrier b in barriers)
            {
                b.Draw(spriteBatch);
            }

            player.Draw(spriteBatch);

            spriteBatch.End();
        }

        public void PlayerOutOfBounds()
        {
            player.setPosition(startPos.ToVector2());
        }


        public Player getPlayer()
        {
            return player;
        }
    }
}
