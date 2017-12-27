using SudokuTools;
using System;
using System.Diagnostics;
using Xunit;

namespace ToolsTests
{
    public class SolvePuzzleTests
    {
        private static Random _rnd = new Random(0);
        private EvolutionSolution _evo;
        private IDisplayMatrix _disp = new DebugDisplayMatrix();
        [Fact]
        public void CAN_I_SOLVE_EASY_PROBLEM()
        {
            Problem problem = ProblemGenerator.GetProblem(ProblemLevels.Easy);
            _evo = new EvolutionSolution();
            int[][] result = _evo.SolveWithinExtinctions(problem.Rows, 15, 5000, 20).Result;
            var resultErrors = EvolutionSolution.Errors(result);
            Assert.Equal(0, resultErrors);

            if (EvolutionSolution.Errors(result) == 0)
                Debug.WriteLine("Optimal solution found. \n");
            else
                Debug.WriteLine("Did not find optimal solution \n");

            Debug.WriteLine("\nBest solution found: \n");
            _disp.DisplayMatrix(result);


        }

        [Theory]
        [InlineData(ProblemLevels.Easy)]
        [InlineData(ProblemLevels.Tough)]
        [InlineData(ProblemLevels.Difficult)]
        [InlineData(ProblemLevels.VeryDifficult)]
        [InlineData(ProblemLevels.ExtremelyDifficult)]
        [InlineData(ProblemLevels.MostDifficult)]
        public void CAN_I_SOLVE_VARIOUS_PROBLEMS(ProblemLevels diff)
        {
            Problem problem = ProblemGenerator.GetProblem(diff);
            _evo = new EvolutionSolution();
            int[][] result = _evo.SolveWithinExtinctions(problem.Rows, 15, 5000, 20).Result;
            var resultErrors = EvolutionSolution.Errors(result);


            if (EvolutionSolution.Errors(result) == 0)
                Debug.WriteLine("Optimal solution found. \n");
            else
                Debug.WriteLine("Did not find optimal solution \n");

            Debug.WriteLine("\nBest solution found: \n");
            _disp.DisplayMatrix(result);


        }



    }
}
