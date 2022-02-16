* Built with C# and .NET Core 6

* A screenshot of the results is provided titled "output.png"

* This program implements a Breadth-First Search and a Depth-First Search. The Breadth is the fastest by far and takes up the least amount of memory and node expansions.
The output for the Depth-First Search function takes too long to complete in any reasonable amount of time as it must brute force search through every possibility in a single branch,
which could be millions of iterations deep, despite the actual solution only needing to be a few iterations deep. Because of the solution requiring such a shallow search, the Breadth-First
Search is by far the best choice.