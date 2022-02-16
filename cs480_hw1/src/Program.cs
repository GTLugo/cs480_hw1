using cs480_hw1;
using Action = cs480_hw1.Action;

var goalState = new State(new [,]{
  { 1,  2,  3,  4},
  { 5,  6,  7,  8},
  { 9, 10, 11, 12},
  {13, 14, 15,  0}
});
var initState = new State(new [,]{
  { 1,  0,  2,  4},
  { 5,  7,  3,  8},
  { 9,  6, 11, 12},
  {13, 10, 14, 15}
});
var agent = new Agent(goalState, initState);

Console.WriteLine("Goal State:\n" + goalState);
Console.WriteLine("Initial State:\n" + initState);

var solutionActions = agent.BreadthFirstSearch();

Console.WriteLine(solutionActions.Count);
foreach (var action in solutionActions) {
  switch (action) {
    case Action.Up:    Console.Write('U'); break;
    case Action.Down:  Console.Write('D'); break;
    case Action.Left:  Console.Write('L'); break;
    case Action.Right: Console.Write('R'); break;
  }
}
Console.WriteLine();
//Console.WriteLine(agent.DepthFirstSearch());

// Console.Write("Press any key to continue...");
// Console.ReadKey();