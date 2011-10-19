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
        public bool DirtyPosition;

        public Sprite Sprite = null;

        public Player(long RUI)
        {
            this._RUI = RUI;
        }

        public Player(long RUI, Texture2D texture)
        {
            Console.WriteLine("Player() called including texture " + texture);
            this._RUI = RUI;
            this.DirtyPosition = false;
            if (texture != null)
            {
                this.Sprite = new Sprite();
                this.Sprite.LoadContent(texture);
            }
        }

        public override string ToString()
        {
            return String.Format("Player[ID={0} X={1} Y={2}]", this.ID, this.Position.X, this.Position.Y);
        }
    }
}
