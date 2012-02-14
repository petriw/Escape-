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
using Mogade.WindowsPhone;
using Mogade;

namespace WindowsPhoneGame135
{
    /// <summary>
    /// This is the main type for your game
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

        Texture2D floor;
        Texture2D mainMenu;

        SpriteFont font;

        List<ScoreboardEntry> sblist = new List<ScoreboardEntry>();


        public IMogadeClient Mogade { get; private set; }


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            // Extend battery life under lock.
            InactiveSleepTime = TimeSpan.FromSeconds(1);

            player = new Player(this);
            enemyHandler = new EnemyHandler(this);

            try
            {
                highScore = (int)IsolatedStorageSettings.ApplicationSettings["HighScore"];
            }
            catch
            {
                IsolatedStorageSettings.ApplicationSettings.Add("HighScore", highScore);
            }

            try
            {
                username = (string)IsolatedStorageSettings.ApplicationSettings["Username"];
            }
            catch
            {
                IsolatedStorageSettings.ApplicationSettings.Add("Username", username);
            }

            Mogade = MogadeHelper.CreateInstance();
            Mogade.LogApplicationStart();
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

            LoadLeaderboard(LeaderboardScope.Overall, 1);

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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
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
                TouchCollection touchStateMain = TouchPanel.GetState();
                if (touchStateMain.Count > 0)
                {
                    object stateObj;
                    try
                    {
                        Guide.BeginShowKeyboardInput(PlayerIndex.One, "Enter name", "Please enter a name for the scoreboard (Minimum 3 and no longer than 20 characters)", username, GetText, stateObj = (object)"GetText for Input PlayerOne");
                    }
                    catch (Exception ex)
                    {
                    }

                    inMainMenu = false;
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
            
            TouchCollection touchState = TouchPanel.GetState();
            foreach (TouchLocation t in touchState)
            {
                player.Target = t.Position-(new Vector2(32,32));
            }

            enemyHandler.Update(gameTime, player.Position);
            player.Update(gameTime);

            if (IsGameOver())
            {
                SaveScore();

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
                    try
                    {
                        IsolatedStorageSettings.ApplicationSettings["HighScore"] = highScore;
                    }
                    catch
                    {
                        IsolatedStorageSettings.ApplicationSettings.Add("HighScore", highScore);
                    }
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

            try
            {
                IsolatedStorageSettings.ApplicationSettings["Username"] = username;
            }
            catch
            {
                IsolatedStorageSettings.ApplicationSettings.Add("Username", username);
            }

            nameTyped = true;

            sblist.Clear();
        }

        public void SaveScore()
        {
            var userscore = new Score { Data = "1", Points = player.Score, UserName = username };
            Mogade.SaveScore(MogadeHelper.LeaderboardId(Leaderboards.Main), userscore, ScoreResponseHandler);
        }


        private void ScoreResponseHandler(Response<SavedScore> r)
        {
            //scoreboardRanks = string.Format("Daily Rank: {0}\nWeely Rank: {1}\nOverall Rank {2}", 0, 0, 0);
            if (!r.Success)
            {
                if (Guide.IsVisible == false)
                {
                    //Guide.BeginShowMessageBox("Error", "Unable to retreive data from the server please check your network connection", new string[] { "OK" }, 0, MessageBoxIcon.Error, null, null);
                    return;
                }
            }
            else
            {
                //scoreboardRanks = string.Format("Daily Rank: {0}\nWeely Rank: {1}\nOverall Rank {2}", r.Data.Ranks.Daily, r.Data.Ranks.Weekly, r.Data.Ranks.Overall);
            }

            LoadLeaderboard(LeaderboardScope.Overall, 1);

        }

        private void LoadLeaderboard(LeaderboardScope scope, int page)
        {
            sblist.Clear();
            Mogade.GetLeaderboard(MogadeHelper.LeaderboardId(Leaderboards.Main), scope, page, r => LeaderboardReceived(r));
        }

        private void LeaderboardReceived(Response<LeaderboardScores> response)
        {
            if (!response.Success)
            {
                if (Guide.IsVisible == false)
                {
                    //Guide.BeginShowMessageBox("Error", "Unable to retreive data from the server, please check your network connection", new string[] { "OK" }, 0, MessageBoxIcon.Error, null, null);
                    return;
                }
            }
            else
            {
                for (var i = 0; i < response.Data.Scores.Count; ++i)
                {
                    sblist.Add(new ScoreboardEntry(response.Data.Scores[i]));
                }
            }
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
                spriteBatch.Draw(mainMenu, new Vector2(), Color.White);
                spriteBatch.End();
            }
            else
            {
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
                spriteBatch.Draw(floor, new Vector2(), Color.White);
                enemyHandler.Draw(gameTime, spriteBatch);
                player.Draw(gameTime, spriteBatch);
                spriteBatch.DrawString(font, "Current score: " + player.Score.ToString(), new Vector2(80, 10), Color.White);
                spriteBatch.DrawString(font, "Last score: " + player.LastScore, new Vector2(80, 35), Color.White);

                if (showHighscore)
                {
                    spriteBatch.DrawString(font, "-- Local highscore ----", new Vector2(80, 60), Color.White);
                    spriteBatch.DrawString(font, "  " + highScore.ToString(), new Vector2(80, 85), Color.White);

                    if (sblist.Count > 0)
                    {
                        spriteBatch.DrawString(font, "-- Online highscore ----", new Vector2(80, 110), Color.White);
                        spriteBatch.DrawString(font, "  " + sblist[0].username + " / " + sblist[0].points, new Vector2(80, 135), Color.White);
                    }

                    if (player.LastScore > 0)
                    {
                        if (lastRoundWasHighscore)
                        {
                            spriteBatch.DrawString(font, "  " + "NEW HIGHSCORE!", new Vector2(100, 160), Color.Green);
                        }
                        else
                        {
                            spriteBatch.DrawString(font, "Only " + (highScore - player.LastScore) + " points left, you can do it!", new Vector2(80, 160), Color.Red);
                        }
                    }
                }
                spriteBatch.End();
            }

            base.Draw(gameTime);
        }
    }
}
