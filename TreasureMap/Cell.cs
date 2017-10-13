using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreasureMap.Constants;

namespace TreasureMap
{
    public class Cell
    {
        public virtual bool IsAccessible => true;

        public override string ToString()
        {
            return ".";
        }
    }

    public class MountainCell : Cell
    {
        public override bool IsAccessible => false;

        public override string ToString()
        {
            return CellType.Mountain;
        }
    }

    public class TreasureCell : Cell
    {
        public override bool IsAccessible => true;

        public int TreasuresCount { get; set; }

        public override string ToString()
        {
            return CellType.Treasure + $"({TreasuresCount})";
        }
    }
}
