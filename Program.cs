using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisAIEnvironment
{
    class Program
    {
        static void Main(string[] args)
        {
      //      string str = GeneticAlgorithm.EncodeToGene(-46);
    //     Console.WriteLine(str);
     //    Console.WriteLine(   GeneticAlgorithm.DecodeFromGene(str));
       //     Console.Read();
          //  Console.WriteLine(TwoPointCrossOver("1111111111","0000000000"));
           // Console.ReadKey();

            GeneticAlgorithm.Learn();


            Console.ReadLine();

            int[,] field = new int[10, 25];
            field = new int[,]
              {
                { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,},
                { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,},
                { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,},
                { 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,},
                { 1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,},
                { 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,},
                { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,},
                { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,},
                { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,},
                { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,}
              };

            var value = (NewTetrisAI.Find(field, 5, new int[1] { 1 }, 0, 2, true));
            TetrisEnvironment.Print(field, false);
            Console.WriteLine(value.way);
            Console.WriteLine(value.eval);
            Console.ReadLine();
        }

    }

}
