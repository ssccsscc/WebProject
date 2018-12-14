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
using System.Windows.Media.Animation;
using System.Threading;

namespace WpfApplication2
{
    public class Battlefield
    {
        public int[,] field = new int[10, 10]; //Представление поля для удобного взаимодействия снаружи
        //public Cell[,] field_imgs = new Cell[10, 10]; //Массив картинок для отрисовки

        Canvas fieldcanvas;
        public Battlefield(Canvas canvas1) //0 - вода, 1 - кораблик, 2 - крестик, 3 - взрыв
        {
            for (int i = 0; i < 10; i++) 
                for (int j = 0; j < 10; j++)
                    field[i, j] = 0; //По умолчанию везде вода

            fieldcanvas = canvas1;
        }

        public void Draw() //Отрисовка в fieldcanvas 
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Cell item = new Cell(30, 30, new System.Windows.Thickness(i * 30, j * 30, 0, 0));

                    switch (field[i, j])
                    {
                        case 0:
                            item.Source = new BitmapImage(new Uri("Sprites/water.png", UriKind.Relative));
                            break;
                        case 1:
                            item.Source = new BitmapImage(new Uri("Sprites/ship.png", UriKind.Relative));
                            break;
                        case 2:
                            item.Source = new BitmapImage(new Uri("Sprites/cross.png", UriKind.Relative));
                            break;
                        case 3: item.Source = new BitmapImage(new Uri("Sprites/fire.png", UriKind.Relative));
                            break;
                        default:
                            break;
                    }
                    fieldcanvas.Children.Add(item);

                }
            }
        }



    }
}
