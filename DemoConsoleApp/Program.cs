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

            _disp = new ConsoleDisplayMatrix();
            _evo = new EvolutionSolution();

            Problem problem = ProblemGenerator.GetProblem(ProblemLevels.Easy);


            _evo = new EvolutionSolution();
            int[][] result = _evo.SolveWithinExtinctions(problem.Rows, 15, 5000, 20).Result;
            var resultErrors = SudokuTool.Errors(result);

            if (SudokuTool.Errors(result) == 0)
                Console.WriteLine("Optimal solution found. \n");
            else
                Console.WriteLine("Did not find optimal solution \n");

            Console.WriteLine("\nBest solution found: \n");

            _disp.SplashView(_evo.stats, problem.Rows, result);


            Console.ReadLine();
        }
    }
}
