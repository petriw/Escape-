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
using Microsoft.Xna.Framework.Input.Touch;


namespace WindowsPhoneGame135.GameClasses
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Player : Microsoft.Xna.Framework.DrawableGameComponent
    {
        Texture2D fotStepTexture;
        Texture2D texture;
        Vector2 position;
        Vector2 target;

        int addNewFotstepTimer = 200;

        List<FotStep> fotstepList = new List<FotStep>();

        int score = 0;
        int lastScore = 0;

        public int LastScore
        {
            get { return lastScore; }
            set { lastScore = value; }
        }

        public int Score
        {
            get { return score; }
            set { score = value; }
        }

        public Vector2 Target
        {
            get { return target; }
            set { target = value; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Player(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
            position = new Vector2();
            target = new Vector2();
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            texture = Game.Content.Load<Texture2D>("player");
            fotStepTexture = Game.Content.Load<Texture2D>("fotstep");

            base.Initialize();
        }

        public void LoadContent()
        {
            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            addNewFotstepTimer -= gameTime.ElapsedGameTime.Milliseconds;
            if (addNewFotstepTimer < 0)
            {
                Vector2 p = this.Position;
                p.X += texture.Width/2;
                p.Y += texture.Height/2;
                fotstepList.Add(new FotStep(Game) { Position = p });
                addNewFotstepTimer = 1;
            }


            List<int> deleteInactiveFotsteps = new List<int>();
            int index = 0;
            foreach (FotStep step in fotstepList)
            {
                step.Update(gameTime);
                if (!step.IsActive)
                {
                    deleteInactiveFotsteps.Add(index);
                }
                index++;
            }

            foreach (int delInd in deleteInactiveFotsteps)
            {
                fotstepList.RemoveAt(delInd);
            }

            if(position.X < target.X-5)
            {
                position.X += gameTime.ElapsedGameTime.Milliseconds * 0.3f;
            }
            else if(position.X > target.X+5)
            {
                position.X -= gameTime.ElapsedGameTime.Milliseconds * 0.3f;
            }
            else
            {
                position.X = target.X;
            }

            if (position.Y < target.Y-5)
            {
                position.Y += gameTime.ElapsedGameTime.Milliseconds * 0.3f;
            }
            else if (position.Y > target.Y+5)
            {
                position.Y -= gameTime.ElapsedGameTime.Milliseconds * 0.3f;
            }
            else
            {
                position.Y = target.Y;
            }

            if (position.Y >= 480 - texture.Width)
            {
                position.Y = 480 - 64;
            }

            if (position.X >= 800 - 64)
            {
                position.X = 800 - 64;
            }


            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (FotStep step in fotstepList)
            {
                Color c = new Color(255, 255, 255, step.Alpha);

                spriteBatch.Draw(fotStepTexture, step.Position, c);
            }

            spriteBatch.Draw(texture, position, Color.White);

            base.Draw(gameTime);
        }
    }
}
