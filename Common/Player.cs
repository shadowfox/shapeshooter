using System;
using Lidgren.Network;

namespace Common
{
    public class Player
    {
        private long _RUI;
        public long RUI { get { return _RUI; } }
        public string ID { get { return NetUtility.ToHexString(this._RUI); } }

        public Player(long RUI)
        {
            this._RUI = RUI;
        }

        public override string ToString()
        {
            return String.Format("Player[ID={0}]", this.ID);
        }
    }
}
