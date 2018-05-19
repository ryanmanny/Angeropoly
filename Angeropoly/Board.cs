using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Angeropoly
{
    public class Board
    {
        //EVENT HANDLERS
        public delegate void PlayerMovedHandler(Player sender);
        public delegate void PlayerDiedHandler(Player sender, Player killer);
        private void HandlePlayerMoved(Player sender)
        {
            //Triggered when the player's location changes
            var space = Spaces[sender.Location];
            space.LandedOn(sender);
        }
        private void HandlePlayerDied(Player sender, Player killer)
        {
            //Triggered when the player goes bankrupt
        }

        //CONSTRUCTOR
        public Board()
        {
            _numSpaces = numSpaces;
            _board = new Space[_numSpaces];
            _players = players;

            foreach (var player in _players)
            {
                player.Moved += HandlePlayerMoved;
            }
        }

        //METHODS

        //PROPERTIES
        public int NumSpaces
        {
            get
            {
                return _numSpaces;
            }
        }

        public Space[] Spaces
        {
            get
            {
                return _board;
            }
        }
        
        //PRIVATE FIELDS
        private int _numSpaces;
        private Space[] _board;
        private Player[] _players;
    }
}
