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
            battlefield.Draw();
        }
    }
}
