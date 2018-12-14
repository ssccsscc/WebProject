using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Threading;

namespace WpfApplication2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        delegate void del();
        string gl_info_output;
        string gl_info_input;
        bool gl_changed = false;
        string gl_result="";
        Battlefield EnemyField;


        public MainWindow()
        {
            InitializeComponent();
            Thread th1 = new Thread(TH1);
            EnemyField = new Battlefield(canvas2);
            EnemyField.Draw();
            th1.Start();

            //run_cmd(@"AI.py", "");
        }

        public void TH1()
        {
            run_cmd(@"AI.py", "");
        }


        public void run_cmd(string cmd, string args)
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = @"C:\Users\CloudCatcher\AppData\Local\Programs\Python\Python37\python.exe";
            start.Arguments = string.Format("{0} {1}", cmd, args);
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            start.RedirectStandardInput = true;
            using (Process process = Process.Start(start)) {
                StreamWriter writer = process.StandardInput;
                StreamReader reader = process.StandardOutput;
                bool exit = process.HasExited;
                int i = 0;
                //writer.Encoding = Encoding.Default; 
                while (!exit) {
                    try
                    {
                        if (i % 2 == 0)
                        {
                            for (int j = 0; j < 17; j++)
                            {
                                string result = reader.ReadLine();
                                if (j == 13)
                                {
                                    Console.WriteLine(result);

                                    gl_result = result;

                                    gl_info_output = result;
                                    del for_th1;
                                    for_th1 = Out_from_python;
                                    Dispatcher.BeginInvoke(DispatcherPriority.Normal, for_th1);
                                }
                                Console.WriteLine(result);
                            }
                        }
                        else
                        {
                            //string dd = Console.ReadLine();
                            bool get_out = true;
                            while (get_out)
                            {
                                if (gl_changed)
                                {
                                    writer.WriteLine(gl_info_input);
                                    gl_changed = false;
                                    get_out = false;


                                    del for_th2;
                                    for_th2 = Draw_enemy_field;
                                    Dispatcher.BeginInvoke(DispatcherPriority.Normal, for_th2);

                                }
                                Thread.Sleep(200);
                            }

                            //Console.Write(result);
                        }

                        i++;
                    }
                    catch { }
                }


            }
        }

        void Out_from_python()
        {
            label_shoot.Content = gl_info_output;
        }

        void button_enter_result_Click(object sender, RoutedEventArgs e)
        {
            gl_info_input = textbox_shoot.Text;
            gl_changed = true;
        }

        

        void Draw_enemy_field()
        {
            //Battlefield EnemyField = new Battlefield(canvas2);
            if (Convert.ToInt32(textbox_shoot.Text) == 0)
                EnemyField.field[Convert.ToInt32(Convert.ToString(gl_info_output[0])), Convert.ToInt32(Convert.ToString(gl_info_output[2]))] = 5;
            else if (Convert.ToInt32(textbox_shoot.Text) == 1 || Convert.ToInt32(textbox_shoot.Text) == 2)
                EnemyField.field[Convert.ToInt32(Convert.ToString(gl_info_output[0])), Convert.ToInt32(Convert.ToString(gl_info_output[2]))] = 3;
            EnemyField.Draw();

            //int column = 0;
            //for (int k = 4; k < 32; k += 3)
            //{

            //    Console.WriteLine(gl_result[k]);
            //    if (gl_result[k] == 79)
            //        EnemyField.field[gl_j, column] = 0;
            //    if (gl_result[k] == 32)
            //        EnemyField.field[gl_j, column] = 5;
            //    if (gl_result[k] == 63)
            //        EnemyField.field[gl_j, column] = 4;
            //    //gl_EnemyField.field[j, column] = result[k];

            //    column++;
            //}
            //EnemyField.Draw();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Battlefield battlefield = new Battlefield(canvas1);
            battlefield.Draw();

            battlefield.field[0, 0] = 1;
            battlefield.field[1, 0] = 1;
            battlefield.field[5, 5] = 1;
            battlefield.field[5, 6] = 1;

            battlefield.field[5, 7] = 1;
            battlefield.field[5, 6] = 2;
            battlefield.field[8, 8] = 3;

            battlefield.Draw();
        }
        private Game game;
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            game = new Game(canvas1);
            game.PlaceShips();
            //game.TryPlaceShip(new Ship(4, false), 1, 1);
            //game.TryPlaceShip(new Ship(3, true), 4, 3);
            game.UpdateBattlefiled();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            var result = game.ProcessShot(int.Parse(textBox.Text), int.Parse(textBox_Copy.Text));
            switch (result)
            {
                case Game.ShotResult.Missed:
                    label1.Content = "Мимо";
                    break;
                case Game.ShotResult.Damaged:
                    label1.Content = "Ранил";
                    break;
                case Game.ShotResult.Destroyed:
                    label1.Content = "Потопил";
                    break;
                default:
                    break;
            }
            game.UpdateBattlefiled();
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            game = new Game(canvas1);
            game.PlaceShips();
            game.UpdateBattlefiled();
        }

     
    }
}
