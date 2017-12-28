# Sudoku Evolution Solution

This is a Combinatorial Evolution C# .NET project following the examples from an article written by James McCaffrey in MSDN Magazine, November 2016.

## Combinatorial Evolution Optimization
	• Combinatorial Evolution optimization uses ideas from several bio-inspired algorithms.
		○ The algorithm maintains a collection of virtual organisms.
			§ Each organism represents a possible solution to the problem.
	• Combinatorial evolution is an iterative process.
		○ Each iteration is called an epoch.
			§ In each epoch, every organism attempts to find a better solution by examining a new possible solution.
			§ After all organisms have had a chance to improve, two good organism-solutions are selected and used to give birth to a new organism, which replaces a poor solution.
		○ If an optimal solution isn't found after some maxEpochs time, the algorithm is restarted by killing all the organisms and creating a new population.
	• Combinatorial evolution doesn't guarantee and optimal solution will be found.

### The Organism
The Organism: Represents a possible solution to the problem
	• Type: 0=Worker, 1=Explorer
	• Matrix: represents a possible solution (int[][] or array-of-arrays)
	• Error: the number of constrains that are violated for this solution (0 = optimal)
	• Age: controls whether or organism dies in each epoch.

### Measure of Error
Measure of Error : the sum of missing numbers in rows and columns.
	this is displayed for the best organism. 
	Tends to get a very good solution (error=2) very quickly, but then gets stuck. 
	The restart process is one mechanism to combat this characteristic, and is a common technique in many optimization algorithms. 

### Steps in the Solve method(s)
Steps in the SolveEvo() method.

	• Create an organism collection "hive" capable of holding x # of organisms.
	• Create a new organism for each hive slot
		○ Mark the first one as a Worker (0) (workers look for better solutions by examining mutations of their solution)
		○ Mark the others as Explorer (1) (explorers search for better solutions randomly)
		○ Calls RandomMatrix(problem) to…
			§ Make a copy of the problem and call it Result.
			§ Loop through each of the 9 Block (Block = 3x3 cube)
				□ Fill the block with 1-9
				□ Shuffle the numbers around randomly
				□ Remove those random values from cells that the problem already had with known correct values.
			§ Take the remaining numbers and put them in the Result and return it as a possible solution.
		○ Test for the Errors sum for this possible solution
		○ Place the organism in the hive.
		○ If the Errors is better than all previous solutions, mark that solution as The Best.
	• If the best solution found has zero error (optimal solution), one of the organisms have an answer. End the search.
	• Else loop through all of the hive organisms and search for better solutions
		○  If is 'worker' organism then: Evolve or Mutate
			§ Take a look at a random neighboring block.
			§ Take up to 2 random cells in that neighbor block that happen to be needed in the problem
			§ Swaps the values in those two cells
			§ Evaluates if the Errors are better, same or worse than worker organism.
				□ If less errors (successful evolution) then
					® Keep that resulting solution and sets the organisms age to 0
					® If the Errors is better than all previous solutions, mark that solution as The Best.
				□ If not less errors (no better or worse)
					® Age organism by 1 
					® If Age is > 1000, 
						◊ Organims dies  
						◊ Get a new RandomMatrix(problem) result
						◊ Hive position is filled with new Worker Organism having the random result
		○ Else an 'explorer' organism 
			§ Get a new RandomMatrix(problem) result
			§ Check the Errors
			§ If the Errors is better than all previous solutions, mark that solution as The Best.
	• Merge (mate) the best worker and best explorer and replace the worst worker
		○ For each block (1-9) of the best explorer, there's a 50/50 chance to copy its contents to the best workers block.
		○ That child solution is used to replace the worst worker organism.

