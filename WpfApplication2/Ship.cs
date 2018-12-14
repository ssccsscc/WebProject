using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication2
{
    public class Ship
    {
        public int Length;
        public bool isVertical;
        public bool[] damagedCells; //true если повреждена часть

        public Ship(int length, bool isVertical)
        {
            Length = length;
            this.isVertical = isVertical;
            damagedCells = new bool[length];
        }
        public bool isDestroyed()
        {
            for (int i = 0; i < damagedCells.Length; i++)
            {
                if (!damagedCells[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
