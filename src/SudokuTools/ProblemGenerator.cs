namespace SudokuTools
{
    public class ProblemGenerator : Problem
    {


        private static Problem _problem;
        public static int[][] ProblemRowS
        {
            get { return _problem.Rows; }
            set { _problem.Rows = value; }
        }
        public static Problem GetProblem(ProblemLevels level)
        {
            // has a wrapper implimentation
            _problem = new Problem();

            switch (level)
            {
                case ProblemLevels.Easy:
                    ProblemRowS = SampleProblems.easyProblem();
                    break;

                case ProblemLevels.Tough:
                    // tough
                    // solved quickly with almost any reasonable no, me
                    ProblemRowS = SampleProblems.toughProblem();
                    break;

                case ProblemLevels.Difficult:
                    // August 2014 MSDN article problem -- difficult
                    // solved using no = 200, me = 5000
                    ProblemRowS = SampleProblems.difficultProblem();
                    break;
                case ProblemLevels.VeryDifficult:
                    // http://ieeexplore.ieee.org/stamp/stamp.jsp?tp=&arnumber=5412260
                    // very difficult.
                    // solved using no = 200, me = 9000
                    ProblemRowS = SampleProblems.veryDifficultProblem();
                    break;

                case ProblemLevels.ExtremelyDifficult:
                    // http://elmo.sbs.arizona.edu/sandiway/sudoku/examples.html
                    // EXTREMELY difficult.
                    // solved quickly using no = 100, me = 19,000
                    ProblemRowS = SampleProblems.extremelyDifficultProblem();
                    break;

                case ProblemLevels.MostDifficult:
                    // http://elmo.sbs.arizona.edu/sandiway/sudoku/examples.html
                    // most difficult problem found by Internet search.
                    // solved eventually using no = 100, me = 5,000.
                    // solution when seed = 577 (i.e., 577 attempts ~ 20 min.)
                    ProblemRowS = SampleProblems.mostDifficultProblem();
                    break;
            }

            return _problem;
        }


        //public static Problem Convert(FullBoard board)
        //{
        //    _problem = new Problem();
        //    ProblemRowS[0] = new int[9];// { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        //    ProblemRowS[1] = new int[9];// { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        //    ProblemRowS[2] = new int[9];// { 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        //    ProblemRowS[3] = new int[9];//  { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        //    ProblemRowS[4] = new int[9];//  { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        //    ProblemRowS[5] = new int[9];//  { 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        //    ProblemRowS[6] = new int[9];// { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        //    ProblemRowS[7] = new int[9];// { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        //    ProblemRowS[8] = new int[9];// { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        //    foreach (var cell in board.BoardList)
        //    {
        //        if (!cell.Value.HasValue) continue;
        //        _problem.Rows[cell.XCoordinate - 1][cell.YCoordinate - 1] = cell.Value.Value;
        //    }
        //    return _problem;
        //}

        //public static FullBoard Convert(Problem problem)
        //{
        //    FullBoard toReturn = new FullBoard();
        //    int rowCnt = 0;

        //    foreach (var problemRow in problem.Rows)
        //    {
        //        int cellIndex = 0;
        //        foreach (var probCell in problemRow)
        //        {
        //            var c = new Cell()
        //            {
        //                XCoordinate = rowCnt + 1,
        //                YCoordinate = cellIndex + 1,
        //                BlockNumber = Constants.BlockSize * (rowCnt / Constants.BlockSize) + (cellIndex / Constants.BlockSize) + 1,
        //                Value = probCell
        //            };
        //            toReturn.BoardList.Add(c);
        //            cellIndex++;
        //        }
        //        rowCnt++;
        //    }
        //    return toReturn;
        //}

        //public static Problem Convert(List<Cell> boardBoardList)
        //{
        //    _problem = new Problem();
        //    ProblemRowS[0] = new int[9]; ProblemRowS[1] = new int[9]; ProblemRowS[2] = new int[9];

        //    ProblemRowS[3] = new int[9]; ProblemRowS[4] = new int[9]; ProblemRowS[5] = new int[9];

        //    ProblemRowS[6] = new int[9]; ProblemRowS[7] = new int[9]; ProblemRowS[8] = new int[9];

        //    foreach (var cell in boardBoardList)
        //    {
        //        if (!cell.Value.HasValue) continue;
        //        ProblemRowS[cell.XCoordinate - 1][cell.YCoordinate - 1] = cell.Value.Value;
        //    }

        //    return _problem;
        //}
    }
}
