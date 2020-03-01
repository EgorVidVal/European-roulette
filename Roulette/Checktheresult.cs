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
       
        public List<object> data = new List<object>() { };

        public int Check_the_result(int z)
        {
            data.Clear();
            BaseGame bas = new BaseGame();
            foreach(object c in instruction)
            {
                Console.WriteLine(c);
            }

            for (int i = 0; i < instruction.Count() - 2; i++)
            {
                if (Convert.ToString(instruction[i].GetType()) == "Roulette.RoulletConb")
                {
                    //Если в инструкции RoulletConb там надятся сведения о ставке.
                    BaseGame game = CombStatr((RoulletConb)instruction[i]);

                    if (game.Outcome(z, (int)instruction[i + 1]) == 0)
                    {

                        data.Add("Ставка" + instruction[i + 2] + " на " + instruction[i - 1] + " Проиграла");

                    }
                    else
                    {
                        Console.WriteLine("***********");
                        Console.WriteLine(instruction[i + 2]);
                        Console.WriteLine(instruction[i + 1]);
                        Console.WriteLine("***********");
                        int money = game.Bank(Convert.ToInt32(instruction[i + 2]), z, Convert.ToInt32(instruction[i + 1]));
                        bank += money;
                        data.Add("Ставка на " + instruction[i - 1] + " победила, выигрыш: " + money);
                    }
                }
            }

           
            return 2;
        }

        public int Outresult()
        {
            int z = Random();
            Check_the_result(z);

            Start_Game start = new Start_Game();
            

            return z;
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
