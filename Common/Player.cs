using System;
using Lidgren.Network;

namespace Common
{
    public class Player
    {
        private long RUI;
        public string ID { get { return NetUtility.ToHexString(this.RUI); } }

        public Player(long RUI)
        {
            this.RUI = RUI;
        }

        public override string ToString()
        {
            return String.Format("Player[ID={0}]", this.ID);
        }
    }
}
