using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Common
{
    /// <summary>
    /// Responsible for keeping a dictionary of players mapped to their remote unique identifiers.
    /// </summary>
    public class PlayerManager
    {
        private Dictionary<long, Player> players;

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
