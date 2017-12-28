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
            var resultErrors = SudokuTool.Errors(result);
            Assert.Equal(0, resultErrors);

            if (SudokuTool.Errors(result) == 0)
                Debug.WriteLine("Optimal solution found. \n");
            else
                Debug.WriteLine("Did not find optimal solution \n");

            Debug.WriteLine("\nBest solution found: \n");
            _disp.DisplayMatrix(result);


        }

        [Theory]
        [InlineData(ProblemLevels.Easy, 15, 5000, 20)]
        [InlineData(ProblemLevels.Tough, 15, 5000, 20)]
        [InlineData(ProblemLevels.Difficult, 50, 5000, 20)]
        [InlineData(ProblemLevels.VeryDifficult, 50, 5000, 20)]
        [InlineData(ProblemLevels.VeryDifficult, 200, 5000, 20)]
        //[InlineData(ProblemLevels.ExtremelyDifficult, 200, 5000, 40)]
        //[InlineData(ProblemLevels.MostDifficult, 200, 5000, 40)]
        public void CAN_I_SOLVE_VARIOUS_PROBLEMS(ProblemLevels diff, int organisms, int maxEpochs, int maxExtinctions)
        {
            Problem problem = ProblemGenerator.GetProblem(diff);
            _evo = new EvolutionSolution();
            Debug.WriteLine($"Attempting solution of {diff.ToString()} with {organisms} organisms \n");
            int[][] result = _evo.SolveWithinExtinctions(problem.Rows, organisms, maxEpochs, maxExtinctions).Result;
            var resultErrors = SudokuTool.Errors(result);

            bool optimalResult = (resultErrors == 0);
            if (optimalResult)
                Debug.WriteLine("Optimal solution found. \n");
            else
                Debug.WriteLine("Did not find optimal solution \n");

            Debug.WriteLine("\nBest solution found: \n");
            _disp.DisplayMatrix(result);
            Assert.True(optimalResult);

        }



    }
}
