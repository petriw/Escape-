using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using System.IO.IsolatedStorage;
using WindowsPhoneGame135.GameClasses;

namespace WindowsPhoneGame135
{
    /// <summary>
    /// 
    /// 
    /// Welcome to Escape!
    /// 
    /// This game was programmed live during a presentation,
    /// use it however you like.
    /// 
    /// petriw@gmail.com
    /// 
    /// 
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        Player player;
        EnemyHandler enemyHandler;

        int scoreTimer = 0;
        int highScore = 0;
        bool inMainMenu = true;
        bool nameTyped = false;
        bool showHighscore = true;
        int showHighscoreTimer = 6000;
        bool lastRoundWasHighscore = false;

        int backTimer = 1000;

        Texture2D floor;
        Texture2D mainMenu;

        SpriteFont font;

        PlayerIndex controllingPlayer = PlayerIndex.One;
       
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";


            player = new Player(this);
            enemyHandler = new EnemyHandler(this);

            graphics.PreparingDeviceSettings +=
            new EventHandler<PreparingDeviceSettingsEventArgs>(
            graphics_PreparingDeviceSettings);
        }

        void graphics_PreparingDeviceSettings(object sender,
        PreparingDeviceSettingsEventArgs e)
        {
            foreach (Microsoft.Xna.Framework.Graphics.DisplayMode displayMode
                in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
            {
                if (displayMode.Width == 1920 && displayMode.Height == 1080)
                {
                    e.GraphicsDeviceInformation.PresentationParameters.
                        BackBufferFormat = displayMode.Format;
                    e.GraphicsDeviceInformation.PresentationParameters.
                        BackBufferHeight = displayMode.Height;
                    e.GraphicsDeviceInformation.PresentationParameters.
                        BackBufferWidth = displayMode.Width;
                }
                else if (displayMode.Width == 1280 && displayMode.Height == 720)
                {
                    e.GraphicsDeviceInformation.PresentationParameters.
                        BackBufferFormat = displayMode.Format;
                    e.GraphicsDeviceInformation.PresentationParameters.
                        BackBufferHeight = displayMode.Height;
                    e.GraphicsDeviceInformation.PresentationParameters.
                        BackBufferWidth = displayMode.Width;
                }
                else if (displayMode.Width == 640 && displayMode.Height == 480)
                {
                    e.GraphicsDeviceInformation.PresentationParameters.
                        BackBufferFormat = displayMode.Format;
                    e.GraphicsDeviceInformation.PresentationParameters.
                        BackBufferHeight = displayMode.Height;
                    e.GraphicsDeviceInformation.PresentationParameters.
                        BackBufferWidth = displayMode.Width;
                }
            }
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            enemyHandler.Initialize();
            player.Initialize();

            // Add 3 players to start with
            enemyHandler.AddEnemy();
            enemyHandler.AddEnemy();
            enemyHandler.AddEnemy();


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

            player.LoadContent();
            font = Content.Load<SpriteFont>("SpriteFont1");

            floor = Content.Load<Texture2D>("floor");

            mainMenu = Content.Load<Texture2D>("mainmenu");

            // TODO: use this.Content to load your game content here
        }

        bool IsGameOver()
        {
            return enemyHandler.GameOver;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            backTimer -= gameTime.ElapsedGameTime.Milliseconds;
            if (GamePad.GetState(controllingPlayer).Buttons.Back == ButtonState.Pressed && backTimer <= 0)
            {
                backTimer = 1000;
                if (inMainMenu)
                {
                    this.Exit();
                }
                else
                {
                    enemyHandler.Clear();
                    enemyHandler.AddEnemy();
                    enemyHandler.AddEnemy();
                    enemyHandler.AddEnemy();

                    player.Score = 0;
                    inMainMenu = true;
                }
            }


            if (inMainMenu)
            {
                for (PlayerIndex index = PlayerIndex.One; index <= PlayerIndex.Four; index++)
                {
                    if (GamePad.GetState(index).Buttons.Start == ButtonState.Pressed)
                    {
                        controllingPlayer = index;
                        break;
                    }
                }

                if (GamePad.GetState(controllingPlayer).Buttons.Start == ButtonState.Pressed)
                {
                    inMainMenu = false;
                    nameTyped = true;
                }
                return;
            }

            if (!nameTyped)
                return;

            showHighscoreTimer -= gameTime.ElapsedGameTime.Milliseconds;
            if (showHighscoreTimer < 0)
            {
                showHighscore = false;
            }
            else showHighscore = true;

            scoreTimer += gameTime.ElapsedGameTime.Milliseconds;

            if (scoreTimer >= 1000)
            {
                scoreTimer = 0;
                player.Score += enemyHandler.NumberOfEnemies();
                enemyHandler.AddEnemy();
            }

            

            GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);
            Vector2 modifyPlayerPosition = new Vector2(gamepadState.ThumbSticks.Left.X * (GraphicsDevice.Viewport.Width * 0.001f), -gamepadState.ThumbSticks.Left.Y * (GraphicsDevice.Viewport.Height * 0.001f));
            player.Position += modifyPlayerPosition * gameTime.ElapsedGameTime.Milliseconds * 0.3f;

            enemyHandler.Update(gameTime, player.Position);
            player.Update(gameTime);

            if (IsGameOver())
            {
                showHighscoreTimer = 6000;
                player.LastScore = player.Score;

                enemyHandler.Clear();
                enemyHandler.AddEnemy();
                enemyHandler.AddEnemy();
                enemyHandler.AddEnemy();

                if (highScore < player.Score)
                {
                    lastRoundWasHighscore = true;
                    highScore = player.Score;
                    
                }
                else lastRoundWasHighscore = false;

                player.Score = 0;
            }

            base.Update(gameTime);
        }

        string username = "Player";
        private void GetText(IAsyncResult result)
        {
            try
            {
                string resultString = Guide.EndShowKeyboardInput(result); ;

                if (resultString != null)
                {

                    if (resultString.Length > 20)
                    {
                        resultString = resultString.Remove(20);
                    }

                    if (resultString.Length < 3)
                    {
                        inMainMenu = true;
                        return;
                    }
                    username = resultString;
                }
                else
                {
                    username = "Player";
                    inMainMenu = true;
                }
            }
            catch (Exception ex)
            {
                username = "Player";
                inMainMenu = true;
                return;
            }


            nameTyped = true;

        }



        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            if (inMainMenu || !nameTyped)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(mainMenu, new Rectangle(0,0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                spriteBatch.End();
            }
            else
            {
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
                spriteBatch.Draw(floor, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                enemyHandler.Draw(gameTime, spriteBatch);
                player.Draw(gameTime, spriteBatch);
                spriteBatch.DrawString(font, "Current score: " + player.Score.ToString(), new Vector2(GraphicsDevice.Viewport.TitleSafeArea.Left, GraphicsDevice.Viewport.TitleSafeArea.Top), Color.White);
                spriteBatch.DrawString(font, "Last score: " + player.LastScore, new Vector2(GraphicsDevice.Viewport.TitleSafeArea.Left, GraphicsDevice.Viewport.TitleSafeArea.Top+25), Color.White);

                if (showHighscore)
                {
                    spriteBatch.DrawString(font, "-- Local highscore ----", new Vector2(GraphicsDevice.Viewport.TitleSafeArea.Left, GraphicsDevice.Viewport.TitleSafeArea.Top + 50), Color.White);
                    spriteBatch.DrawString(font, "  " + highScore.ToString(), new Vector2(GraphicsDevice.Viewport.TitleSafeArea.Left, GraphicsDevice.Viewport.TitleSafeArea.Top + 75), Color.White);

                    if (player.LastScore > 0)
                    {
                        if (lastRoundWasHighscore)
                        {
                            spriteBatch.DrawString(font, "  " + "NEW HIGHSCORE!", new Vector2(GraphicsDevice.Viewport.TitleSafeArea.Left, GraphicsDevice.Viewport.TitleSafeArea.Top + 125), Color.Green);
                        }
                        else
                        {
                            spriteBatch.DrawString(font, "Only " + (highScore - player.LastScore) + " points left, you can do it!", new Vector2(GraphicsDevice.Viewport.TitleSafeArea.Left, GraphicsDevice.Viewport.TitleSafeArea.Top + 125), Color.Red);
                        }
                    }
                }
                spriteBatch.End();
            }

            base.Draw(gameTime);
        }
    }
}
