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

        enum okColors {WHITE, BLACK, err};
        Dictionary<okColors, Color> availableColors;

        okColors primraryColor, secondaryColor;
        okColors originalPrimraryColor, originalSecondaryColor;

        Player player;
        Point startPos;

        List<Barrier> barriers;

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

            primraryColor = okColors.BLACK;
            secondaryColor = okColors.WHITE;

            Barrier.Init(Content);
            barriers = new List<Barrier>();
            this.loadLevel();

            Point playerSize = new Point(15, 25);
            startPos = new Point(350 - playerSize.X / 2, 250);
            player = new Player(new Rectangle(startPos, playerSize), availableColors[primraryColor]);

            base.Initialize();
        }

        private void loadLevel() {
            originalPrimraryColor = okColors.BLACK;
            originalSecondaryColor = okColors.WHITE;
            barriers.Add(new Barrier(new Rectangle(new Point(300, 300), new Point(100, 20)), availableColors[okColors.BLACK])); //Floor
            barriers.Add(new Barrier(new Rectangle(new Point(300, 200), new Point(100, 20)), availableColors[okColors.BLACK])); //Roof
            barriers.Add(new Barrier(new Rectangle(new Point(280, 200), new Point(20, 120)), availableColors[okColors.BLACK])); //Left wall
            barriers.Add(new Barrier(new Rectangle(new Point(380, 200), new Point(20, 120)), availableColors[okColors.BLACK])); //Right wall

            barriers.Add(new Barrier(new Rectangle(new Point(700, 300), new Point(100, 20)), availableColors[okColors.BLACK])); //Floor
            barriers.Add(new Barrier(new Rectangle(new Point(700, 200), new Point(100, 20)), availableColors[okColors.BLACK])); //Roof
            barriers.Add(new Barrier(new Rectangle(new Point(680, 200), new Point(20, 120)), availableColors[okColors.BLACK])); //Left wall
            barriers.Add(new Barrier(new Rectangle(new Point(780, 200), new Point(20, 120)), availableColors[okColors.BLACK])); //Right wall

            barriers.Add(new Barrier(new Rectangle(new Point(300, 600), new Point(500, 20)), availableColors[okColors.WHITE])); //Bottom Line

            barriers.Add(new Barrier(new Rectangle(new Point(500, 500), new Point(100, 20)), availableColors[okColors.WHITE])); //White help line

            barriers.Add(new Barrier(new Rectangle(new Point(500, 400), new Point(100, 20)), availableColors[okColors.BLACK])); //Black help line

            barriers.Add(new Barrier(new Rectangle(new Point(700, 300), new Point(100, 20)), availableColors[okColors.WHITE])); //White end floor
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

            foreach (Barrier b in barriers) {
                b.Update();
            }

            if (player.getColor() != availableColors[primraryColor]) {
                player.setColor(availableColors[primraryColor]);
            }
            player.Update(barriers, ks, oldks);

            if (player.getDimensions().Y > graphics.PreferredBackBufferHeight ||
                player.getDimensions().Y < 0 ||
                player.getDimensions().X > graphics.PreferredBackBufferWidth ||
                player.getDimensions().X < 0)
            {
                resetLevel();
            }

            oldks = ks;
            base.Update(gameTime);
        }

        private void resetLevel()
        {
            player.setPosition(startPos);
            primraryColor = originalPrimraryColor;
            secondaryColor = originalSecondaryColor;
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

            foreach (Barrier b in barriers) {
                if (b.getColor() != availableColors[secondaryColor]) {
                    b.Draw(spriteBatch);
                }
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
