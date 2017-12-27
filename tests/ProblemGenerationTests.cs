using SudokuTools;
using System;
using Xunit;

namespace ToolsTests
{
    public class ProblemGenerationTests
    {
        private static Random _rnd = new Random(0);

        [Theory]
        [InlineData(ProblemLevels.Easy)]
        [InlineData(ProblemLevels.Tough)]
        [InlineData(ProblemLevels.Difficult)]
        [InlineData(ProblemLevels.VeryDifficult)]
        [InlineData(ProblemLevels.ExtremelyDifficult)]
        [InlineData(ProblemLevels.MostDifficult)]
        public void Can_I_Get_Problems_From_Generator(ProblemLevels lev)
        {
            Problem problem = ProblemGenerator.GetProblem(lev);
            Assert.IsType<Problem>(problem);
        }


        [Theory]
        [InlineData(ProblemLevels.Easy)]
        [InlineData(ProblemLevels.Tough)]
        [InlineData(ProblemLevels.Difficult)]
        [InlineData(ProblemLevels.VeryDifficult)]
        [InlineData(ProblemLevels.ExtremelyDifficult)]
        [InlineData(ProblemLevels.MostDifficult)]
        public void Does_Problem_Have_Nine_Cells(ProblemLevels lev)
        {
            Problem problem = ProblemGenerator.GetProblem(lev);
            Assert.IsType<Problem>(problem);
            int[][] rows = problem.Rows;
            Assert.Equal<int>(9, rows.Length);
        }

        [Theory]
        [InlineData(ProblemLevels.Easy)]
        [InlineData(ProblemLevels.Tough)]
        [InlineData(ProblemLevels.Difficult)]
        [InlineData(ProblemLevels.VeryDifficult)]
        [InlineData(ProblemLevels.ExtremelyDifficult)]
        [InlineData(ProblemLevels.MostDifficult)]
        public void Does_Sample_Problem_Have_Correct_Values(ProblemLevels lev)
        {
            Problem problem = ProblemGenerator.GetProblem(lev);

            var expectedProblem =
                (lev == ProblemLevels.Easy) ? SampleProblems.easyProblem() :
                (lev == ProblemLevels.Tough) ? SampleProblems.toughProblem() :
                (lev == ProblemLevels.Difficult) ? SampleProblems.difficultProblem() :
                (lev == ProblemLevels.VeryDifficult) ? SampleProblems.veryDifficultProblem() :
                (lev == ProblemLevels.ExtremelyDifficult) ? SampleProblems.extremelyDifficultProblem() :
               (lev == ProblemLevels.MostDifficult) ? SampleProblems.mostDifficultProblem() : SampleProblems.easyProblem();

            for (int i = 0; i < problem.Rows.Length; i++)
            {
                Assert.Equal(expectedProblem[i], problem.Rows[i]);
                for (int j = 0; j < problem.Rows[i][j]; j++)
                {
                    Assert.Equal(expectedProblem[i][j], problem.Rows[i][j]);
                }
            }

        }
    }
}
