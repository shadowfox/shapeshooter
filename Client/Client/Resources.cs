using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;


namespace Client
{
    public static class Resources
    {
        private static ContentManager cm;

        public static Texture2D player;
        public static SpriteFont debugFont;

        public static void Load(ContentManager contentManager)
        {
            cm = contentManager;

            player = cm.Load<Texture2D>("Player");
            debugFont = cm.Load<SpriteFont>("Debug");
        }
    }
}
