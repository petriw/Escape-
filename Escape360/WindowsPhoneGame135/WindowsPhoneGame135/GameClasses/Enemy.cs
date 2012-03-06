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
    public class Enemy : Microsoft.Xna.Framework.DrawableGameComponent
    {
        Texture2D texture;
        Vector2 position;
        Vector2 target;

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

        public Enemy(Game game)
            : base(game)
        {
            

            Initialize();
            LoadContent();
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

        public void LoadContent()
        {
            // TODO: Construct any child components here
            Random rnd = new Random();

            int posX = rnd.Next(-1000 - GraphicsDevice.Viewport.Width, 1000 + GraphicsDevice.Viewport.Width);
            int posY = rnd.Next(-1000 - GraphicsDevice.Viewport.Height, 1000 + GraphicsDevice.Viewport.Height);

            if (posX >= 0 && posX <= GraphicsDevice.Viewport.Width)
            {
                posX = -32;
            }

            if (posY >= 0 && posY <= GraphicsDevice.Viewport.Height)
            {
                posY = -32;
            }

            position = new Vector2(posX, posY);
            target = new Vector2();

            texture = Game.Content.Load<Texture2D>("ghost");
            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            
            if(position.X < target.X-5)
            {
                position.X += gameTime.ElapsedGameTime.Milliseconds * 0.03f * (GraphicsDevice.Viewport.Width * 0.001f);
            }
            else if(position.X > target.X+5)
            {
                position.X -= gameTime.ElapsedGameTime.Milliseconds * 0.03f * (GraphicsDevice.Viewport.Width * 0.001f);
            }
            else
            {
                //position.X = target.X;
            }

            if (position.Y < target.Y-5)
            {
                position.Y += gameTime.ElapsedGameTime.Milliseconds * 0.03f * (GraphicsDevice.Viewport.Height * 0.001f);
            }
            else if (position.Y > target.Y+5)
            {
                position.Y -= gameTime.ElapsedGameTime.Milliseconds * 0.03f * (GraphicsDevice.Viewport.Height * 0.001f);
            }
            else
            {
                //position.Y = target.Y;
            }


            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, Convert.ToInt32(0.04f * GraphicsDevice.Viewport.Width), Convert.ToInt32(0.066666f * GraphicsDevice.Viewport.Height)), Color.White);

            base.Draw(gameTime);
        }
    }
}
