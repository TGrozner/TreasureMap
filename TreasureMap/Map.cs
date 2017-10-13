using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreasureMap.Constants;

namespace TreasureMap
{
    public class Map
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public Cell[,] Content { get; set; }
    }
}
