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
        public EvolutionStats stats = new EvolutionStats();

        //  private List<Organism> Hive;
        //  private OrganismBase TheBest;

        /// <summary>
        /// Attempts find an optimal solution to a sudoku puzzle with the a hive of organisms
        /// The organisms each have a total number of allowed Epochs (generations) to find the optimal solution. 
        /// If after the max epochs is reached without an optimal solution, they are culled and another hive is started within the maximum number of specified extinctions
        /// NOTE: Combinatorial evolution doesn't guarantee and optimal solution will be found.
        /// </summary>
        /// <param name="problem">int[][]</param>
        /// <param name="numOrganisms">how many virtual organism are in the hive at a given time, each representing a possible solution</param>
        /// <param name="maxEpochs">number of iterations (generations) each organism can run through to find the optimal solution</param>
        /// <param name="maxExtinctions">if an optimal solution isn't found after the maxEpochs, all organisms are killed and the cycle is restarted if not more than maxExtinctions.</param>
        /// <returns>The best solution (int[][]) found within the specified time.</returns>
        public async Task<int[][]> SolveWithinExtinctions(int[][] problem, int numOrganisms, int maxEpochs, int maxExtinctions)
        {
            stats.organismsBorn += numOrganisms;
            OriginalMatrix = SudokuTool.DuplicateMatrix(problem);
            // wrapper for SolveEvo()
            var err = int.MaxValue;
            var seed = 0;
            int[][] bestSolution = null;
            var attempt = 0;
            stats.extinctions = attempt;
            // generate a new hive of organisms with a number of epochs to try for an error-free solution or until we've killed them off too often
            while (err != 0 && attempt < maxExtinctions)
            {
                _sb.AppendLine("\nseed = " + seed);
                _rnd = new Random(seed);
                bestSolution = await SolveAsync(problem, numOrganisms, maxEpochs).ConfigureAwait(true);
                err = SudokuTool.Errors(bestSolution);
                ++seed;
                ++attempt;
                stats.extinctions++;
            }

            // show some stats
            System.Diagnostics.Debug.WriteLine(
                $@"Stats | Epochs: {stats.epochsCompleted} | 
Mass Extinctions: {stats.extinctions} | Organisms Created: {stats.organismsBorn} | 
Died of Old Age: {stats.diedOfOldAge} | Random Was Best: {stats.randomWasTheBest} | 
Mutation Fail: {stats.mutationFailed} | Evolution Worked: {stats.evolutionWorked} | 
Best Mutation Ever: {stats.bestMutationsEver} | Worst Organisms Retired: {stats.culledCount} | 
Mating Pairs: {stats.matingPairs} | Babies Were Best: {stats.bestBabies} ");

            System.Diagnostics.Debug.WriteLine(
               $@"Evolution that turned out for the best: {(double)(stats.bestMutationsEver / stats.evolutionWorked) * 100 }%");

            System.Diagnostics.Debug.WriteLine(
               $@"Mating that resulted in KHAN: {stats.bestBabies / stats.matingPairs * 100}%");
            return bestSolution;
        }



        /// <summary>
        /// Core engine for finding a solution. Running this is going to spawn a hive of organisms (some percentage of workers, the rest explorers)
        /// 
        /// </summary>
        /// <param name="problem"></param>
        /// <param name="numOrganisms"></param>
        /// <param name="maxEpochs"></param>
        /// <returns></returns>
        private async Task<int[][]> SolveAsync(int[][] problem, int numOrganisms, int maxEpochs)
        {
            // start fresh
            var Hive = new List<Organism>(new Organism[numOrganisms]);

            // the best organisms in each epoch will be stored here
            var TheBest = new OrganismBase(0, null, Int32.MaxValue, 0);

            // initialize combinatorial Organisms with 90% of them workers and the rest explorers
            // Explorers always get a new random matrix to test against.
            var numWorker = (int)(numOrganisms * 0.90);
            var numExplorer = numOrganisms - numWorker;


            // fill the hive with new organisms having a random puzzle and known error count and also if it was the best yet
            for (var i = 0; i < numOrganisms; ++i)
            {
                var orgType = i < numWorker ? OrganismTypes.Worker : OrganismTypes.Explorer;

                // get a new random puzzle and count the errors
                int[][] rndMatrix = await GetRandomMatrixAsync(problem).ConfigureAwait(true);
                var currentErrors = SudokuTool.Errors(rndMatrix);

                // create a new organism, give it the random puzzle and errors
                Hive[i] = new Organism(orgType, rndMatrix, currentErrors, 0);
                stats.organismsBorn++;

                // track the best so far puzzle and copy it to TheBest
                if (currentErrors < TheBest.Error)
                {
                    TheBest.Error = currentErrors;
                    TheBest.Matrix = SudokuTool.DuplicateMatrix(rndMatrix);
                    stats.randomWasTheBest++; // this should average out at some point?
                    // Splash();
                }
            }

            // main loop - start the generations (epochs)
            var epoch = 0;
            while (epoch < maxEpochs)
            {
                // keep a little log for every 1000 generations
                if (epoch % 1000 == 0)
                {
                    _sb.AppendLine("epoch = " + epoch);
                    _sb.AppendLine("best error = " + TheBest.Error);
                }

                // if we somehow have no errors (optimal solution!) then just stop now.
                if (TheBest.Error == 0) // solution found
                    break;

                // process each organism and give explorers random solutions and evolve workers
                // (with for loop)...why not foreach? because if it ages out we want to put a new organism in place
                for (var i = 0; i < numOrganisms; ++i)
                {
                    // we treat Explorers differently than Workers. Explorers always get a new random matrix to test against. Workers evolve, mutate, age +/- or die of old age
                    switch (Hive[i].Type)
                    {
                        case OrganismTypes.Explorer:
                            // give it a new random matrix
                            int[][] rndMatrix = await GetRandomMatrixAsync(problem).ConfigureAwait(true);
                            Hive[i].Matrix = SudokuTool.DuplicateMatrix(rndMatrix);
                            Hive[i].Error = SudokuTool.Errors(rndMatrix);

                            if (Hive[i].Error < TheBest.Error)
                            {
                                TheBest.Error = Hive[i].Error;
                                TheBest.Matrix = SudokuTool.DuplicateMatrix(rndMatrix);
                                stats.randomWasTheBest++;
                                // Splash();
                            }
                            break;
                        case OrganismTypes.Worker:

                            // mutate the matrix, pick a random block and pick two random cells and swap them.
                            int[][] evolving = await EvolveMatrixAsync(problem, Hive[i].Matrix).ConfigureAwait(true);
                            var evolutionErrors = SudokuTool.Errors(evolving);

                            var probability = _rnd.NextDouble();
                            var rareMutation = probability < 0.001;
                            bool evoWorked = evolutionErrors < Hive[i].Error;

                            // if the evolution is better (natural selection) or if this is a rareMutation, take it
                            if (evoWorked || rareMutation) // better, or a mistake
                            {
                                // positive result to evolution provides refreshed genepool and resets age
                                if (evoWorked)
                                {
                                    Hive[i].Age = 0;
                                    stats.evolutionWorked++;
                                }

                                // if you didn't get the better end of the deal
                                if (rareMutation && !evoWorked)
                                    stats.mutationFailed++;

                                Hive[i].Matrix = SudokuTool.DuplicateMatrix(evolving); // by value
                                Hive[i].Error = evolutionErrors;

                                if (Hive[i].Error < TheBest.Error)
                                {
                                    TheBest.Error = Hive[i].Error;
                                    TheBest.Matrix = SudokuTool.DuplicateMatrix(evolving);
                                    stats.bestMutationsEver++;
                                    // Splash();
                                }
                            }
                            else // evolved code is not better and there was no chance for forced mutation
                            {
                                // age this organism
                                Hive[i].Age++;

                                if (Hive[i].Age > 1000) // die - if so, get a random to replace
                                {
                                    stats.diedOfOldAge++;

                                    // create a new replacement for the hive.
                                    var m = await GetRandomMatrixAsync(problem).ConfigureAwait(true);
                                    Hive[i] = new Organism(0, m, SudokuTool.Errors(m), 0);
                                    stats.organismsBorn++;
                                }
                            }
                            break;

                    }

                } // each organism


                // find the index of the best worker
                var bestWkrIndex = SudokuTool.GetBestWorkerIndex(Hive, numWorker);
                var bestWorker = Hive[bestWkrIndex];

                // find the index of the best explorer
                var bestExpIndex = SudokuTool.GetBestExplorerIndex(Hive, numWorker);
                var bestExplorer = Hive[bestExpIndex];

                // find the index of the worst worker
                var worstWkrIndex = SudokuTool.GetWorstWorkerIndex(Hive, numWorker);


                // have a  50/50 chance for each block in 2nd organism will be blended into 1st organism 
                var babyOrg = await MatingResultAsync(bestWorker.Matrix, bestExplorer.Matrix, 0.50);
                stats.matingPairs++;
                var babyErr = SudokuTool.Errors(babyOrg);
                var genNext = new Organism(OrganismTypes.Worker, babyOrg, babyErr, 0)
                {
                    // generations are only incremented if one of the parents have a GeneMarker
                    Generation = incrementGeneration(bestWorker, bestExplorer),
                    // GeneMarker = "GenX" // might use this if you happened to want to see details later.
                }; stats.organismsBorn++;

                // replace the worst worker with the next generation of worker
                Hive[worstWkrIndex] = genNext;
                stats.culledCount++;

                if (babyErr < TheBest.Error)
                {
                    TheBest.Error = babyErr;
                    TheBest.Matrix = SudokuTool.DuplicateMatrix(babyOrg);
                    stats.bestBabies++;
                    // Splash();
                }

                ++epoch;
                stats.epochsCompleted = epoch;
            } // while


            return TheBest.Matrix;
        } // SolveEvo


        /// <summary>
        /// This creates a new random solution and maps that solution onto provided problem
        /// </summary>
        /// <param name="problem">int[][]</param>
        /// <returns></returns>
        public static Task<int[][]> GetRandomMatrixAsync(int[][] problem)
        {
            // make a copy of the current puzzle
            var newRandomMatrix = SudokuTool.DuplicateMatrix(problem);


            for (var block = 0; block < Constants.BoardSize; ++block)
            {
                // fill them with 1 through 9
                var corners = SudokuTool.Corner(block);
                var vals = new List<int>(Constants.BoardSize);
                for (var i = 1; i <= Constants.BoardSize; ++i)
                    vals.Add(i);

                // shuffle each cell with a value from another cell
                for (var k = 0; k < vals.Count; ++k)
                {
                    var ri = _rnd.Next(k, vals.Count);
                    var tmp = vals[k];
                    vals[k] = vals[ri];
                    vals[ri] = tmp;
                }

                // remove the random cell values from the known cell values
                var r = corners[0];
                var c = corners[1];
                for (var i = r; i < r + 3; ++i)
                {
                    for (var j = c; j < c + 3; ++j)
                    {
                        var v = problem[i][j];
                        if (v != 0) // a fixed starting number is assumed correct
                            vals.Remove(v);
                    }
                }

                // walk thru block and add the new values to the random solution output
                var ptr = 0; // pointer into List
                for (var i = r; i < r + 3; ++i)
                {
                    for (var j = c; j < c + 3; ++j)
                    {
                        if (newRandomMatrix[i][j] == 0) // not occupied
                        {
                            var v = vals[ptr]; // get value from List
                            newRandomMatrix[i][j] = v;
                            ++ptr; // move to next value in List
                        }
                    }
                }
            } // each block, k

            return Task.FromResult(newRandomMatrix);
        } // RandomMatrix


        /// <summary>
        /// Randomly swap the values in two cells of a random block of the supplied matrix
        /// </summary>
        /// <param name="problem">the original puzzle </param>
        /// <param name="matrix">the current working solution</param>
        /// <returns></returns>
        public static Task<int[][]> EvolveMatrixAsync(int[][] problem, int[][] matrix)
        {
            // pick a random 3x3 block,
            // pick two random cells in block
            // swap values

            var result = SudokuTool.DuplicateMatrix(matrix);

            var block = _rnd.Next(0, 9); // [0,8]
            //Console.WriteLine("block = " + block);
            var corners = SudokuTool.Corner(block);
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

            return Task.FromResult(result);
        } // NeighborMatrix

        /// <summary>
        /// for each block (3x3) in matrix2, there's a 50% chance to be added into matrix1
        /// </summary>
        /// <param name="matrix1">mating partner 1</param>
        /// <param name="matrix2">mating partner 2</param>
        /// <param name="chance">0.50 or some percential double value</param>
        /// <returns>a single matrix with some relationships between both provided</returns>
        public static Task<int[][]> MatingResultAsync(int[][] matrix1, int[][] matrix2, double chance = 0.50)
        {
            var result = SudokuTool.DuplicateMatrix(matrix1);
            for (var block = 0; block < 9; ++block)
            {
                if (_rnd.NextDouble() < chance)
                {
                    var corners = SudokuTool.Corner(block);
                    for (var i = corners[0]; i < corners[0] + 3; ++i)
                    {
                        for (var j = corners[1]; j < corners[1] + 3; ++j)
                        {
                            result[i][j] = matrix2[i][j];
                        }
                    }
                }
            }
            return Task.FromResult(result);// Task.FromResult(result);
        }

        /// <summary>
        /// Just use this to get the next generation number by adding 1 to the latest generation's gen number.... only if either orgs have a GeneMarker
        /// </summary>
        /// <param name="workerOrganism"></param>
        /// <param name="explorerOrganism"></param>
        /// <returns></returns>
        public static int incrementGeneration(Organism workerOrganism, Organism explorerOrganism)
        {
            if ((workerOrganism.GeneMarker != string.Empty || explorerOrganism.GeneMarker != string.Empty))
            {
                var highest = workerOrganism.Generation < explorerOrganism.Generation
                    ? explorerOrganism.Generation : workerOrganism.Generation;

                return highest + 1;
            }
            return 1;
        }



    }
}
