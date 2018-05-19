using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Angeropoly
{
    public class MonopolyDeck : Deck<MonopolyCard>
    {
        //Specifies that the deck must take parameter MonopolyCard
        public MonopolyDeck(IEnumerable<MonopolyCard> cards) : base(cards)
        {

        }

        public new void Shuffle()
        {
            base.Shuffle();

            //This solves the Traveling Get out of Jail Free Card Problem
            int discard = 0;
            for (int i = 0; i < _numItems; i++)
            {
                //I can probably use LINQ for this instead?
                if (_items[i].OutOfDeck)
                {
                    //Swaps the cards that aren't in the deck into the discard pile basically
                    Swap(ref _items[i], ref _items[discard++]);
                }
            }
            _top = discard;
            //What if all the cards are outside of the deck?
            //That would cause big undefined behavior
        }

        //Draw also might need to be overridden
    }

    //This is a really generic version of the Deck class from my Cribbage game
    //It can convert an array of anything into a deck
    public class Deck<T>
    {
        protected static Random rand = new Random();
        
        //CONSTRUCTOR
        public Deck(IEnumerable<T> items, bool isInfinite = true)
        {
            //Stores all items
            _items = items.ToArray();
            _numItems = items.Count();

            //Initially shuffles the deck
            Shuffle();
        }

        //METHODS
        public void Shuffle()
        {
            int swap;
            //Swaps every item with a random position (can be itself)
            //Ensures virtually infinite entropy I think
            for (int i = 0; i < _numItems; i++)
            {
                //Stores which item is about to be swapped to avoid multiple random calls ;)
                swap = rand.Next(_numItems);

                //Swaps items
                Swap(ref _items[i], ref _items[swap]);
            }
            //Resets top of the deck to 0
            _top = 0;
        }

        public T Draw()
        {
            //Returns top item, moves top position down one
            if (_top < _numItems)
            {
                return _items[_top++];
            }
            else
            {
                if (_isInfinite)
                {
                    Shuffle();
                    return _items[_top++];
                }
                else
                {
                    throw new Exception("Out of items error!");
                }
            }
        }
        
        protected static void Swap(ref T left, ref T right)
        {
            //This should probably be a helper function
            T temp = left;
            left = right;
            right = temp;
        }

        public override string ToString()
        {
            //Used for debugging
            var str = new StringBuilder();
            foreach (T item in _items)
            {
                str.AppendLine(item.ToString());
            }
            return str.ToString();
        }
        
        //PRIVATE FIELDS
        //The array of items
        protected T[] _items;
        protected int _numItems;

        //Keeps track of the top of the deck, like a stack
        //Only the deck needs to know where this is
        protected int _top;

        //Will be true if the deck should automatically reshuffle when the items run out
        //Some games break if things work like this (e.g. Cribbage). Shuffle should be called manually
        protected bool _isInfinite;
    }
}
