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

        public static KeyboardState ks, oldks;

        private static Point screenSize = new Point(1080, 720);

        private static src.Interfaces.GameMode currentGameMode = null;

        public enum okColors { WHITE, BLACK, BLUE, ORANGE, GREY, err };
        private static Dictionary<okColors, Color> availableColors;
        private void InitOkColors()
        {
            availableColors = new Dictionary<okColors, Color>();
            availableColors.Add(okColors.WHITE, Color.White);
            availableColors.Add(okColors.BLACK, Color.Black);
            availableColors.Add(okColors.BLUE, new Color(0, 110, 199));
            availableColors.Add(okColors.ORANGE, new Color(247, 163, 10));
            availableColors.Add(okColors.GREY, new Color(86, 90, 95));
        }

        private static okColors primraryColor, secondaryColor, backgroundColor;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = screenSize.Y;
            graphics.PreferredBackBufferWidth = screenSize.X;
            Window.Position = new Point(300);
            IsMouseVisible = true;
            graphics.ApplyChanges();
        }

        public static void changeGameMode(string gameMode)
        {
            SetBaseColors();

            if (gameMode == "mainMenu")
            {
                currentGameMode = new src.GameModes.MainMenu();
            }
            else if (gameMode == "storyMode")
            {
                currentGameMode = new src.GameModes.StoryMode();
            }
            else if (gameMode == "endlessRun")
            {
                currentGameMode = new src.GameModes.EndlessRunner();
            }
            else if (gameMode == "endlessJump")
            {
                //NOT YET IMPLEMENTED
            }
        }

        private void Init()
        {
            InitOkColors();

            src.GameModes.EndlessRunner.Init(Content);
            src.GameModes.StoryMode.Init(Content);
            src.GameModes.MainMenu.Init(Content);

            src.Items.Barrier.Init(Content);
            src.Items.ColorBlob.Init(Content);
            Player.Init(Content);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Init();
            
            changeGameMode("mainMenu");

            base.Initialize();
        }

        private static void SetBaseColors() {
            primraryColor = okColors.BLACK;
            secondaryColor = okColors.ORANGE;
            backgroundColor = okColors.WHITE;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            ks = Keyboard.GetState();

            if (ks.IsKeyDown(Keys.Escape))
                changeGameMode("mainMenu");

            if (currentGameMode != null) {
                currentGameMode.Update(gameTime);
               
                if (currentGameMode.getPlayer().getOuterColor() != Game1.getPrimraryColor() || currentGameMode.getPlayer().getInnerColor() != Game1.getSecondaryColor())
                {
                    currentGameMode.getPlayer().setOuterColor(Game1.getPrimraryColor());
                    currentGameMode.getPlayer().setInnerColor(Game1.getSecondaryColor());
                }

                if (currentGameMode.getPlayer().getDimensions().getBoundingBox().Y > Game1.getScreenSize().Y ||
                    currentGameMode.getPlayer().getDimensions().getBoundingBox().Y < 0 ||
                    currentGameMode.getPlayer().getDimensions().getBoundingBox().X > Game1.getScreenSize().X ||
                    currentGameMode.getPlayer().getDimensions().getBoundingBox().X < 0)
                {
                    currentGameMode.PlayerOutOfBounds();
                }
            }

            oldks = ks;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            if (Game1.getBackgroundColor() != Game1.okColors.BLACK)
                GraphicsDevice.Clear(Color.Lerp(Game1.getAvailableColors()[Game1.getBackgroundColor()], Color.Black, 0.1f));
            else
                GraphicsDevice.Clear(Color.Lerp(Game1.getAvailableColors()[Game1.getBackgroundColor()], Color.White, 0.1f));

            if (currentGameMode != null)
            {
                currentGameMode.Draw(spriteBatch);
            }

            base.Draw(gameTime);
        }

        #region get/set
        public static Point getScreenSize() {
            return screenSize;
        }
        public static Dictionary<okColors, Color> getAvailableColors()
        {
            return availableColors;
        }
        public static okColors getPrimraryColor()
        {
            return primraryColor;
        }
        public static void setPrimaryColor(okColors _primraryColor)
        {
            primraryColor = _primraryColor;
        }
        public static okColors getSecondaryColor()
        {
            return secondaryColor;
        }
        public static void setSecondaryColor(okColors _secondaryColor)
        {
            secondaryColor = _secondaryColor;
        }
        public static okColors getBackgroundColor()
        {
            return backgroundColor;
        }
        public static void setBackgroundColor(okColors _backgroundColor) {
            backgroundColor = _backgroundColor;
        }
        #endregion
    }
}
