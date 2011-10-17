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
        #endregion

        public override string ToString()
        {
            String str = String.Format("{0} Players:", this.Count);
            foreach (Player player in players.Values)
            {
                str += player.ToString() + "\n";
            }
            return str;
        }
    }
}
