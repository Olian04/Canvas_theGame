using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Canvas_theGame.src;

namespace Canvas_theGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public enum okColors {WHITE, BLACK, ORANGE, BLUE, err};
        private static Dictionary<okColors, Color> availableColors;

        static okColors primraryColor, secondaryColor;

        Player player;
        Point playerSize;

        Level.levels currentLevel;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1080;
            Window.Position = new Point(300);
            IsMouseVisible = true;
            graphics.ApplyChanges();
        }

        public static Dictionary<okColors, Color> getAvailableColors() {
            return availableColors;
        }

        public static Color getPrimraryColor() {
            return availableColors[primraryColor];
        }
        public static Color getSecondaryColor() {
            return availableColors[secondaryColor];
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            availableColors = new Dictionary<okColors, Color>();
            availableColors.Add(okColors.WHITE, Color.White);
            availableColors.Add(okColors.BLACK, Color.Black);
            availableColors.Add(okColors.ORANGE, Color.Orange);
            availableColors.Add(okColors.BLUE, Color.Blue);

            Barrier.Init(Content);
            ColorBlob.Init(Content);

            playerSize = new Point(15, 25);
            currentLevel = Level.levels.DEMO;
            player = new Player(new Rectangle(new Point(/* Empty point; start pos is set in resetLevel. */), playerSize), availableColors[primraryColor]);

            resetLevel();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            player.Init(Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        KeyboardState ks, oldks;
        protected override void Update(GameTime gameTime)
        {
            ks = Keyboard.GetState();

            if (ks.IsKeyDown(Keys.Escape))
                Exit();

            if (oldks != ks)
                if (ks.IsKeyDown(Keys.Space)) {
                    okColors holder = primraryColor;
                    primraryColor = secondaryColor;
                    secondaryColor = holder;
                }

            foreach (Barrier b in Level.getBarriers()) {
                b.Update();
            }

            if (player.getColor() != availableColors[primraryColor]) {
                player.setColor(availableColors[primraryColor]);
            }
            player.Update(Level.getBarriers(), ks, oldks);

            if (player.getDimensions().Y > graphics.PreferredBackBufferHeight ||
                player.getDimensions().Y < 0 ||
                player.getDimensions().X > graphics.PreferredBackBufferWidth ||
                player.getDimensions().X < 0)
            {
                resetLevel();
            }

            foreach (ColorBlob c in Level.getColorBlobs())
            {
                if (player.getDimensions().Intersects(c.getDimensions()))
                {
                    primraryColor = c.getColorEnum();
                    Level.removeFromColorBlobs(c);
                    break;
                }
            }

            oldks = ks;
            base.Update(gameTime);
        }

        private void resetLevel()
        {
            Level.loadLevel(currentLevel);
            player.setPosition(Level.getStartPos());
            primraryColor = Level.getOriginalPrimraryColor();
            secondaryColor = Level.getOriginalSecondaryColor();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(availableColors[secondaryColor]);

            spriteBatch.Begin();

            player.Draw(spriteBatch);

            foreach (Barrier b in Level.getBarriers()) {
                if (b.getColor() != availableColors[secondaryColor]) {
                    b.Draw(spriteBatch);
                }
            }

            foreach (ColorBlob c in Level.getColorBlobs()) {
                c.Draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
