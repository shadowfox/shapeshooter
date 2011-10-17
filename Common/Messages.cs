using System;
using System.Collections.Generic;
using Lidgren.Network;
using Lidgren.Network.Xna;
using Microsoft.Xna.Framework;

namespace Common
{
    public enum MessageType : byte
    {
        None = 0,
        Special = 1,
        PlayerList = 2,
    }

    public struct S_PlayerListMessage : ISendable
    {
        public MessageType MessageType { get { return MessageType.PlayerList; } }
        public int PlayerCount;
        public Dictionary<long, Player> Players;

        public void Read(NetIncomingMessage msg)
        {
            this.PlayerCount = msg.ReadInt32();
            this.Players = new Dictionary<long, Player>();
            for (int i = 0; i < this.PlayerCount; i++)
            {
                long RUI = msg.ReadInt64();
                this.Players.Add(RUI, new Player(RUI));
            }
        }

        public void Write(NetOutgoingMessage msg)
        {
            msg.Write((byte)this.MessageType);
            msg.Write(this.PlayerCount);
            foreach (KeyValuePair<long, Player> kvp in Players)
            {
                // RUI
                msg.Write(kvp.Key);
            }
        }
    }

    internal interface ISendable
    {
        MessageType MessageType { get; }
        void Read(NetIncomingMessage msg);
        void Write(NetOutgoingMessage msg);
    }
}
