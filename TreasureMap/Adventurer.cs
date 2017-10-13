using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreasureMap.Constants;

namespace TreasureMap
{
    public class Adventurer
    {
        public string Name { get; set; }
        public Coordinates Coordinates { get; set; }
        public string Orientation { get; set; }
        public Queue<string> Moves { get; set; }
        public int TreasuresCount { get; set; }

        public Adventurer(string name, Coordinates coordinates, string orientation, Queue<string> moves, int treasuresCount)
        {
            Name = name;
            Coordinates = coordinates;
            Orientation = orientation;
            Moves = moves;
            TreasuresCount = treasuresCount;
        }

        public Adventurer()
        {
        }

        public static List<Adventurer> Get(string[] fileContent)
        {
            var data = fileContent.Select(x => x.Replace("-", "").Split(' ').Where(y => y != string.Empty).ToArray()).ToArray();
            var adventurers = new List<Adventurer>();
            foreach (var mapLine in data)
            {
                switch (mapLine[0])
                {
                    case CellType.Adventurer:
                        var adventurer = new Adventurer
                        {
                            Name = mapLine[1],
                            Coordinates = new Coordinates(int.Parse(mapLine[2]), int.Parse(mapLine[3])),
                            Moves = new Queue<string>(mapLine[5].Select(x => x.ToString())),
                            Orientation = mapLine[4]
                        };
                        adventurers.Add(adventurer);
                        break;
                }
            }
            return adventurers;
        }
    }

    public class Coordinates
    {
        public Coordinates()
        {
        }

        public Coordinates(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }

        public override string ToString()
        {
            return $"X: {X}, Y:{Y}";
        }
    }
}
