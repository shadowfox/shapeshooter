using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Lidgren.Network;
using Common;


namespace Client
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class PlayerListDebugComponent : DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private PlayerManager playerManager;
        public Vector2 position;
        public bool Visible = false;

        public PlayerListDebugComponent(Game game, ref PlayerManager playerManager)
            : base(game)
        {
            this.playerManager = playerManager;
            this.position = new Vector2(5, 110);
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            if (this.Visible)
            {
                spriteBatch.Begin();
                spriteBatch.DrawString(Resources.debugFont, getPlayerListString(), position, Color.Gray);
                spriteBatch.End();
            }
            base.Draw(gameTime);
        }

        /// <summary>
        /// Returns a list of players as a string
        /// </summary>
        /// <returns>The completed string</returns>
        private string getPlayerListString()
        {
            return this.playerManager.ToString();
        }
    }
}
