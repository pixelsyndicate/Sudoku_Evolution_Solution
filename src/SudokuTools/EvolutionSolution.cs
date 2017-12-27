using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SudokuTools
{
    public class EvolutionSolution
    {
        private static Random _rnd = new Random();
        private static StringBuilder _sb = new StringBuilder();
        private static int[][] OriginalMatrix;
        private EvolutionStats stats = new EvolutionStats { extinctions = 0, randomWasTheBest = 0, naturalMisselection=0, bestMutationsEver=0, oldAgeKilled=0, culledCount=0, bestBabies=0, epochsCompleted=0 };

        private List<Organism> Hive;
        private OrganismBase TheBest;

        public async Task<int[][]> SolveWithinExtinctions(int[][] problem, int numOrganisms, int maxEpochs, int maxExtinctions)
        {

            OriginalMatrix = DuplicateMatrix(problem);
            // wrapper for SolveEvo()
            var err = int.MaxValue;
            var seed = 0;
            int[][] bestSolution = null;
            var attempt = 0;
            while (err != 0 && attempt < maxExtinctions)
            {
                stats.extinctions = attempt;
                _sb.AppendLine("\nseed = " + seed);
                _rnd = new Random(seed);
                bestSolution = await SolveAsync(problem, numOrganisms, maxEpochs).ConfigureAwait(true); // things, maxEpochs
                err = Errors(bestSolution);
                ++seed;
                ++attempt;
            }

            return bestSolution;
        }

        private static int[][] DuplicateMatrix(int[][] matrix)
        {
            var result = CreateMatrix(Constants.BoardSize, Constants.BoardSize);

            for (var i = 0; i < Constants.BoardSize; ++i)
                for (var j = 0; j < Constants.BoardSize; ++j)
                    result[i][j] = matrix[i][j];
            return result;
        }
        private static int[][] CreateMatrix(int r, int c)
        {
            var result = new int[r][];
            for (var row = 0; row < r; ++row)
                result[row] = new int[c];
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

        public static async Task<int[][]> GetRandomMatrixAsync(int[][] problem)
        {
            // fill each 3x3 block with 1-9
            var result = DuplicateMatrix(problem);

            for (var block = 0; block < 9; ++block)
            {
                var corners = Corner(block);
                var vals = new List<int>(9);
                for (var i = 1; i <= 9; ++i)
                    vals.Add(i);

                // shuffle
                for (var k = 0; k < vals.Count; ++k)
                {
                    var ri = _rnd.Next(k, vals.Count);
                    var tmp = vals[k];
                    vals[k] = vals[ri];
                    vals[ri] = tmp;
                }

                // walk thru block and remove from list starting numbers in problem
                var r = corners[0];
                var c = corners[1];
                for (var i = r; i < r + 3; ++i)
                {
                    for (var j = c; j < c + 3; ++j)
                    {
                        var v = problem[i][j];
                        if (v != 0) // a fixed starting number
                            vals.Remove(v);
                    }
                }

                // walk thru block and add values
                var ptr = 0; // pointer into List
                for (var i = r; i < r + 3; ++i)
                {
                    for (var j = c; j < c + 3; ++j)
                    {
                        if (result[i][j] == 0) // not occupied
                        {
                            var v = vals[ptr]; // get value from List
                            result[i][j] = v;
                            ++ptr; // move to next value in List
                        }
                    }
                }
            } // each block, k

            return result;
        } // RandomMatrix

        public static async Task<int[][]> NeighborMatrixAsync(int[][] problem, int[][] matrix)
        {
            // pick a random 3x3 block,
            // pick two random cells in block
            // swap values

            var result = DuplicateMatrix(matrix);

            var block = _rnd.Next(0, 9); // [0,8]
            //Console.WriteLine("block = " + block);
            var corners = Corner(block);
            //Console.WriteLine("corners = " + corners[0] + " " + corners[1]);


            // look for at minimum 2 cells in the neighbor matrix that might be useful in the problem
            var cells = new List<int[]>();
            for (var i = corners[0]; i < corners[0] + 3; ++i)
            {
                for (var j = corners[1]; j < corners[1] + 3; ++j)
                {
                    //Console.WriteLine("problem " + i + " " + j + " = " + problem[i][j]);
                    if (problem[i][j] == 0) // a non-fixed value
                    {
                        cells.Add(new[] { i, j });
                    }
                }
            }

            if (cells.Count < 2)
            {
                _sb.AppendLine($"Found only {cells.Count} useful cell in the neighbor.");
                throw new Exception("block " + block + " doesn't have two values to swap!");
            }

            // pick two. suppose there are 4 possible cells 0,1,2,3
            var k1 = _rnd.Next(0, cells.Count); // 0,1,2,3
            var inc = _rnd.Next(1, cells.Count); // 1,2,3
            var k2 = (k1 + inc) % cells.Count;
            //Console.WriteLine("k1 k2 = " + k1 + " " + k2);

            var r1 = cells[k1][0];
            var c1 = cells[k1][1];
            var r2 = cells[k2][0];
            var c2 = cells[k2][1];

            //Console.WriteLine("r1 c1 = " + r1 + " " + c1);
            //Console.WriteLine("r2 c2 = " + r2 + " " + c2);

            var tmp = result[r1][c1];
            result[r1][c1] = result[r2][c2];
            result[r2][c2] = tmp;

            return result;
        } // NeighborMatrix

        /// <summary>
        /// 50/50 chance for each block in organism 2 will be blended into organism 1
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <param name="chance">50%</param>
        /// <returns>a single matrix with some relationships between both provided</returns>
        public static async Task<int[][]> ChromoCrossoverAsync(int[][] matrix1, int[][] matrix2, double chance = 0.50)
        {
            var result = DuplicateMatrix(matrix1);

            for (var block = 0; block < 9; ++block)
            {
                if (_rnd.NextDouble() < chance)
                {
                    var corners = Corner(block);
                    for (var i = corners[0]; i < corners[0] + 3; ++i)
                        for (var j = corners[1]; j < corners[1] + 3; ++j)
                            result[i][j] = matrix2[i][j];
                }
            }
            return await Task.FromResult(result);// Task.FromResult(result);
        }

        /// <summary>
        /// Just use this to get the next generation number by adding 1 to the latest generation's gen number.
        /// </summary>
        /// <param name="workerOrganism"></param>
        /// <param name="explorerOrganism"></param>
        /// <returns></returns>
        public static int incrementGeneration(Organism workerOrganism, Organism explorerOrganism)
        {
            if ((workerOrganism.GeneMarker == "GenX" || explorerOrganism.GeneMarker == "GenX"))
            {
                var highest = workerOrganism.Generation < explorerOrganism.Generation
                    ? explorerOrganism.Generation : workerOrganism.Generation;

                return highest + 1;
            }
            return 1;
        }

        /// <summary>
        /// inspects each row and each column, taking count each time a number is missing
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static int Errors(int[][] matrix)
        {
            var err = 0;
            // assumes blocks are OK (one each 1-9)

            // rows error
            for (var i = 0; i < Constants.BoardSize; ++i) // each row
            {
                int[] counts = new int[Constants.BoardSize]; // [0] = count of 1s, [1] = count of 2s


                for (var j = 0; j < Constants.BoardSize; ++j) // walk down column of curr row
                {
                    var v = matrix[i][j]; // 1 to 9
                    ++counts[v - 1];
                }

                for (var k = 0; k < Constants.BoardSize; ++k) // number missing 
                {
                    if (counts[k] == 0)
                        ++err;
                }
            } // each row

            // columns error
            for (var j = 0; j < Constants.BoardSize; ++j) // each column
            {
                var counts = new int[Constants.BoardSize]; // [0] = count of 1s, [1] = count of 2s

                for (var i = 0; i < Constants.BoardSize; ++i) // walk down 
                {
                    var v = matrix[i][j]; // 1 to 9
                    ++counts[v - 1]; // counts[0-8]
                }

                for (var k = 0; k < Constants.BoardSize; ++k) // number missing in the colum
                {
                    if (counts[k] == 0)
                        ++err;
                }
            } // each column

            return err;
        } // Error

        private async Task<int[][]> SolveAsync(int[][] problem, int numOrganisms, int maxEpochs)
        {
            // initialize combinatorial Organisms
            var numWorker = (int)(numOrganisms * 0.90);
            var numExplorer = numOrganisms - numWorker;
            Hive = new List<Organism>(new Organism[numOrganisms]);
            TheBest = new OrganismBase(0, null, Int32.MaxValue, 0);

            // fill the hive with new organisms
            for (var i = 0; i < numOrganisms; ++i)
            {
                var orgType = i < numWorker ? OrganismTypes.Worker : OrganismTypes.Explorer;

                var rndMatrix = await GetRandomMatrixAsync(problem).ConfigureAwait(true);
                var thisErr = Errors(rndMatrix);

                Hive[i] = new Organism(orgType, rndMatrix, thisErr, 0);

                if (thisErr < TheBest.Error)
                {
                    TheBest.Error = thisErr;
                    TheBest.Matrix = DuplicateMatrix(rndMatrix);
                    stats.randomWasTheBest++;
                   // Splash();
                }
            }

            // main loop
            var epoch = 0;
            while (epoch < maxEpochs)
            {
                if (epoch % 1000 == 0)
                {
                    _sb.AppendLine("epoch = " + epoch);
                    _sb.AppendLine("best error = " + TheBest.Error);
                }

                if (TheBest.Error == 0) // solution found
                    break;

                // process each organism
                for (var i = 0; i < numOrganisms; ++i)
                {

                    if (Hive[i].Type == OrganismTypes.Worker) // worker
                    {

                        // get a neighbor matrix, pick a random block and pick two random cells and swap them.
                        var neighbor = await NeighborMatrixAsync(problem, Hive[i].Matrix).ConfigureAwait(true);
                        var neighborErrors = Errors(neighbor);

                        var prob = _rnd.NextDouble();
                        var mutation = prob < 0.001;
                        if (neighborErrors < Hive[i].Error || mutation) // better, or a mistake
                        {
                            // refreshed genepool resets age
                            if (neighborErrors < Hive[i].Error)
                                Hive[i].Age = 0;

                            // if you didn't get the better end of the deal
                            if (mutation && neighborErrors > Hive[i].Error)
                                stats.naturalMisselection++;

                            Hive[i].Matrix = DuplicateMatrix(neighbor); // by value
                            Hive[i].Error = neighborErrors;

                            if (Hive[i].Error < TheBest.Error)
                            {
                                TheBest.Error = Hive[i].Error;
                                TheBest.Matrix = DuplicateMatrix(neighbor);
                                stats.bestMutationsEver++;
                               // Splash();
                            }
                        }
                        else // neighbor is not better
                        {
                            // age this organism
                            Hive[i].Age++;
                            if (Hive[i].Age > 1000) // die - if so, get a random to replace
                            {
                                stats.oldAgeKilled++;
                                var m = await GetRandomMatrixAsync(problem).ConfigureAwait(true);
                                Hive[i] = new Organism(0, m, Errors(m), 0);
                            }
                        }
                    } // worker
                    else if (Hive[i].Type == OrganismTypes.Explorer) // explorer
                    {
                        var rndMatrix = await GetRandomMatrixAsync(problem).ConfigureAwait(true);
                        Hive[i].Matrix = DuplicateMatrix(rndMatrix);
                        Hive[i].Error = Errors(rndMatrix);

                        if (Hive[i].Error < TheBest.Error)
                        {
                            TheBest.Error = Hive[i].Error;
                            TheBest.Matrix = DuplicateMatrix(rndMatrix);
                            stats.randomWasTheBest++;
                           // Splash();
                        }
                    }
                } // each organism

                // merge best worker with best explorer into worst worker
                var bestWkrIndex = 0; // index of best worker
                var smallestWorkerError = Hive[0].Error;
                for (var i = 0; i < numWorker; ++i)
                {
                    if (Hive[i].Error < smallestWorkerError)
                    {
                        smallestWorkerError = Hive[i].Error;
                        bestWkrIndex = i;
                    }
                }

                var bestExpIndex = numWorker; // index of best explorer
                var smallestExplorerError = Hive[numWorker].Error;
                for (var i = numWorker; i < numOrganisms; ++i)
                {
                    if (Hive[i].Error < smallestExplorerError)
                    {
                        smallestExplorerError = Hive[i].Error;
                        bestExpIndex = i;
                    }
                }

                var worstWkrIndex = 0; // index of worst worker
                var largestWorkerError = Hive[0].Error;
                for (var i = 0; i < numWorker; ++i)
                {
                    if (Hive[i].Error > largestWorkerError)
                    {
                        largestWorkerError = Hive[i].Error;
                        worstWkrIndex = i;
                    }
                }

                
                var bestWorker = Hive[bestWkrIndex];
                var bestExplorer = Hive[bestExpIndex];
                // have a  50/50 chance for each block in 2nd organism will be blended into 1st organism 
                var merged = await ChromoCrossoverAsync(bestWorker.Matrix, bestExplorer.Matrix, 0.50);
                var mergedErr = Errors(merged);
                var genNext = new Organism(OrganismTypes.Worker, merged, mergedErr, 0, "GenX")
                {
                    Generation = incrementGeneration(bestWorker, bestExplorer)
                };

                // replace the worst worker with the next generation of worker
                Hive[worstWkrIndex] = genNext;
                stats.culledCount++;


                if (mergedErr < TheBest.Error)
                {
                    TheBest.Error = mergedErr;
                    TheBest.Matrix = DuplicateMatrix(merged);
                    stats.bestBabies++;
                   // Splash();
                }

                ++epoch;
                stats.epochsCompleted = epoch;
            } // while
            return TheBest.Matrix;
        } // SolveEvo
    }
}
