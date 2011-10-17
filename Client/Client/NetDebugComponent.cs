using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Lidgren.Network;


namespace Client
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class NetDebugComponent : DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private NetClient client;
        public Vector2 position;
        public bool Visible = false;

        public NetDebugComponent(Game game, ref NetClient client) : base(game)
        {
            this.client = client;
            this.position = new Vector2(5, 5);
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
                spriteBatch.DrawString(Resources.debugFont, getNetworkInfoString(), position, Color.Gray);
                spriteBatch.End();
            }
            base.Draw(gameTime);
        }

        /// <summary>
        /// Build together a network statistics string from the client object.
        /// </summary>
        /// <returns>The completed string</returns>
        private string getNetworkInfoString()
        {
            String stats = String.Format("STATUS: {0} ({1}) {2}\n{3}",
                client.Status, client.ConnectionStatus, client.ServerConnection, client.Statistics);
            return stats;
        }
    }
}
