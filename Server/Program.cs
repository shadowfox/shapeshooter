using System;
using System.Collections.Generic;
using System.Threading;
using Lidgren.Network;
using Lidgren.Network.Xna;
using Common;

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
        private static int serverPort = 42421;
        private static string serverName = "Test Game Server";
        private static bool isShuttingDown = false;

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

            server = new NetServer(config);
            try
            {
                server.Start();
                Thread.Sleep(500);
                log.Info("Server started with status {0} on port {1}", server.Status, serverPort);
            }
            catch (Exception e)
            {
                log.Error("Failed to start server\n" + e);
                isShuttingDown = true;
            }
        }
    }
}
