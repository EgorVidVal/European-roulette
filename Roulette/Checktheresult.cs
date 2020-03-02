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

        public int Check_the_result(int rand)
        {
            data.Clear();
            BaseGame game = new BaseGame();

            for (int i = 0; i < instruction.Count() - 2; i++)
            {
                switch (Instruct(i, rand, instruction, game))
                {
                    case 0:
                        data.Add("Ставка" + instruction[i + 2] + " на " + instruction[i + 1] + " Проиграла");
                        break;
                    case 1:
                        game = CombStatr((RoulletConb)instruction[i]);
                        int money = game.Bank(Convert.ToInt32(instruction[i + 2]), rand, Convert.ToInt32(instruction[i + 1]));   
                        bank += money;                      
                        data.Add("Ставка на " + instruction[i + 1] + " победила, выигрыш: " + money);
                        break;
                    default:
                         break;
                }
            }
            return 2;
        }

        public int AutoGame(int numberOfGames, int rand, List<object> instruct)
        {
            BaseGame game = new BaseGame();
            //Прошла ставка для проверки или нет
            List<int> outp = new List<int>() { };

            int x = Instruct(numberOfGames,rand, instruct, game);
            if (x == 0) return 0;
            if (x == 1) return 1;

            return 2;
        }
        public int Instruct(int i, int z, List<object> instruct, BaseGame game)
        {
            if (Convert.ToString(instruct[i].GetType()) == "Roulette.RoulletConb")
            {
                //Если в инструкции RoulletConb там надятся сведения о ставке.
                game = CombStatr((RoulletConb)instruct[i]);

                if (game.Outcome(z, (int)instruct[i + 1]) == 0)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            return 2;
        }

        public void Outresult(int rand)
        {            
            Check_the_result(rand);

            Start_Game start = new Start_Game();        
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
