using SudokuTools;
using System;

namespace DemoConsoleApp
{
    class Program
    {
        private static EvolutionSolution _evo;
        private static IDisplayMatrix _disp;
        static void Main(string[] args)
        {
            long startTime = DateTime.UtcNow.Ticks;
            _disp = new ConsoleDisplayMatrix();
            _evo = new EvolutionSolution();
            ProblemLevels thisLevel = ProblemLevels.Difficult;

            Problem problem = ProblemGenerator.GetProblem(thisLevel);
            Console.WriteLine("Starting problem resolution for a " + thisLevel + " puzzle.");
       

            _evo = new EvolutionSolution();
            int[][] result = _evo.SolveWithinExtinctions(problem.Rows, 15, 5000, 20).Result;

            var resultErrors = SudokuTool.Errors(result);
            long stopTime = DateTime.UtcNow.Ticks;

            if (SudokuTool.Errors(result) == 0)
                Console.WriteLine("Optimal solution found. \n");
            else
                Console.WriteLine("Did not find optimal solution \n");

            Console.WriteLine("\nBest solution found: \n");

            _disp.SplashView(_evo.stats, problem.Rows, result);

            Console.WriteLine();
            Console.ForegroundColor = (ConsoleColor.Cyan);
            Console.WriteLine("This run took: " + TimeSpan.FromTicks(stopTime - startTime).TotalSeconds.ToString("#.##") + "seconds");

            Console.ReadLine();
        }
    }
}
