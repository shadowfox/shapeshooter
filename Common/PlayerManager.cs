using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Common
{
    /// <summary>
    /// Responsible for keeping a dictionary of players mapped to their remote unique identifiers.
    /// </summary>
    public class PlayerManager
    {
        Logger log;
        private Dictionary<long, Player> players;
        private Player tempPlayer;

        public Dictionary<long, Player> Players
        {
            get { return this.players; }
        }

        public int Count
        {
            get { return this.players.Count; }
        }

        public bool isEmpty
        {
            get { return this.players.Count < 1; }
        }

        public PlayerManager()
        {
            // Initialise the player list.
            players = new Dictionary<long, Player>();

            log = new Logger();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Player player in players.Values)
            {
                player.Sprite.Draw(spriteBatch, player.Position);
            }
        }

        /// <summary>
        /// Update a player's position given their RUI and the new position
        /// </summary>
        /// <param name="RUI"></param>
        /// <param name="position"></param>
        public void UpdatePlayerPosition(long RUI, Vector2 position)
        {
            if (this.players.TryGetValue(RUI, out tempPlayer))
            {
                tempPlayer.Position = position;
                tempPlayer.DirtyPosition = true;
                //log.Info("Position of {0} is now {1}", Helper.getRUIHex(RUI), position);
            }
            else
            {
                log.Error("Could not find player with RUI {0}", RUI);
            }
        }

        public void GetPositions(out int playerCount, out List<long> RUIList, out List<Vector2> PositionList)
        {
            playerCount = 0;
            RUIList = new List<long>();
            PositionList = new List<Vector2>();

            foreach (KeyValuePair<long, Player> kvp in this.players)
            {
                if (kvp.Value.DirtyPosition)
                {
                    // Reset the dirty flag.
                    kvp.Value.DirtyPosition = false;
                    playerCount++;
                    RUIList.Add(kvp.Key);
                    PositionList.Add(kvp.Value.Position);
                }
            }
        }

        public void UpdatePositions(int playerCount, List<long> RUIList, List<Vector2> positionList)
        {
            if (RUIList.Count != positionList.Count)
            {
                log.Error("RUI LIST AND PLAYER POSITION LIST ARE OUT OF SYNC !!!!!!!!!!");
            }
            else
            {
                for (int i = 0; i < playerCount; i++)
                {
                    if (this.players.TryGetValue(RUIList[i], out tempPlayer))
                    {
                        tempPlayer.Position = positionList[i];
                    }
                    else
                    {
                        log.Error("Could not find player with RUI {0}", RUIList[i]);
                    }
                }
            }
        }

        #region Public dictionary access methods
        /// <summary>
        /// Public interface for adding objects.
        /// </summary>
        /// <param name="RUI">The key for the players dictionary</param>
        /// <param name="player">The player object</param>

        public void Add(long RUI, Player player)
        {
            players.Add(RUI, player);
        }

        /// <summary>
        /// Public interface for removing objects.
        /// </summary>
        /// <param name="RUI">The key of the object</param>
        public void Remove(long RUI)
        {
            players.Remove(RUI);
        }

        /// <summary>
        /// Updates the master player list with new items.
        /// Only new players are added; players with existing keys will be ignored.
        /// </summary>
        /// <param name="newPlayers">The dictonary of new players to add</param>
        public void AddAll(Dictionary<long, Player> newPlayers)
        {
            foreach (KeyValuePair<long, Player> kvp in newPlayers)
            {
                if (!this.players.ContainsKey(kvp.Key))
                {
                    this.players.Add(kvp.Key, kvp.Value);
                }
            }
        }

        /// <summary>
        /// Replaces the currently players dictionary with a new empty one
        /// </summary>
        public void Clear()
        {
            this.players = new Dictionary<long, Player>();
        }
        #endregion

        public override string ToString()
        {
            String str = String.Format("{0} Players:", this.Count);
            foreach (Player player in players.Values)
            {
                str += "\n" + player.ToString();
            }
            return str;
        }
    }
}
