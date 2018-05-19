using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Angeropoly
{
    //Players should only have knowledge of other players
    //They should not trigger any board or card events
    //The board should handle all of that

    public class Player
    {
        //CONSTRUCTOR
        public Player(MonopolySettings settings) //MonopolySettings.DefaultStartingMoney
        {
            //Players should start at the beginning (GO space)
            _location = 0;

            //May be changed independently for handicaps or something
            _money = settings.StartingMoney;

            //User starts with no spaces owned
            _owned = null;
        }

        //METHODS
        public void Goto(int location)
        {
            //Moves the player
            //Only the board should call this function
            Location = location;
        }

        public bool AskToBuy(OwnableSpace space)
        {
            bool wantToBuy = true;
            throw new NotImplementedException();
            if (wantToBuy)
            {
                //Player accepts to buy
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Own(OwnableSpace space)
        {
            Properties.Add(space);
        }

        public void Disown(OwnableSpace space)
        {
            if (!Properties.Remove(space))
            {
                throw new ArgumentException();
            }
        }

        public void GetMoney(int money)
        {
            Money += money;
        }

        public void GiveMoney(int money, Player creditor)
        {
            //Creditor can be null if you're paying the board for something

            Money -= money;
            creditor?.GetMoney(money);
            
            if (Money < 0)
            {
                //Handle running out of money
                TryEscapeBankruptcy(creditor);
            }
        }

        protected void TryEscapeBankruptcy(Player creditor)
        {
            //Overdraft alert 
            //Game cannot continue until you either get back in the black
            //Or declare bankruptcy
            if (NetWorth < 0)
            {
                //Even by selling all houses and mortgaging all properties,
                //you can't get back in the black
                if (!ForceTrade(creditor))
                {
                    //Your only escape is to trade something to an opponent
                    GoBankrupt(creditor);
                }
            }
            else
            {
                //You have the ability to escape bankruptcy by mortgaging or selling houses
                ForceEarnMoney();
            }
        }

        protected virtual void ForceEarnMoney()
        {
            //The player is on his last legs. He must mortgage or sell property to live

            //I don't really have a clue what to do here
            //This seems like more of a UI trigger since it requires player input...
            throw new NotImplementedException();

            while (Bankrupt)
            {
                //Sell something
            }
        }

        protected virtual bool ForceTrade(Player creditor)
        {
            //Returns true if player was able to trade, false otherwise
            //You can trade with anybody except for the creditor. That breaks the rules

            throw new NotImplementedException();
            //Again, not really sure on implementation. It requires player input

            while (Bankrupt)
            {

            }
        }

        protected void GoBankrupt(Player killer)
        {
            //He tried all he could but could not escape bankruptcy
            NotifyPlayerDied(killer);
        }

        protected void Mortgage(PropertySpace space)
        {
            if (!space.IsMortgaged)
            {
                //Set property to mortgage
                space.IsMortgaged = true;

                //Give player the money
                GetMoney(space.MortgageValue);
            }
        }

        protected void Unmortgage(PropertySpace space)
        {
            if (space.IsMortgaged)
            {
                //Set property to unmortgaged
                space.IsMortgaged = false;

                //Player pays back the money
                GiveMoney(space.UnmortgageCost, null);
                //This can trigger bankruptcy escape so be careful!
                //Maybe include a warning...
            }
        }
        
        //EVENTS
        public event Board.PlayerMovedHandler Moved;
        public event Board.PlayerDiedHandler Died;
        public void NotifyPlayerMoved()
        {
            if (!ReferenceEquals(Moved, null))
            {
                Moved(this);
            }
        }
        public void NotifyPlayerDied(Player killer)
        {
            if (!ReferenceEquals(Died, null))
            {
                Died(this, killer);
            }
        }

        //PROPERTIES
        public int Money
        {
            get
            {
                return _money;
            }
            set
            {
                _money = value;
            }
        }
        public int PropertyValue
        {
            get
            {
                int total = 0;

                foreach (var p in Properties)
                {
                    //Totals up all the money you can make from selling houses
                    //and mortgaging properties

                    if (p.GetType() == typeof(PropertySpace))
                    {
                        //Only properties have associated House value
                        var property = p as PropertySpace;
                        //SELL A HOUSE! SELL A HOUSE!
                        total += property.Houses * property.HouseSalePrice;
                    }
                    total += p.MortgageValue;
                }

                return total;
            }
        }
        public int NetWorth
        {
            get
            {
                return Money + PropertyValue;
            }
        }
        public int Location
        {
            get
            {
                return _location;
            }
            set
            {
                _location = value;
                //Notify the board that the player has changed position
                NotifyPlayerMoved();
            }
        }
        public bool Bankrupt
        {
            get
            {
                return (NetWorth < 0);
            }
        }

        public List<OwnableSpace> Properties
        {
            get
            {
                return _owned;
            }
            set
            {
                _owned = value;
            }
        } //huh

        public Player NextPlayer
        {
            get
            {
                return _nextPlayer;
            }
        }

        //PRIVATE FIELDS
        protected int _money;
        protected int _location;
        protected List<OwnableSpace> _owned;

        protected readonly Player _nextPlayer;
    }

    public class AIPlayer : Player
    {
        //ENUMS
        public enum Difficulty
        {
            //Look into replacing this enum with a Strategy Pattern implementation
            //That way I can mix and match different anger algorithms
            Angry
        }

        //CONSTRUCTOR
        public AIPlayer(MonopolySettings settings) : base(settings)
        {
            //All AI players have the same difficulty by this strategy
            _difficulty = settings.AIDifficulty;
        }

        //METHODS
        protected override bool ForceTrade(Player creditor)
        {
            //Done automatically. Hopefully according to anger levels
            throw new NotImplementedException();
        }

        protected override void ForceEarnMoney()
        {
            //Done automatically. Hopefully according to anger levels
            throw new NotImplementedException();
        }

        //PRIVATE FIELDS
        Difficulty _difficulty;
    }
}
