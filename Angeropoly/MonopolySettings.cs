using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Angeropoly
{
    //This should be instantiated based on an XML file
    //Defaults should be stored there
    //The Game() class should be instantiated with this object as its argument
    public class MonopolySettings
    {
        //What should be customizable? Virtually everything
        //Perhaps this should be like a database... maybe too complicated
        public MonopolySettings()
        {
            //Directly initializes a default Monopoly game
            //This function should be removed, replaced with a default XML file eventually
            //The non-default settings should ideally just override the default settings
            //So maybe this default constructor could be useful
        }

        public MonopolySettings(System.IO.StreamReader reader)
        {
            //TODO: Finish this function

            throw new NotImplementedException();
            
            using (var xmlreader = new System.Xml.XmlTextReader(reader))
            {
                while (xmlreader.Read())
                {
                    switch (xmlreader.NodeType)
                    {
                        case System.Xml.XmlNodeType.Element:
                            //If it's the board, keep reading
                            break;
                        case System.Xml.XmlNodeType.Attribute:
                            break;
                    }
                }
            }
        }

        //The actual settings!
        //These are properties and not readonly BECAUSE if I ever implement custom behaviors 
        //I like the idea of dynamically changing, volatile behavior

        //Things which should be customizable

        //0) Game settings. Specific to this round
        public AIPlayer.Difficulty AIDifficulty { get; private set; }

        //1) Global settings like railroad/utility rent dictionary, GO amount, starting money, max # houses
        public Dictionary<int, int> RailroadRent { get; private set; }
        public Dictionary<int, int> UtilityRent { get; private set; }
        //The maximum number of houses that can be associated with one property. 5 in Reg, 6 in MegaMonopoly
        public int MaxHouseNumber { get; private set; }

        public int GoAmount { get; private set; }
        public int StartingMoney { get; private set; }
        
        //2) The board and its spaces -> (names, prices, colors, individual rent dictionary)
        //   (the board should be constructed based on this list of spaces set here)
        public List<Space> Spaces { get; private set; }

        //3) The decks of cards 
        public List<MonopolyDeck> Decks { get; private set; }

        //4) The shape of the board should probably be defined here too, I guess
        public int BoardLongEdgeLength { get; private set; } //Minimum 2
        public int BoardShortEdgeLength { get; private set; } //Minimum 2
        //public int BoardNumSpaces { get; private set; }
        //I would rather implement this in a Haystack type way for more interesting board shapes - no rectangles
        
        public struct SpecialHouse
        {
            //This struct is only necessary for knowing if you're out of hotels, etc.
            //I don't think this is generic enough. I don't like it
            public string Name; //Hotel -> Skyscraper
            public int HouseNum; //How many houses it is equivalent to = 5 -> 6
            public int MaxNum; //Max number = 12
        }

        //5) Equipment limits -> Some of these rules are dubious
        public int MaxHousesInventory { get; private set; } //Most rules enforce max houses, bank can run out
        public List<SpecialHouse> SpecialHouses { get; private set; }
        //public List<Tuple<int, int>> BankStartingInventory; //Way too complicated, unnecessary
        //public int BankStartingMoney {get; private set; } //Most rules allow infinite bank money

        //6) Pointers to graphics in the file hierarchy should also be in here
    }
}
