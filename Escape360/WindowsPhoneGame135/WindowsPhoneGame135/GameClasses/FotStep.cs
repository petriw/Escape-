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
    public class FotStep : Microsoft.Xna.Framework.GameComponent
    {
        Vector2 position;

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        int alpha;

        public int Alpha
        {
            get { return alpha; }
            set { alpha = value; }
        }
        bool isActive;

        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        public FotStep(Game game)
            : base(game)
        {
            position = new Vector2();
            alpha = 255;
            isActive = true;
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

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            alpha -= gameTime.ElapsedGameTime.Milliseconds / 2;
            if (alpha <= 0)
                isActive = false;

            base.Update(gameTime);
        }
    }
}
