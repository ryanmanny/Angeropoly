using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Angeropoly
{
    //PREVIOUS DESIGN IDEA: I like this, it's much more dynamic. Maybe we can go back
    //A card is just a name and a function
    //public delegate void CardFunction(Player player);

    public abstract class MonopolyCard
    {
        //CONSTRUCTOR
        public MonopolyCard(string name)
        {
            _name = name;
            _outOfDeck = false;
        }

        //METHODS
        //A function is actived when you draw the card
        public abstract void Drawn(Player player);

        //PROPERTIES
        public bool OutOfDeck
        {
            get { return _outOfDeck; }
        }

        //PRIVATE FIELDS
        protected readonly string _name;
        //The Get out of Jail Free card can leave the deck
        protected bool _outOfDeck;
    }

    //What kind of cards are there?

    // 1) Teleport cards 
    // 2) Gain/lose money from bank cards
    // 3) Pay/steal from player cards - can be slightly more complicated, like Chairman of the Board
    // 4) 
}
