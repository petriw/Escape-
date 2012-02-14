using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace WindowsPhoneGame135.GameClasses
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class EnemyHandler : Microsoft.Xna.Framework.DrawableGameComponent
    {
        List<Enemy> enemies = new List<Enemy>();
        bool gameOver = false;

        public bool GameOver
        {
            get { return gameOver; }
            set { gameOver = value; }
        }

        public EnemyHandler(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        public int NumberOfEnemies()
        {
            return enemies.Count;
        }

        public void AddEnemy()
        {
            enemies.Add(new Enemy(Game));
        }
        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        public void Clear()
        {
            gameOver = false;
            enemies.Clear();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime, Vector2 target)
        {
            // TODO: Add your update code here
            Rectangle playerRect = new Rectangle((int)target.X, (int)target.Y, 64, 64);

            foreach (Enemy e in enemies)
            {
                e.Target = target;
                e.Update(gameTime);
                Rectangle enemyRect = new Rectangle((int)e.Position.X+7, (int)e.Position.Y+7, 32-7, 32-7);

                if (playerRect.Intersects(enemyRect))
                {
                    gameOver = true;
                }
            }

            base.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Enemy e in enemies)
            {
                e.Draw(gameTime, spriteBatch);
            }

            base.Draw(gameTime);
        }
    }
}
