using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreasureMap.Constants;

namespace TreasureMap
{
    public class World
    {
        public Map Map { get; set; }
        public IEnumerable<Adventurer> Adventurers { get; set; }

        public static World Build(string[] fileContent)
        {
            var map = new Map();
            var adventurers = new List<Adventurer>();
            foreach (var splittedLine in fileContent.Select(line => line.Split(new[] {" - "}, StringSplitOptions.None)))
            {
                switch (splittedLine[0])
                {
                    case "C":
                        map.Width = int.Parse(splittedLine[1]);
                        map.Height = int.Parse(splittedLine[2]);
                        map.Content = new Cell[map.Width, map.Height];
                        break;
                    case CellType.Mountain:
                        map.Content[int.Parse(splittedLine[1]), int.Parse(splittedLine[2])] = new MountainCell();
                        break;
                    case CellType.Treasure:
                        map.Content[int.Parse(splittedLine[1]), int.Parse(splittedLine[2])] = new TreasureCell { TreasuresCount = int.Parse(splittedLine[3])};
                        break;
                    case CellType.Adventurer:
                        var coordinates = new Coordinates(int.Parse(splittedLine[2]), int.Parse(splittedLine[3]));
                        var moves = new Queue<string>(splittedLine[5].Select(x => x.ToString()));
                        adventurers.Add(new Adventurer(splittedLine[1], coordinates, splittedLine[4], moves, 0));
                        break;
                }
                for (var i = 0; i < map.Width; i++)
                {
                    for (var j = 0; j < map.Height; j++)
                    {
                        if (map.Content[i, j] == null)
                        {
                            map.Content[i, j] = new Cell();
                        }
                    }
                }
            }
            return new World
            {
                Map = map,
                Adventurers = adventurers
            };
        }

        public World NextTick()
        {
            var map = Map;
            var adventurers = Adventurers;

            foreach (var adventurer in adventurers)
            {
                var moves = adventurer.Moves;
                if (moves.Count > 0)
                {
                    var nextMove = moves.Dequeue();
                    ComputeMove(this, adventurer, nextMove);
                }
            }

            return this;
        }

        private void ComputeMove(World world, Adventurer adventurer, string move)
        {
            if (move == MoveType.TurnRight)
            {
                switch (adventurer.Orientation)
                {
                    case Orientation.North:
                        adventurer.Orientation = Orientation.East;
                        break;
                    case Orientation.East:
                        adventurer.Orientation = Orientation.South;
                        break;
                    case Orientation.South:
                        adventurer.Orientation = Orientation.West;
                        break;
                    case Orientation.West:
                        adventurer.Orientation = Orientation.North;
                        break;
                }
            }
            else if (move == MoveType.TurnLeft)
            {
                switch (adventurer.Orientation)
                {
                    case Orientation.North:
                        adventurer.Orientation = Orientation.West;
                        break;
                    case Orientation.East:
                        adventurer.Orientation = Orientation.North;
                        break;
                    case Orientation.South:
                        adventurer.Orientation = Orientation.East;
                        break;
                    case Orientation.West:
                        adventurer.Orientation = Orientation.South;
                        break;
                }
            }
            else if (move == MoveType.GoForward)
            {
                var nextCell = new Cell();
                switch (adventurer.Orientation)
                {
                    case Orientation.North:
                        nextCell = world.Map.Content[adventurer.Coordinates.X, adventurer.Coordinates.Y - 1];
                        if (nextCell.IsAccessible)
                            --adventurer.Coordinates.Y;
                        break;
                    case Orientation.East:
                        nextCell = world.Map.Content[adventurer.Coordinates.X + 1, adventurer.Coordinates.Y];
                        if (nextCell.IsAccessible)
                            ++adventurer.Coordinates.X;
                        break;
                    case Orientation.South:
                        nextCell = world.Map.Content[adventurer.Coordinates.X, adventurer.Coordinates.Y + 1];
                        if (nextCell.IsAccessible)
                            ++adventurer.Coordinates.Y;
                        break;
                    case Orientation.West:
                        nextCell = world.Map.Content[adventurer.Coordinates.X - 1, adventurer.Coordinates.Y];
                        if (nextCell.IsAccessible)
                            --adventurer.Coordinates.X;
                        break;
                }

                var treasureCell = nextCell as TreasureCell;
                if (treasureCell?.TreasuresCount > 0)
                {
                    ++adventurer.TreasuresCount;
                    --treasureCell.TreasuresCount;
                }
            }
        }

        public override string ToString()
        {
            var result = string.Empty;
            for (var i = 0; i < Map.Height; i++)
            {
                for (var j = 0; j < Map.Width; j++)
                {
                    result += Map.Content[j, i].ToString().PadRight(8);
                }
                result += "\n";
            }

            return result;
        }
    }
}
