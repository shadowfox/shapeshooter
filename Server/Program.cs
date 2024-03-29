﻿using System;
using System.Collections.Generic;
using System.Threading;
using Lidgren.Network;
using Lidgren.Network.Xna;
using Common;
using Microsoft.Xna.Framework;

namespace Server
{
    class Program
    {
        private static Logger log;

        // Server and related config object.
        private static NetServer server;
        private static NetPeerConfiguration config;
        
        // Single reusable message object.
        private static NetIncomingMessage msg;

        // Server information.
        private static string appName = "testgame"; // Must be the same between server and client.
        private static int serverPort = 14242;
        private static string serverName = "Test Game Server";
        private static bool isShuttingDown = false;
        private static double updatesPerSecond = 30.0;
        private static int gameHeight = 400;
        private static int gameWidth = 400;

        private static PlayerManager playerManager;
        private static long RUI;

        private static double now;
        private static double nextSendUpdates;

        /// <summary>
        /// Server entry point
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            log = new Logger();
            setupServer();

            while (!Console.KeyAvailable || Console.ReadKey().Key != ConsoleKey.Escape)
            {
                if (isShuttingDown)
                    break;

                handleIncomingMessages();

                handleOutgoingMessages();

                // sleep to allow other processes to run smoothly
                Thread.Sleep(1);
            }

            server.Shutdown("Server shutting down");
            log.Info("Server shut down");
            Console.ReadLine();
        }

        /// <summary>
        /// Creates, configures and starts the server.
        /// </summary>
        private static void setupServer()
        {
            log.Info("Setting up server...");
            config = new NetPeerConfiguration(appName);
            config.Port = serverPort;
            // Clients will send a DiscoveryRequest to see if they can connect.
            config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
            // ConnectionApproval received once a client runs Connect(); must be explicitly approved or denied.
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

            playerManager = new PlayerManager(gameHeight, gameWidth);
            server = new NetServer(config);
            try
            {
                server.Start();
                nextSendUpdates = NetTime.Now;
                Thread.Sleep(500);
                log.Info("Server started with status {0} on port {1}", server.Status, serverPort);
            }
            catch (Exception e)
            {
                log.Error("Failed to start server\n" + e);
                isShuttingDown = true;
            }
        }

        /// <summary>
        /// Handles messages the server has received from clients.
        /// </summary>
        private static void handleIncomingMessages()
        {
            while ((msg = server.ReadMessage()) != null)
            {
                switch (msg.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        handleDataMessage(msg);
                        break;
                    case NetIncomingMessageType.DiscoveryRequest:
                        handleDiscoveryRequestMessage(msg);
                        break;
                    case NetIncomingMessageType.VerboseDebugMessage:
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.WarningMessage:
                    case NetIncomingMessageType.ErrorMessage:
                        log.Info("Lidgren message: " + msg.ReadString());
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        NetConnectionStatus status = (NetConnectionStatus)msg.ReadByte();
                        if (status == NetConnectionStatus.Connected)
                        {
                            log.Info("====> {0} connected ({1})", Helper.getRemoteTag(msg), msg.ReadString());

                            // Add the new player to the player list.
                            Player player = new Player(msg.SenderConnection.RemoteUniqueIdentifier);
                            player.Position = new Vector2(gameWidth / 2, gameHeight / 2);
                            playerManager.Add(msg.SenderConnection.RemoteUniqueIdentifier, player);

                            // Send them a list of all players in the game.
                            // They will already know their unique identifier.
                            NetOutgoingMessage om = server.CreateMessage();
                            S_PlayerListMessage playerListMessage = new S_PlayerListMessage()
                            {
                                PlayerCount = playerManager.Count,
                                Players = playerManager.Players,
                            };
                            playerListMessage.Write(om);
                            server.SendMessage(om, msg.SenderConnection, NetDeliveryMethod.ReliableUnordered);

                            // Tell all other players about the newly joined player.
                            om = server.CreateMessage();
                            S_PlayerJoinMessage playerJoinMessage = new S_PlayerJoinMessage()
                            {
                                PlayerRUI = player.RUI,
                                Color = player.Color
                            };
                            playerJoinMessage.Write(om);
                            server.SendToAll(om, msg.SenderConnection, NetDeliveryMethod.ReliableUnordered, 0);
                        }
                        else if (status == NetConnectionStatus.Disconnected || status == NetConnectionStatus.Disconnecting)
                        {
                            log.Info("<==== {0} disconnected ({1})", Helper.getRemoteTag(msg), msg.ReadString());
                        }
                        break;
                    case NetIncomingMessageType.ConnectionApproval:
                        // Just aprove everything for now.
                        msg.SenderConnection.Approve();
                        log.Info("Approved connection for {0}", Helper.getRemoteTag(msg));
                        break;
                    default:
                        log.Error("Unhandled message type: " + msg.MessageType);
                        break;
                }
            }
        }

        /// <summary>
        /// Handle a message of our own type
        /// </summary>
        /// <param name="msg"></param>
        private static void handleDataMessage(NetIncomingMessage msg)
        {
            MessageType type = (MessageType)msg.ReadByte();
            RUI = msg.SenderConnection.RemoteUniqueIdentifier;
            switch(type)
            {
                case MessageType.PlayerPosition:
                    C_PlayerPositionMessage playerPositionMessage = new C_PlayerPositionMessage();
                    playerPositionMessage.Read(msg);
                    playerManager.UpdatePlayerPosition(RUI, playerPositionMessage.Position);
                    break;
                default:
                    log.Error("Unknown data message type: {0}", type);
                    break;
            }
        }

        /// <summary>
        /// Respond to a client's discovery request with a discovery response.
        /// Send the client some basic information about the server.
        /// </summary>
        /// <param name="msg"></param>
        private static void handleDiscoveryRequestMessage(NetIncomingMessage msg)
        {
            NetOutgoingMessage responseMessage = server.CreateMessage();

            responseMessage.Write(serverName);
            responseMessage.Write(gameHeight);
            responseMessage.Write(gameWidth);
            server.SendDiscoveryResponse(responseMessage, msg.SenderEndpoint);
        }

        /// <summary>
        /// Prepares and sends all pending outgoing messages.
        /// </summary>
        private static void handleOutgoingMessages()
        {
            // Send updates 30 times per second.
            now = NetTime.Now;
            if (now > nextSendUpdates)
            {
                //log.Info("SENDING UPDATES!");
                // Prepare a player position packet of every player with a 'dirty' position.
                int playerCount;
                List<long> RUIList;
                List<Vector2> positionList;
                playerManager.GetPositions(out playerCount, out RUIList, out positionList);
                S_PlayerPositionMessage playerPositionMessage = new S_PlayerPositionMessage()
                {
                    PlayerCount = playerCount,
                    RUIList = RUIList,
                    PositionList = positionList
                };

                if (playerCount > 0)
                {
                    //Console.WriteLine(playerPositionMessage.ToString());
                    // There are updates to send, so send them.
                    NetOutgoingMessage om = server.CreateMessage();
                    playerPositionMessage.Write(om);

                    // Send to each connection.
                    try
                    {
                        server.SendToAll(om, NetDeliveryMethod.Unreliable);
                    }
                    catch (NetException e)
                    {

                    }
                }

                nextSendUpdates += (1.0 / updatesPerSecond);
            }

            
        }
    }
}
