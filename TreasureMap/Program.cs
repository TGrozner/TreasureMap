using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreasureMap
{
    class Program
    {
        static void Main()
        {
            var fileContent = File.ReadAllLines(".\\Map.txt");
            var world = World.Build(fileContent);
            while (world.Adventurers.FirstOrDefault().Moves.Count > 0)
            {
                world.NextTick();
                Console.WriteLine(world.ToString());
            }
            Console.Read();
        }
    }
}
