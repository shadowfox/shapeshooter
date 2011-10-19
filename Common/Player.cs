using System;
using Lidgren.Network;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Common
{
    public class Player
    {
        private long _RUI;
        public long RUI { get { return _RUI; } }
        public string ID { get { return NetUtility.ToHexString(this._RUI); } }
        public Vector2 Position;
        public Color Color;
        public bool DirtyPosition;

        public static Color[] colors = {
            Color.White, Color.Red, Color.Blue, Color.Yellow, Color.Green, Color.Orange,
            Color.Pink, Color.Purple, Color.Cyan, Color.Tan, Color.Lime, Color.DeepSkyBlue,
            Color.Brown, Color.SkyBlue, Color.SpringGreen, Color.PowderBlue, Color.Maroon
        };

        public Sprite Sprite = null;

        public Player(long RUI)
        {
            this._RUI = RUI;
            this.Color = getRandomColor();
        }

        public Player(long RUI, Texture2D texture)
        {
            this._RUI = RUI;
            this.Color = getRandomColor();
            this.DirtyPosition = false;
            if (texture != null)
            {
                this.Sprite = new Sprite();
                this.Sprite.LoadContent(texture);
            }
        }

        public Player(long RUI, Texture2D texture, Color color)
        {
            this._RUI = RUI;
            this.Color = color;
            this.DirtyPosition = false;
            if (texture != null)
            {
                this.Sprite = new Sprite();
                this.Sprite.LoadContent(texture);
            }
        }

        public static Color getRandomColor()
        {
            Random random = new Random();
            return colors[random.Next(0, colors.Length - 1)];
        }

        public override string ToString()
        {
            return String.Format("Player[ID={0} X={1} Y={2}]", this.ID, this.Position.X, this.Position.Y);
        }
    }
}
