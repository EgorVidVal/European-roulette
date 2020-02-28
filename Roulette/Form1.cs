using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;

namespace Roulette
{


    public partial class Start_Game : Form
    {
        public Start_Game()
        {
            InitializeComponent();
        }

        //Данные о ставке
        List<object> instruction = new List<object>() { };

        //Данные о ставке для повторения
        List<object> instruction_Copy = new List<object>() { };

        //Сумма всех ставок.
        int allBank = 0;

        //Список для хранений историй банка и колличества игр
        PointPairList history = new PointPairList();

        //Счетчик колличетсва игр      
        int count = 0;

        //Условия для инструкции
        List<object> Conditions = new List<object>() { };

        //Включает режим автоиструкции
        bool conditions = false;

        //Выводить на данные на табло или нет.
        bool stream = false;

        int zip = 0;
        public int StartGame()
        {
            int z = Random_rull();
            
            BaseGame bas = new BaseGame();
            if (checkBox1.Checked == false)
            {
                Output.Text += "***********\n";
                //Что выпало
                Output.Text += "Выпадение\n";

                Output.Text += "\n" + bas.Color_nubmer(z) + "\n";
                Output.Text += Convert.ToString(z) + "\n";
                Output.Text += "\n";
                Output.Text += "Результат: \n";
            } 
            int result = Calculation(z);
            
            //Добавляет в список колилчество денег и какой ход для визуализации на графике.
            if (checkBox2.Checked == true)
            {
                int simplification = count / (50 + zip);
               

                if (simplification >= 1)
                {
                    Console.WriteLine(history.Count);
                    try
                    {
                        for (int i = 5; i <= history.Count - 1; i += 2)
                            history.RemoveAt(i);
                        for(int i = 0; i <= history.Count - 1; i += 2)
                            history.RemoveAt(i);
                        Console.WriteLine(history.Count);
                    }
                    catch
                    {

                    }
                    zip += 50;
                }
            }
            history.Add(count, Convert.ToDouble(richTextBox7.Text));
            count++;
            //Thread.Sleep(15);
            
            instruction_Copy = instruction;
            instruction = new List<object>();
            if (checkBox1.Checked == false) Output.Text += "************" + "\n";
            if (checkBox1.Checked == false) Output.Text += "\n";
            //Graph(history);
            return result; 
        }      
        public int Calculation(int z)
        {
            for (int i = 0; i < instruction.Count(); i++)
            {
                if (Convert.ToString(instruction[i].GetType()) == "Roulette.RoulletConb")
                {
                    //Если в инструкции RoulletConb там надятся сведения о ставке.
                    BaseGame game = CombStatr((RoulletConb)instruction[i]);

                    if (game.Outcome(z, (int)instruction[i + 1]) == 0)
                    {
                        if (checkBox1.Checked == false) Output.Text += "Ставка" + instruction[i + 2] + " на " + instruction[i - 1] + " Проиграла\n";
                        return 0;
                    }
                    else
                    {
                        int bank = game.Bank((int)numericUpDown1.Value, z, (int)instruction[i + 1]);
                        if (checkBox1.Checked == false) Output.Text += "Ставка на " + instruction[i - 1] + " победила, выигрыш: " + bank + "\n";
                        // обновляет банк если выиград
                        richTextBox7.Text = Convert.ToString(Convert.ToInt32(richTextBox7.Text) + bank);

                        return 1;
                    }
                }
            }
            return 2;
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
        public void StartGameInstruction()
        {
            Start_Game start = new Start_Game();
            int count = 0;

            for (int x = 0; x < numericUpDown2.Value; x++)
            {
                if (count >= numericUpDown3.Value)
                {
                    int result = StartGame();
                    ReturnRate();

                    if (Conditions[4] == "null")
                    {
                        count = 0;
                    }

                    if (Conditions[4] == "gameall")
                    {
                        if (result == 1)
                        {
                            count = 0;
                        }
                    }
                }

                //Проверяет все ставки из интрукции
                for (int i = 0; i < Conditions.Count() - 1; i++)
                {
                    Start_Game f = new Start_Game();

                    int z = f.Random_rull();
                    if (Convert.ToString(Conditions[i + 1].GetType()) == "Roulette.RoulletConb")
                    {
                        //Если в инструкции RoulletConb там надятся сведения о ставке.
                        BaseGame game = CombStatr((RoulletConb)Conditions[i + 1]);

                        if (game.Outcome(z, (int)Conditions[i]) == 0)
                        {
                            if (Conditions[2] == "not")
                            {
                                count++;
                            }
                            else
                            {
                                count = 0;
                            }
                        }
                        else
                        {
                            if (Conditions[2] == "yes")
                            {
                                count++;
                            }
                            else
                            {
                                count = 0;
                            }

                        }
                    }
                }

            }

        }
        public int Random_rull()
        {
            Random rand = new Random();

            return rand.Next(0, 37);

        }
        public void Graph(PointPairList list)
        {

            GraphPane pan = zed.GraphPane;
            pan.Title.Text = "График банка";
            pan.XAxis.Title.Text = "Колличество игр";
            pan.YAxis.Title.Text = "Сумма";



            pan.AddCurve(null, list, System.Drawing.Color.Blue, SymbolType.None);

            zed.AxisChange();
            zed.Invalidate();


        }

        //Выводит информацию о ставке.
        //0 Название ставки
        //1 на что ставим
        //3 дейсвие
        private void InstructioInput(string value, int value_rate, object addRoul)
        {
            if (conditions == false)
            {
                allBank += (int)numericUpDown1.Value;
                instruction.Add(value);
                instruction.Add(addRoul);
                instruction.Add(value_rate);
                instruction.Add(numericUpDown1.Value);
                OutPut(Output, value, (int)numericUpDown1.Value, richTextBox7);
            }
            else
            {
                Conditions.Add(value_rate);
                Conditions.Add(addRoul);

            }


        }
        static void OutPut(System.Windows.Forms.RichTextBox output, string znach, int rate, System.Windows.Forms.RichTextBox textbox = null)
        {
            output.Text += znach + "\n";
            output.Text += "Ставка " + rate + "\n";
            if (textbox != null)
            {
                textbox.Text = Convert.ToString(Convert.ToInt32(textbox.Text) - rate);
            }

        }
        public void Output_TextChanged(object sender, EventArgs e)
        {
            Output.SelectionStart = Output.TextLength;
            Output.ScrollToCaret();
        }

        #region RateButton 
        private void But1v18_Click(object sender, EventArgs e) { InstructioInput("HighLow 1-18", 1, RoulletConb.HighLow); }
        private void But19v36_Click(object sender, EventArgs e) { InstructioInput("HighLow 1-19", 19, RoulletConb.HighLow); }
        private void Nech_Click(object sender, EventArgs e) { InstructioInput("EvenOdd", 3, RoulletConb.EvenOdd); }   
        private void But2k1_3_Click(object sender, EventArgs e) { InstructioInput("Columns Bet 3 ", 3, RoulletConb.ColumnsBet); }
        private void But2k1_2_Click(object sender, EventArgs e) { InstructioInput("Columns Bet 2 ", 2, RoulletConb.ColumnsBet); }
        private void But2k1_1_Click(object sender, EventArgs e) { InstructioInput("Columns Bet 1 ", 1, RoulletConb.ColumnsBet); }
        private void Three12_Click(object sender, EventArgs e) { InstructioInput("Dozens Bet 3", 25, RoulletConb.DozensBet); }
        private void Two12_Click(object sender, EventArgs e) { InstructioInput("Dozens Bet  2", 13, RoulletConb.DozensBet); }
        private void One12_Click(object sender, EventArgs e) { InstructioInput("Dozens Bet 1", 3, RoulletConb.DozensBet); }
        private void Chet_Click(object sender, EventArgs e) { InstructioInput("EvenOdd", 4, RoulletConb.EvenOdd); }
        private void Button6_Click(object sender, EventArgs e) { InstructioInput("Red", 1, RoulletConb.Color); }
        private void Button7_Click(object sender, EventArgs e) { InstructioInput("Black", 2, RoulletConb.Color); }
        #endregion

        #region Nubmer
        private void Butt36_Click(object sender, EventArgs e) { InstructioInput("36", 36, RoulletConb.Number); }
        private void Butt35_Click(object sender, EventArgs e) { InstructioInput("35", 35, RoulletConb.Number); }
        private void Butt34_Click(object sender, EventArgs e) { InstructioInput("34", 34, RoulletConb.Number); }
        private void Butt33_Click(object sender, EventArgs e) { InstructioInput("33", 33, RoulletConb.Number); }
        private void Butt32_Click(object sender, EventArgs e) { InstructioInput("32", 32, RoulletConb.Number); }
        private void Butt31_Click(object sender, EventArgs e) { InstructioInput("31", 31, RoulletConb.Number); }
        private void Butt30_Click(object sender, EventArgs e) { InstructioInput("30", 30, RoulletConb.Number); }
        private void Butt29_Click(object sender, EventArgs e) { InstructioInput("29", 29, RoulletConb.Number); }
        private void Butt28_Click(object sender, EventArgs e) { InstructioInput("28", 28, RoulletConb.Number); }
        private void Butt27_Click(object sender, EventArgs e) { InstructioInput("27", 27, RoulletConb.Number); }
        private void Butt26_Click(object sender, EventArgs e) { InstructioInput("26", 26, RoulletConb.Number); }
        private void Butt25_Click(object sender, EventArgs e) { InstructioInput("25", 25, RoulletConb.Number); }
        private void Butt24_Click(object sender, EventArgs e){ InstructioInput("24", 24, RoulletConb.Number); }
        private void Butt23_Click(object sender, EventArgs e) { InstructioInput("23", 23, RoulletConb.Number); }
        private void Butt22_Click(object sender, EventArgs e) { InstructioInput("22", 22, RoulletConb.Number); }
        private void Butt21_Click(object sender, EventArgs e){ InstructioInput("21", 21, RoulletConb.Number); }
        private void Butt20_Click(object sender, EventArgs e) { InstructioInput("20", 20, RoulletConb.Number); }
        private void Butt19_Click(object sender, EventArgs e) { InstructioInput("19", 19, RoulletConb.Number); }
        private void Butt18_Click(object sender, EventArgs e) { InstructioInput("18", 18, RoulletConb.Number); }
        private void Butt17_Click(object sender, EventArgs e) { InstructioInput("17", 17, RoulletConb.Number); }
        private void Butt16_Click(object sender, EventArgs e) { InstructioInput("16", 16, RoulletConb.Number); }
        private void Butt15_Click(object sender, EventArgs e) { InstructioInput("15", 15, RoulletConb.Number); }
        private void Butt14_Click(object sender, EventArgs e) { InstructioInput("14", 14, RoulletConb.Number); }
        private void Butt13_Click(object sender, EventArgs e) { InstructioInput("13", 13, RoulletConb.Number); }
        private void Butt12_Click(object sender, EventArgs e) { InstructioInput("12", 12, RoulletConb.Number); }
        private void Butto11_Click(object sender, EventArgs e) { InstructioInput("11", 11, RoulletConb.Number); }
        private void Butto10_Click(object sender, EventArgs e) { InstructioInput("10", 10, RoulletConb.Number); }
        private void Button09_Click(object sender, EventArgs e) { InstructioInput("9", 9, RoulletConb.Number); }
        private void Button08_Click(object sender, EventArgs e) { InstructioInput("8", 8, RoulletConb.Number); }
        private void Button07_Click(object sender, EventArgs e) { InstructioInput("7", 7, RoulletConb.Number); }
        private void Button06_Click(object sender, EventArgs e) { InstructioInput("6", 6, RoulletConb.Number); }
        private void Button05_Click(object sender, EventArgs e) { InstructioInput("5", 5, RoulletConb.Number); }
        private void Button04_Click(object sender, EventArgs e) { InstructioInput("4", 4, RoulletConb.Number); }
        private void Button03_Click(object sender, EventArgs e){InstructioInput("3", 3, RoulletConb.Number);}
        private void Button02_Click(object sender, EventArgs e) { InstructioInput("2", 2, RoulletConb.Number); }
        private void Button01_Click(object sender, EventArgs e) { InstructioInput("1", 1, RoulletConb.Number); }
        private void Zerro_Click(object sender, EventArgs e) { }
        #endregion


        private void Button1_Click(object sender, EventArgs e)
        {
            StartGame();
        }
      

        private void NumericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void RichTextBox7_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void Start_Game_Load(object sender, EventArgs e)
        {
           
            
        }
        
        //Построение графика
       

        private void Zed_Load_1(object sender, EventArgs e)
        {
            
        }

        private void GraphClear_Click(object sender, EventArgs e)
        {
            history.Clear();
            count = 0;
        }

   

        private void NumericUpDown2_ValueChanged(object sender, EventArgs e)
        {

        }

        
        private void Button2_Click_1(object sender, EventArgs e)
        {

            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
            // вызов метода в другой поток с дополнительными параметрами в скобке
            Thread MyThread = new System.Threading.Thread(delegate () { StartGameInstruction(); });
            MyThread.IsBackground = true;
            MyThread.Start(); // запускаем поток  
            Graph(history);



        }

        private void Button3_Click(object sender, EventArgs e)
        {
            conditions = true;
        }

        private void NumericUpDown3_ValueChanged(object sender, EventArgs e)
        {

        }

        private void Button5_Click(object sender, EventArgs e)
        {
            Conditions.Add(numericUpDown3.Value);
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            Conditions.Add("yes");
        }

        private void Button7_Click_1(object sender, EventArgs e)
        {
            conditions = false;
        }

        private void Button8_Click(object sender, EventArgs e)
        {
            ReturnRate();
        }

        public void ReturnRate()
        {
            instruction = instruction_Copy;
            richTextBox7.Text = Convert.ToString(Convert.ToInt32(richTextBox7.Text) - allBank);
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            stream = true;
        }

        private void Button9_Click(object sender, EventArgs e)
        {
            Conditions.Add("not"); 
        }

        private void Button10_Click(object sender, EventArgs e)
        {
            Conditions.Add("gameall");
        }

        private void Button11_Click(object sender, EventArgs e)
        {
            Conditions.Add("null");
        }

        private void CheckBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Button12_Click(object sender, EventArgs e)
        {
            Checktheresult ceck = new Checktheresult();
            ceck.Bank = (int)numericUpDown1.Value;
            ceck.Intruction = instruction;
        }
    }
}
