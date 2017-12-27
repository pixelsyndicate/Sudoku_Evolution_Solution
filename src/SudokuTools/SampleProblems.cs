namespace SudokuTools
{
    public class SampleProblems
    {
        /// easy
        /// solved quickly with almost any reasonable no, me
        public static int[][] easyProblem()
        {
            int[][] ProblemRowS = new int[Constants.BoardSize][];
            for (int i = 0; i < Constants.BlockSize; i++)
            {
                ProblemRowS[i] = new int[Constants.BlockSize];
            }
            ProblemRowS[0] = new int[] { 0, 0, 7, 0, 0, 2, 9, 3, 0 };
            ProblemRowS[1] = new int[] { 0, 8, 1, 0, 0, 0, 0, 0, 5 };
            ProblemRowS[2] = new int[] { 9, 0, 4, 7, 0, 0, 1, 6, 0 };

            ProblemRowS[3] = new int[] { 0, 1, 0, 8, 0, 0, 0, 0, 6 };
            ProblemRowS[4] = new int[] { 8, 4, 6, 0, 0, 0, 5, 9, 2 };
            ProblemRowS[5] = new int[] { 5, 0, 0, 0, 0, 6, 0, 1, 0 };

            ProblemRowS[6] = new int[] { 0, 9, 2, 0, 0, 8, 3, 0, 1 };
            ProblemRowS[7] = new int[] { 4, 0, 0, 0, 0, 0, 6, 5, 0 };
            ProblemRowS[8] = new int[] { 0, 6, 5, 4, 0, 0, 2, 0, 0 };
            return ProblemRowS;
        }

        public static int[][] toughProblem()
        {
            int[][] ProblemRowS = new int[Constants.BoardSize][];
            for (int i = 0; i < Constants.BlockSize; i++)
            {
                ProblemRowS[i] = new int[Constants.BlockSize];
            }
            ProblemRowS[0] = new int[] { 0, 0, 3, 0, 0, 1, 0, 0, 9 };
            ProblemRowS[1] = new int[] { 4, 0, 0, 0, 0, 0, 1, 0, 0 };
            ProblemRowS[2] = new int[] { 1, 9, 0, 7, 3, 0, 0, 8, 0 };

            ProblemRowS[3] = new int[] { 0, 4, 0, 0, 1, 0, 5, 0, 8 };
            ProblemRowS[4] = new int[] { 0, 0, 0, 4, 5, 3, 0, 1, 0 };
            ProblemRowS[5] = new int[] { 5, 0, 1, 0, 2, 0, 0, 0, 0 };

            ProblemRowS[6] = new int[] { 0, 7, 0, 1, 8, 5, 0, 6, 0 };
            ProblemRowS[7] = new int[] { 0, 0, 4, 3, 7, 0, 8, 0, 1 };
            ProblemRowS[8] = new int[] { 8, 0, 0, 6, 0, 0, 7, 0, 0 };
            return ProblemRowS;
        }

        public static int[][] difficultProblem()
        {
            int[][] ProblemRowS = new int[Constants.BoardSize][];
            for (int i = 0; i < Constants.BlockSize; i++)
            {
                ProblemRowS[i] = new int[Constants.BlockSize];
            }
            ProblemRowS[0] = new int[] { 0, 0, 6, 2, 0, 0, 0, 8, 0 };
            ProblemRowS[1] = new int[] { 0, 0, 8, 9, 7, 0, 0, 0, 0 };
            ProblemRowS[2] = new int[] { 0, 0, 4, 8, 1, 0, 5, 0, 0 };

            ProblemRowS[3] = new int[] { 0, 0, 0, 0, 6, 0, 0, 0, 2 };
            ProblemRowS[4] = new int[] { 0, 7, 0, 0, 0, 0, 0, 3, 0 };
            ProblemRowS[5] = new int[] { 6, 0, 0, 0, 5, 0, 0, 0, 0 };

            ProblemRowS[6] = new int[] { 0, 0, 2, 0, 4, 7, 1, 0, 0 };
            ProblemRowS[7] = new int[] { 0, 0, 3, 0, 2, 8, 4, 0, 0 };
            ProblemRowS[8] = new int[] { 0, 5, 0, 0, 0, 1, 2, 0, 0 };
            return ProblemRowS;
        }

        public static int[][] veryDifficultProblem()
        {
            int[][] ProblemRowS = new int[Constants.BoardSize][];
            for (int i = 0; i < Constants.BlockSize; i++)
            {
                ProblemRowS[i] = new int[Constants.BlockSize];
            }
            ProblemRowS[0] = new int[] { 0, 0, 0, 0, 7, 0, 0, 0, 0 };
            ProblemRowS[1] = new int[] { 0, 9, 0, 5, 0, 6, 0, 8, 0 };
            ProblemRowS[2] = new int[] { 0, 0, 8, 4, 0, 1, 2, 0, 0 };

            ProblemRowS[3] = new int[] { 0, 5, 9, 0, 0, 0, 8, 4, 0 };
            ProblemRowS[4] = new int[] { 7, 0, 0, 0, 0, 0, 0, 0, 6 };
            ProblemRowS[5] = new int[] { 0, 2, 3, 0, 0, 0, 5, 7, 0 };

            ProblemRowS[6] = new int[] { 0, 0, 5, 3, 0, 7, 4, 0, 0 };
            ProblemRowS[7] = new int[] { 0, 1, 0, 6, 0, 8, 0, 9, 0 };
            ProblemRowS[8] = new int[] { 0, 0, 0, 0, 1, 0, 0, 0, 0 };
            return ProblemRowS;
        }

        public static int[][] extremelyDifficultProblem()
        {
            int[][] ProblemRowS = new int[Constants.BoardSize][];
            for (int i = 0; i < Constants.BlockSize; i++)
            {
                ProblemRowS[i] = new int[Constants.BlockSize];
            }
            ProblemRowS[0] = new int[] { 0, 0, 0, 6, 0, 0, 4, 0, 0 };
            ProblemRowS[1] = new int[] { 7, 0, 0, 0, 0, 3, 6, 0, 0 };
            ProblemRowS[2] = new int[] { 0, 0, 0, 0, 9, 1, 0, 8, 0 };

            ProblemRowS[3] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            ProblemRowS[4] = new int[] { 0, 5, 0, 1, 8, 0, 0, 0, 3 };
            ProblemRowS[5] = new int[] { 0, 0, 0, 3, 0, 6, 0, 4, 5 };

            ProblemRowS[6] = new int[] { 0, 4, 0, 2, 0, 0, 0, 6, 0 };
            ProblemRowS[7] = new int[] { 9, 0, 3, 0, 0, 0, 0, 0, 0 };
            ProblemRowS[8] = new int[] { 0, 2, 0, 0, 0, 0, 1, 0, 0 };
            return ProblemRowS;
        }


        public static int[][] mostDifficultProblem()
        {
            int[][] ProblemRowS = new int[Constants.BoardSize][];
            for (int i = 0; i < Constants.BlockSize; i++)
            {
                ProblemRowS[i] = new int[Constants.BlockSize];
            }
            ProblemRowS[0] = new int[] { 0, 2, 0, 0, 0, 0, 0, 0, 0 };
            ProblemRowS[1] = new int[] { 0, 0, 0, 6, 0, 0, 0, 0, 3 };
            ProblemRowS[2] = new int[] { 0, 7, 4, 0, 8, 0, 0, 0, 0 };

            ProblemRowS[3] = new int[] { 0, 0, 0, 0, 0, 3, 0, 0, 2 };
            ProblemRowS[4] = new int[] { 0, 8, 0, 0, 4, 0, 0, 1, 0 };
            ProblemRowS[5] = new int[] { 6, 0, 0, 5, 0, 0, 0, 0, 0 };

            ProblemRowS[6] = new int[] { 0, 0, 0, 0, 1, 0, 7, 8, 0 };
            ProblemRowS[7] = new int[] { 5, 0, 0, 0, 0, 9, 0, 0, 0 };
            ProblemRowS[8] = new int[] { 0, 0, 0, 0, 0, 0, 0, 4, 0 };
            return ProblemRowS;
        }


    }
}
