using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Lidgren.Network;
using Lidgren.Network.Xna;
using Common;

namespace Client
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Logger log;

        // Networking related members
        private NetClient client;
        public NetClient Client
        {
            get { return this.client; }
        }
        private NetPeerConfiguration config;
        private NetIncomingMessage msg;
        private PlayerManager playerManager;
        public PlayerManager PlayerManager
        {
            get { return this.playerManager; }
        }

        // The player object belonging to this client.
        private Player localPlayer = null;
        public Player LocalPlayer
        {
            get { return this.localPlayer; }
        }

        // Network client config
        private string appName = "testgame";
        private string serverAddress = "localhost";
        private int serverPort = 42421;
        private string serverName = "";

        public Game()
        {
            log = new Logger();
            log.Info("Setting up game...");
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            config = new NetPeerConfiguration(appName);
            config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);

            client = new NetClient(config);

            // Start with an empty player manager.
            playerManager = new PlayerManager();

            NetDebugComponent netDebugComponent = new NetDebugComponent(this, ref client);
            this.Components.Add(netDebugComponent);
            netDebugComponent.Visible = true;

            PlayerListDebugComponent playerListDebugComponent = new PlayerListDebugComponent(this, ref playerManager);
            this.Components.Add(playerListDebugComponent);
            playerListDebugComponent.Visible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            log.Info("Initialising...");
            base.Initialize();

            try
            {
                client.Start();
                log.Info("Started network client with status: {0}", client.Status);
                log.Info("Sending discovery request to {0}:{1}", serverAddress, serverPort);
                client.DiscoverKnownPeer(serverAddress, serverPort);
            }
            catch (Exception e)
            {
                log.Error("Failed to start client. Exception:\n{0}", e);
            }
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Resources.Load(Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            handleMessages();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        /// <summary>
        /// Main client message receive loop.
        /// </summary>
        private void handleMessages()
        {
            while ((msg = client.ReadMessage()) != null)
            {
                switch (msg.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        handleDataMessage(msg);
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        NetConnectionStatus status = (NetConnectionStatus)msg.ReadByte();
                        log.Info("StatusChanged: {0} ({1})", status, msg.ReadString());
                        if (status == NetConnectionStatus.Disconnected || status == NetConnectionStatus.Disconnecting)
                        {
                            clearData();
                        }
                        break;
                    case NetIncomingMessageType.DiscoveryResponse:
                        handleDiscoveryResponseMessage(msg);
                        break;
                    case NetIncomingMessageType.VerboseDebugMessage:
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.WarningMessage:
                    case NetIncomingMessageType.ErrorMessage:
                        log.Info("Message from server: {0}", msg);
                        break;
                    default:
                        log.Info("Unhandled message type: {0}", msg.MessageType);
                        break;
                }
                client.Recycle(msg);
            }
        }

        /// <summary>
        /// Handle a discovery response sent to us from the server in response to our discovery request.
        /// Now its time to attempt a connection.
        /// </summary>
        /// <param name="msg"></param>
        private void handleDiscoveryResponseMessage(NetIncomingMessage msg)
        {
            try
            {
                log.Info("Received a discovery response from {0}; attempting to connect...", msg.SenderEndpoint);
                if (client.Connect(msg.SenderEndpoint) != null)
                {
                    serverName = msg.ReadString();
                    log.Info("Connection with '{0}' at {1} established!", serverName, msg.SenderEndpoint);
                }
                else
                {
                    log.Error("Failed to connect to server at {0}", msg.SenderEndpoint);
                }
            }
            catch (Exception e)
            {
                log.Error("Failed to connect to server at {0}. Exception: {1}", msg.SenderEndpoint, e);
            }
        }

        private void handleDataMessage(NetIncomingMessage msg)
        {
            log.Info("Got data message: {0}", msg);

            MessageType type = (MessageType)msg.ReadByte();
            log.Info("Message is of type {0}", type);
            switch (type)
            {
                case MessageType.PlayerJoin:
                    // A new player has joined, so add it to the local dictionary.
                    S_PlayerJoinMessage playerJoinMessage = new S_PlayerJoinMessage();
                    playerJoinMessage.Read(msg);
                    PlayerManager.Add(playerJoinMessage.PlayerRUI, new Player(playerJoinMessage.PlayerRUI, Resources.playerTexture));
                    break;
                case MessageType.PlayerList:
                    S_PlayerListMessage playerListMessage = new S_PlayerListMessage();
                    playerListMessage.playerTexture = Resources.playerTexture;
                    playerListMessage.Read(msg);
                    log.Info("playersCount: {0}", playerListMessage.PlayerCount);
                    playerManager.AddAll(playerListMessage.Players);
                    // Find and set the local player from the players list.
                    if (!playerManager.Players.TryGetValue(client.UniqueIdentifier, out localPlayer))
                    {
                        // This should never happen.
                        log.Error("Unable to find local player in player list");
                        client.Disconnect("Error");
                    }
                    localPlayer.Sprite = new Sprite();
                    break;
                default:
                    log.Error("Unknown message type: {0}", type);
                    break;
            }
        }

        /// <summary>
        /// Clear all of the client's data so things are clean for a reconnect.
        /// </summary>
        private void clearData()
        {
            localPlayer = null;
            playerManager.Clear();
        }
    }
}
