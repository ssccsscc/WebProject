using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication2
{
    public class Game
    {
        /// <summary>
        /// Результат выстрела
        /// </summary>
        public enum ShotResult
        {
            Missed, Damaged, Destroyed
        }
        /// <summary>
        /// Структура с кораблем и его расположением
        /// </summary>
        public struct ShipOnBattlefield 
        {
            public int x;
            public int y;
            public Ship ship;
        }
        public struct Coordinates
        {
            public int x;
            public int y;
        }
        public Battlefield battleField;
        /// <summary>
        /// Все корабли на поле
        /// </summary>
        public readonly List<ShipOnBattlefield> Ships;
        /// <summary>
        /// Выстрелы не попавшие по кораблю
        /// </summary>
        public readonly List<Coordinates> MissedShots;

        public Game(System.Windows.Controls.Canvas canvas1)
        {
            battleField = new Battlefield(canvas1);
            Ships = new List<ShipOnBattlefield>();
            MissedShots = new List<Coordinates>();
        }
        /// <summary>
        /// true если координаты находятся на поле
        /// </summary>
        public bool isCoordinateValid(int x, int y)
        {
            if (x>9 || x<0 || y>9 || y<0)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// Обрабатывает выстрел и возвращает результат выстрела
        /// </summary>
        public ShotResult ProcessShot(int x, int y)
        {
            foreach (var ship in Ships)
            {
                if (!ship.ship.isVertical && ship.x<=x && ship.x+ship.ship.Length>x && ship.y==y)
                {
                    ship.ship.damagedCells[x - ship.x] = true;
                    if (ship.ship.isDestroyed())
                    {
                        for (int z = -1; z <= ship.ship.Length; z++)
                        {
                            if (isCoordinateValid(ship.x + z, ship.y - 1)) MissedShots.Add(new Coordinates() { x = ship.x + z, y = ship.y - 1 });
                            if (isCoordinateValid(ship.x + z, ship.y + 1)) MissedShots.Add(new Coordinates() { x = ship.x + z, y = ship.y + 1 });
                        }
                        if (isCoordinateValid(ship.x -1, ship.y)) MissedShots.Add(new Coordinates() { x = ship.x -1, y = ship.y });
                        if (isCoordinateValid(ship.x + ship.ship.Length, ship.y)) MissedShots.Add(new Coordinates() { x = ship.x + ship.ship.Length, y = ship.y });
                        return ShotResult.Destroyed;
                    }
                    return ShotResult.Damaged;
                }
                else
                {
                    if(ship.ship.isVertical && ship.y <= y && ship.y + ship.ship.Length > y && ship.x == x)
                    {
                        ship.ship.damagedCells[y - ship.y] = true;
                        if (ship.ship.isDestroyed())
                        {
                            for (int z = -1; z <= ship.ship.Length; z++)
                            {
                                if (isCoordinateValid(ship.x - 1, ship.y + z)) MissedShots.Add(new Coordinates() { x = ship.x - 1, y = ship.y + z });
                                if (isCoordinateValid(ship.x + 1, ship.y + z)) MissedShots.Add(new Coordinates() { x = ship.x + 1, y = ship.y + z });
                            }
                            if (isCoordinateValid(ship.x, ship.y - 1)) MissedShots.Add(new Coordinates() { x = ship.x, y = ship.y-1 });
                            if (isCoordinateValid(ship.x, ship.y + ship.ship.Length)) MissedShots.Add(new Coordinates() { x = ship.x, y = ship.y + ship.ship.Length });
                            return ShotResult.Destroyed;
                        }
                        return ShotResult.Damaged;
                    }
                }
            }
            if (isCoordinateValid(x, y)) MissedShots.Add(new Coordinates() { x = x, y = y });
            return ShotResult.Missed;
        }

        public void UpdateBattlefiled()
        {
            battleField.field = new int[10, 10];
            foreach (var shot in MissedShots)
            {
                battleField.field[shot.x,shot.y] = 3;
            }
            foreach (var ship in Ships)
            {
                for (int i = 0; i < ship.ship.Length; i++)
                {
                    if (ship.ship.isVertical)
                    {
                        if (ship.ship.damagedCells[i])
                        {
                            battleField.field[ship.x, ship.y + i] = 2;
                        }
                        else
                        {
                            battleField.field[ship.x, ship.y + i] = 1;
                        }
                    }
                    else
                    {
                        if (ship.ship.damagedCells[i])
                        {
                            battleField.field[ship.x + i, ship.y] = 2;
                        }
                        else
                        {
                            battleField.field[ship.x + i, ship.y] = 1;
                        }
                    }
                }
            }
            battleField.Draw();
        }

        /// <summary>
        /// Пробует поставить указанный корабль по заданным координатам. Если удалось то ставит и возвращает true
        /// </summary>
        public bool TryPlaceShip(Ship s, int x, int y)
        {
            bool[,] TempField = new bool[10, 10];
            foreach (var ship in Ships)
            {
                if (ship.ship.isVertical == false)
                {
                    for (int k = -1; k < 2; k++)
                    {
                        for (int z = -1; z < ship.ship.Length + 1; z++)
                        {
                            if (isCoordinateValid(ship.x + z, ship.y + k))
                            {
                                TempField[ship.x + z, ship.y + k] = true;
                            }
                        }
                    }
                }
                else
                {
                    for (int k = -1; k < ship.ship.Length + 1; k++)
                    {
                        for (int z = -1; z < 2; z++)
                        {
                            if (isCoordinateValid(ship.x + z, ship.y + k))
                            {
                                TempField[ship.x + z, ship.y + k] = true;
                            }
                        }
                    }
                }
            }
            if (s.isVertical == false)
            {
                for (int z = 0; z < s.Length; z++)
                {
                    if (isCoordinateValid(x + z, y))
                    {
                        if (TempField[x + z, y])
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                for (int z = 0; z < s.Length; z++)
                {
                    if (isCoordinateValid(x, y + z))
                    {
                        if (TempField[x, y + z])
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            Ships.Add(new ShipOnBattlefield() { ship = s, x = x, y = y });
            return true;
        }
        private static Random rnd = new Random();
        public void PlaceShips()
        {
            List<int> ships = new List<int>() { 4, 3, 3, 2, 2, 2, 1, 1, 1, 1 };
            for (int i = 0; i < ships.Count; i++)
            {
                Ship s = new Ship(ships[i], rnd.Next(0, 2)==1);
                while (!TryPlaceShip(s, rnd.Next(0, 10), rnd.Next(0, 10))) ;
            }
        }
    }
}
