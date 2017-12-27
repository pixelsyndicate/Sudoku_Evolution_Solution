using System;
using System.Collections.Generic;

namespace SudokuTools
{
    public class SudokuTool
    {
        /// <summary>
        /// Creates a copy of the provided matrix
        /// </summary>
        /// <param name="matrix">int[][]</param>
        /// <returns>another int[][] with the same values as passed in</returns>
        public static int[][] DuplicateMatrix(int[][] matrix)
        {
            var result = CreateMatrix(Constants.BoardSize, Constants.BoardSize);

            for (var i = 0; i < Constants.BoardSize; ++i)
                for (var j = 0; j < Constants.BoardSize; ++j)
                    result[i][j] = matrix[i][j];
            return result;
        }

        /// <summary>
        /// Generates a matrix (grid, or array of arrays) representing the sudoku puzzle
        /// </summary>
        /// <param name="rows">number of rows</param>
        /// <param name="columns">number of columns per row</param>
        /// <returns>an array of int[][]</returns>
        public static int[][] CreateMatrix(int rows, int columns)
        {
            var result = new int[rows][];
            for (var row = 0; row < rows; ++row)
                result[row] = new int[columns];
            return (result);
        }

     

        /// <summary>
        /// returns the row/column coordinate of the top-left cell in the block identified
        /// </summary>
        /// <param name="block">index of a 3x3 block</param>
        /// <returns>returns the (row,column) coordinate of the top-left cell</returns>
        public static int[] Corner(int block)
        {
            int r = -1, c = -1;

            if (block == 0 || block == 1 || block == 2) r = 0; // first row
            else if (block == 3 || block == 4 || block == 5) r = 3;
            else if (block == 6 || block == 7 || block == 8) r = 6;

            if (block == 0 || block == 3 || block == 6) c = 0;
            else if (block == 1 || block == 4 || block == 7) c = 3;
            else if (block == 2 || block == 5 || block == 8) c = 6;

            return new[] { r, c };
        }

        /// <summary>
        /// Returns the index of one of the nine blocks, 
        /// the one that contains the cell at the provided row/cell coordinates
        /// </summary>
        /// <param name="r">row index</param>
        /// <param name="c">column index</param>
        /// <returns>Returns the index of a block, as number left to right, top to bottom</returns>
        public static int Block(int r, int c)
        {
            if (r >= 0 && r <= 2 && c >= 0 && c <= 2)
                return 0;
            if (r >= 0 && r <= 2 && c >= 3 && c <= 5)
                return 1;
            if (r >= 0 && r <= 2 && c >= 6 && c <= 8)
                return 2;
            if (r >= 3 && r <= 5 && c >= 0 && c <= 2)
                return 3;
            if (r >= 3 && r <= 5 && c >= 3 && c <= 5)
                return 4;
            if (r >= 3 && r <= 5 && c >= 6 && c <= 8)
                return 5;
            if (r >= 6 && r <= 8 && c >= 0 && c <= 2)
                return 6;
            if (r >= 6 && r <= 8 && c >= 3 && c <= 5)
                return 7;
            if (r >= 6 && r <= 8 && c >= 6 && c <= 8)
                return 8;
            throw new Exception("Unable to find Block()");
        }

        internal static int GetBestWorkerIndex(List<Organism> hive, int workerCount)
        {
            var previousError = hive[0].Error;
            var bestWkrIndex = 0;
            for (var i = 0; i < workerCount; ++i)
            {
                if (hive[i].Error < previousError)
                {
                    previousError = hive[i].Error;
                    bestWkrIndex = i;
                }
            }
            return bestWkrIndex;
        }

        internal static int GetBestExplorerIndex(List<Organism> hive, int numWorker)
        {
            var previousError = hive[numWorker].Error;
            var bestExpIndex = numWorker;
            for (var i = numWorker; i < hive.Count; ++i)
            {
                if (hive[i].Error < previousError)
                {
                    previousError = hive[i].Error;
                    bestExpIndex = i;
                }
            }
            return bestExpIndex;
        }

        internal static int GetWorstWorkerIndex(List<Organism> hive, int numWorker)
        {
            var worstWkrIndex = 0; // index of worst worker
            var previousError = hive[0].Error;
            for (var i = 0; i < numWorker; ++i)
            {
                if (hive[i].Error > previousError)
                {
                    previousError = hive[i].Error;
                    worstWkrIndex = i;
                }
            }
            return worstWkrIndex;
        }
    }

}
