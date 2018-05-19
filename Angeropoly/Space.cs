using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Angeropoly
{
    public abstract class Space
    {
        //What is a space, exactly?
        //This program is just a musing on philosopoly, there's no code
        
        public abstract void LandedOn(Player player);
    }

    public abstract class OwnableSpace : Space
    {
        //CONSTRUCTOR
        public OwnableSpace(string name, int price)
        {
            //These are readonly and can never be changed
            _name = name;
            _price = price;

            //Ownable spaces all start with no owner
            _owner = null;
            //Mortgaged should initially be false, UNLESS it makes more sense as a nullable
            _mortgaged = false;
        }

        //METHODS
        protected abstract int CalculateRent();
                             //Add something here like charge rent to player, put that in the behavior
        
        //PROPERTIES
        public string Name
        {
            get
            {
                return _name;
            }
        }
        public int Price
        {
            get
            {
                return _price;
            }
        }
        public int Rent
        {
            get
            {
                //For all implementations Rent should call this function
                return CalculateRent();
            }
        }
        public int MortgageValue
        {
            get
            {
                //Mortgage value is always half the price
                //This is also the auction starting bid
                return Price / 2;
            }
        }
        public int UnmortgageCost
        {
            get
            {
                //You have to pay 10% extra on top of the mortgage value to unmortgage
                return (int) (MortgageValue * 1.1);
            }
        }
        public bool IsMortgaged
        {
            get
            {
                return _mortgaged;
            }
            set
            {
                _mortgaged = value;
            }
        }
        public Player Owner
        {
            get
            {
                return _owner;
            }
            protected set
            {
                if (!_mortgaged)
                {
                    _owner = value;
                }
                else
                {
                    throw new ArgumentException();
                }
            }
        }

        //PRIVATE FIELDS
        protected readonly string _name;
        protected readonly int _price;
        protected bool _mortgaged;
        protected Player _owner;
    }

    public class PropertySpace : OwnableSpace
    {
        //This is a space like Boardwalk Avenue that dynamically calculates rent based on number of houses
        //If the owner ALSO owns the adjacent colored properties, rent is doubled

        //ENUM
        public enum ColorType
        {
            Purple, LightBlue, Pink, Orange, Red, Yellow, Green, DarkBlue
        }

        //CONSTRUCTOR
        public PropertySpace(string name, int price, ColorType color, int housePrice, Dictionary<int, int> rents, int maxNumHouses) : base(name, price)
        {
            //These fields are readonly and cannot be modified later
            
            //Color is used for grouping
            _color = color;
            _housePrice = housePrice;
            _rents = rents;

            //Properties start with no houses
            _numHouses = 0;
            _maxNumHouses = maxNumHouses;
        }

        //METHODS
        protected override int CalculateRent()
        {
            int rent = _rents[Houses];

            if (CanBuyHouse)
            {
                //Rent is doubled if the owner can buy a house on the property group
                rent *= 2;
            }

            return rent;
        }

        public override void LandedOn(Player player)
        {
            if (ReferenceEquals(Owner, null))
            {
                //These questions must be answered
                int price;
                Player buyer;
                
                if (player.AskToBuy(this))
                {
                    buyer = player;
                    price = Price;
                }
                else
                {
                    int auctionResult;
                    //Price becomes the auction outcome, player is the starting bidder
                    buyer = Auction(player, out auctionResult);
                    price = auctionResult;
                }
                
                //The buyer pays the bank the allotted price
                buyer.GiveMoney(price, null);
                buyer.Own(this);
                Owner = buyer;
            }
            else
            {
                if (!IsMortgaged)
                {
                    //CHARGE RENT
                }
            }
        }

        public Player Auction(Player firstBidder, out int auctionResult)
        {
            
        }

        //PROPERTIES
        public new Player Owner
        {
            //This Property might hide the old one BUT it's just a slightly different interface to the same variable
            get
            {
                return _owner;
            }
            protected set
            {
                if (Houses == 0)
                {
                    //Owners can only change when there are no houses associated
                    base.Owner = value;
                }
                else
                {
                    throw new ArgumentException();
                }
            }
        }

        public int Houses
        {
            get
            {
                return _numHouses;
            }
            set
            {
                _numHouses = value;
                if (Houses > MaxNumHouses)
                {
                    //This shouldn't happen anyway, but it would break the dictionary so I want to enforce it
                    Houses = MaxNumHouses;
                }
            }
        }
        public int HousePrice
        {
            get
            {
                return _housePrice;
            }
        }
        public int HouseSalePrice
        {
            get
            {
                //Abandon hope all ye who enter here
                return HousePrice / 2;
            }
        }
        public int MaxNumHouses
        {
            get
            {
                return _maxNumHouses;
            }
        }
        public bool CanBuyHouse
        {
            get
            {
                //Will return true if the owner owns all of the properties of this color
                throw new NotImplementedException();
            }
        }
        public ColorType Color
        {
            get
            {
                return _color;
            }
        }
        
        //PRIVATE FIELDS
        //These fields are exclusive to properties - since they are the only ones with houses associated
        private readonly Dictionary<int, int> _rents;
        private readonly int _housePrice;
        private readonly ColorType _color;
        private readonly int _maxNumHouses;

        private int _numHouses;
    }

    public class UtilitySpace : OwnableSpace
    {
        //CONSTRUCTOR
        public UtilitySpace(string name, int price) : base(name, price)
        {
            //In the base game all utilities cost the same, but that should be customizable here
        }

        protected override int CalculateRent()
        {
            //DEFAULT UTILITY RENT IS CALCULATED LIKE SO
            //4X ROLL DOLLARS TO OWNER
            //10X ROLL DOLLARS TO OWNER IF BOTH UTILITIES OWNED BY SAME OWNER
            //If a card brings you to a utility, the player must roll again and always pay 10X

            throw new NotImplementedException();

            //Same sort of thing needs to happen here as checking if the owner also owns properties of the same color
        }

        //METHODS
        public override void LandedOn(Player player)
        {
            //This needs different behavior if the player teleported there vs. landing
            throw new NotImplementedException();
        }
    } 

    public class RailroadSpace : OwnableSpace
    {
        public RailroadSpace(string name, int price) : base(name, price)
        {

        }
        
        public override void LandedOn(Player player)
        {
            throw new NotImplementedException();
        }

        protected override int CalculateRent()
        {
            //This function will need to utilize the owner field
            //to calculate the rent
            throw new NotImplementedException();
        }
    }

    public class CardSpace : Space
    {
        //Draws a card from a specified deck
        public override void LandedOn(Player player)
        {
            throw new NotImplementedException();
        }
    }
}
