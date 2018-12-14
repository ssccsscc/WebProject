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

namespace WpfApplication2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            run_cmd(@"AI.py", "");
        }

        public static void run_cmd(string cmd, string args)
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = @"C:\Users\Rakowayaweika\AppData\Local\Programs\Python\Python37\python.exe";
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

                    if (i % 2 == 0) {
                        for (int j = 0; j < 17; j++) {
                            string result = reader.ReadLine();
                            Console.WriteLine(result);
                        }


                    }
                    else {
                        string dd = Console.ReadLine();

                        writer.WriteLine(dd);
                        //Console.Write(result);

                    }

                    i++;
                }


            }
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
