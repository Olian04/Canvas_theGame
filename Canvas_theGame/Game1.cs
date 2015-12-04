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

        public enum okColors {WHITE, BLACK, ORANGE, err};
        private static Dictionary<okColors, Color> availableColors;
        private void InitOkColors() {
            availableColors = new Dictionary<okColors, Color>();
            availableColors.Add(okColors.WHITE, Color.White);
            availableColors.Add(okColors.BLACK, Color.Black);
            availableColors.Add(okColors.ORANGE, Color.Orange);
        }

        static okColors primraryColor, secondaryColor, backgroundColor;

        private static Player player;
        Point playerSize;

        static Level.levels currentLevel;

        static float gravity;

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

        public static okColors getPrimraryColor() {
            return primraryColor;
        }
        public static okColors getSecondaryColor() {
            return secondaryColor;
        }
        public static okColors getBackgroundColor() {
            return backgroundColor;
        }

        public static float getGravity() {
            return gravity;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            InitOkColors();
            Barrier.Init(Content);
            ColorBlob.Init(Content);

            gravity = 1;

            playerSize = new Point(15, 25);
            currentLevel = Level.levels.DEMO1;
            player = new Player(new Rectangle(new Point(/* Empty point; start pos is set in resetLevel. */), playerSize), primraryColor, secondaryColor);

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

            if (ks.IsKeyDown(Keys.Space) && !oldks.IsKeyDown(Keys.Space)) {
                okColors holder = primraryColor;
                primraryColor = backgroundColor;
                backgroundColor = holder;
            }

            if (player.getOuterColor() != primraryColor || player.getInnerColor() != secondaryColor) {
                player.setOuterColor(primraryColor);
                player.setInnerColor(secondaryColor);
            }
            player.Update(Level.getBarriers(), ks, oldks);

            if (player.getDimensions().getBoundingBox().Y > graphics.PreferredBackBufferHeight ||
                player.getDimensions().getBoundingBox().Y < 0 ||
                player.getDimensions().getBoundingBox().X > graphics.PreferredBackBufferWidth ||
                player.getDimensions().getBoundingBox().X < 0)
            {
                resetLevel();
            }

            foreach (Barrier b in Level.getBarriers())
                b.Update();

            foreach (ColorBlob c in Level.getColorBlobs())
            {
                if (player.getDimensions().Intersects(c.getDimensions()))
                {
                    primraryColor = c.getColor();
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
            resetLevelStatic();
        }
        public static void resetLevelStatic()
        {
            player.setPosition(Level.getStartPos());
            primraryColor = Level.getOriginalPrimraryColor();
            secondaryColor = Level.getOriginalSecondaryColor();
            backgroundColor = Level.getOriginalBackgroundColor();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            if (backgroundColor != okColors.BLACK)
                GraphicsDevice.Clear(Color.Lerp(availableColors[backgroundColor], Color.Black, 0.1f));
            else
                GraphicsDevice.Clear(Color.Lerp(availableColors[backgroundColor], Color.White, 0.1f));


            spriteBatch.Begin();

            player.Draw(spriteBatch);

            foreach (Barrier b in Level.getBarriers()) {
                b.Draw(spriteBatch);
            }

            foreach (ColorBlob c in Level.getColorBlobs()) {
                c.Draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
