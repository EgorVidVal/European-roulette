using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roulette
{
    public class BaseGame
    {
        public virtual int Outcome(int rand, int number)
        {
            return 3;
        }
        public virtual int Bank(int rate, int rand, int number)
        {
            return 0;
        }

        public string Color_nubmer(int number_roulette)
        {
            switch (number_roulette)
            {
                case 0: return ("Zerro");
                case 1: return ("Red");
                case 3: return ("Red");
                case 5: return ("Red");
                case 7: return ("Red");
                case 9: return ("Red");
                case 12: return ("Red");
                case 14: return ("Red");
                case 16: return ("Red");
                case 18: return ("Red");
                case 19: return ("Red");
                case 21: return ("Red");
                case 23: return ("Red");
                case 27: return ("Red");
                case 25: return ("Red");
                case 30: return ("Red");
                case 32: return ("Red");
                case 34: return ("Red");
                case 36: return ("Red");
                default:
                    return "Black";

            }
        }
    }
    public class Number : BaseGame
    {
        public override int Outcome(int rand, int number)
        {
            if (number == rand)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        public override int Bank(int rate, int rand, int number)
        {
            if (Outcome(rand, number) == 1)
            {
                return rate * 36;
            }
            return 0;
        }
    }
    public class Color : BaseGame
    {
        public override int Outcome(int rand, int number)
        {
           


            if (Color_nubmer(rand) == Color_nubmer(number))
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        public override int Bank(int rate, int rand, int number)
        {
            if (Outcome(rand, number) == 1)
            {
                return rate * 2;
            }
            return 0;
        }
        
    }
    public class ColumnsBet : BaseGame
    {
        public override int Outcome(int rand, int number)
        {
            for(int i = 0;i < 12;i++)
            {
                if(rand > 3)
                {
                    rand -= 3;
                }
                if(number > 3)
                {
                    number -= 3;
                }
            }

            if(rand == number)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        public override int Bank(int rate, int rand, int number)
        {
            if (Outcome(rand, number) == 1)
            {
                return rate * 3;
            }
            return 0;
        }
    }
    public class DozensBet : BaseGame
    {
        public override int Outcome(int rand, int number)
        {
            for (int i = 0; i < 12; i++)
            {
                if (rand >= 1 && rand <= 12)
                {
                    
                    if (number == 3)
                    {
                        return 1;
                    }
                }
             
                if (rand >= 13 && rand <= 24)
                {
                    if (number == 13)
                    {
                        return 1;
                    }
                }

                if (rand >= 25)
                {
                    if (number == 25)
                    {
                        return 1;
                    }
                }
            }

            return 0;
        }
        public override int Bank(int rate, int rand, int number)
        {
            if (Outcome(rand, number) == 1)
            {
                return rate * 3;
            }
            return 0;
        }
    }
    public class EvenOdd : BaseGame
    {
        public override int Outcome(int rand, int number)
        {

            if ((rand % 2) == 0 && (number & 2) == 0)
            {
                
                return 1;
            }

            if ((rand % 2) != 0 && (number & 2) != 0)
            {
                return 1;
            }

            return 0;
        }
        public override int Bank(int rate, int rand, int number)
        {
            if (Outcome(rand, number) == 1)
            {
                return rate * 2;
            }
            return 0;
        }
    }
    public class HighLow : BaseGame
    {
        public override int Outcome(int rand, int number)
        {
            if(rand <= 18 && rand != 0 && number <=18)
            {
                return 1;
            }
            if (rand > 18 && number > 18)
            {
                return 1;
            }

            return 0;
        }
        public override int Bank(int rate, int rand, int number)
        {
            if (Outcome(rand, number) == 1)
            {
                return rate * 2;
            }
            return 0;
        }
    }
    public enum RoulletConb
    {
        Number = 1,
        Color,
        ColumnsBet,
        DozensBet,
        EvenOdd,
        HighLow
    }
}
