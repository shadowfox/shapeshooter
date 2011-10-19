using System;
using System.Collections.Generic;
using Lidgren.Network;
using Lidgren.Network.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Common
{
    public enum MessageType : byte
    {
        None = 0,
        Special = 1,
        PlayerList = 2,
        PlayerJoin = 3,
        PlayerPosition = 4,
    }

    public struct S_PlayerListMessage : ISendable
    {
        public MessageType MessageType { get { return MessageType.PlayerList; } }
        public int PlayerCount;
        public Dictionary<long, Player> Players;

        public Texture2D playerTexture;
        public Color Color;

        public void Read(NetIncomingMessage msg)
        {
            this.PlayerCount = msg.ReadInt32();
            this.Players = new Dictionary<long, Player>();
            for (int i = 0; i < this.PlayerCount; i++)
            {
                long RUI = msg.ReadInt64();
                Vector2 position = msg.ReadVector2();
                Player player = new Player(RUI, playerTexture, new Color(msg.ReadByte(), msg.ReadByte(), msg.ReadByte()));
                player.Position = position;
                this.Players.Add(RUI, player);
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
                msg.Write(kvp.Value.Position);
                msg.Write(kvp.Value.Color.R);
                msg.Write(kvp.Value.Color.B);
                msg.Write(kvp.Value.Color.G);
            }
        }
    }

    public struct S_PlayerJoinMessage : ISendable
    {
        public MessageType MessageType { get { return MessageType.PlayerJoin; } }
        public long PlayerRUI;
        public Color Color;

        public void Read(NetIncomingMessage msg)
        {
            this.PlayerRUI = msg.ReadInt64();
            this.Color = new Color(msg.ReadByte(), msg.ReadByte(), msg.ReadByte());
        }

        public void Write(NetOutgoingMessage msg)
        {
            msg.Write((byte)this.MessageType);
            msg.Write(this.PlayerRUI);
            msg.Write(this.Color.R);
            msg.Write(this.Color.B);
            msg.Write(this.Color.G);
        }
    }

    public struct C_PlayerPositionMessage : ISendable
    {
        public MessageType MessageType { get { return MessageType.PlayerPosition; } }
        public Vector2 Position;

        public void Read(NetIncomingMessage msg)
        {
            this.Position = XNAExtensions.ReadVector2(msg);
        }

        public void Write(NetOutgoingMessage msg)
        {
            msg.Write((byte)this.MessageType);
            XNAExtensions.Write(msg, this.Position);
        }
    }

    public struct S_PlayerPositionMessage : ISendable
    {
        public MessageType MessageType { get { return MessageType.PlayerPosition; } }
        public int PlayerCount;
        public List<long> RUIList;
        public List<Vector2> PositionList;

        public void Read(NetIncomingMessage msg)
        {
            this.PlayerCount = msg.ReadInt32();
            for (int i = 0; i < this.PlayerCount; i++)
            {
                RUIList.Add(msg.ReadInt64());
                PositionList.Add(XNAExtensions.ReadVector2(msg));
            }
        }

        public void Write(NetOutgoingMessage msg)
        {
            msg.Write((byte)this.MessageType);
            msg.Write(this.PlayerCount);
            for (int i = 0; i < this.PlayerCount; i++)
            {
                msg.Write(this.RUIList[i]);
                XNAExtensions.Write(msg, this.PositionList[i]);
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
