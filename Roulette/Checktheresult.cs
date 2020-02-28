using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roulette
{
    class Checktheresult : IResult
    {


        List<object> instruction;

        int bank = 0;

        public int Bank
        {
            get { return bank; }
            set { bank = value; }
        }
        public List<object> Intruction
        {
            get { return instruction; }
            set { instruction = value; }
        }

        public int Check_the_result(int z)
        {
            BaseGame bas = new BaseGame();

            List<object> data = new List<object>() { };

            for (int i = 0; i < instruction.Count(); i++)
            {
                if (Convert.ToString(instruction[i].GetType()) == "Roulette.RoulletConb")
                {
                    //Если в инструкции RoulletConb там надятся сведения о ставке.
                    BaseGame game = CombStatr((RoulletConb)instruction[i]);

                    if (game.Outcome(z, (int)instruction[i + 1]) == 0)
                    {
                        return 0;
                        //data.Add("Ставка" + instruction[i + 2] + " на " + instruction[i - 1] + " Проиграла");

                    }
                    else
                    {
                        
                        int money  = game.Bank((int)instruction[i + 2], z, (int)instruction[i + 1]);
                        bank += money;
                        //data.Add("Ставка на " + instruction[i - 1] + " победила, выигрыш: " + money);
                        return 1;

                    }
                }
            }
            return 2;
        }

        public List<object> Outresult()
        {
            
            List<object> result = new List<object>() { };
            int z = Random();
            Check_the_result(z);

            return result;

        }
        public int Random()
        {
           Random rand = new Random();

            return rand.Next(0, 37);
        }

        static BaseGame CombStatr(RoulletConb roulletConb)
        {
            switch (roulletConb)
            {
                case RoulletConb.Number:
                    return new Number();
                case RoulletConb.Color:
                    return new Color();
                case RoulletConb.ColumnsBet:
                    return new ColumnsBet();
                case RoulletConb.DozensBet:
                    return new DozensBet();
                case RoulletConb.EvenOdd:
                    return new EvenOdd();
                case RoulletConb.HighLow:
                    return new HighLow();
                default:
                    throw new Exception("Что то не то");
            }
        }

        
    }

}
