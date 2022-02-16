namespace cs480_hw1;

using KVPair = KeyValuePair<Tuple<int, int>, int>;

public class Agent {
  
  private readonly State goalState_;
  private readonly State initState_;
  
  private List<Node> frontierNodes_ = new();
  private List<Node> exploredNodes_ = new();
  
  private Node? solutionNode_ = null;
  private int nodesExpanded_ = 0;

  public Agent(State goalState, State initState) {
    if (!goalState.DimsMatch(initState)) {
      // Ensures that both goalState and initState match in dimensions
      throw new Exception("Dimensions of state arrays must match");
    }
    goalState_ = goalState;
    initState_ = initState;
  }

  public List<Action>? BreadthFirstSearch() {
    Reset();
    
    while (frontierNodes_.Count != 0) {
      var nextNode = frontierNodes_.Min()!; // finds min depth value in nodes
      exploredNodes_.Add(nextNode);

      if (nextNode.state == goalState_) {
        solutionNode_ = nextNode;
        break;
      }
      
      var expandedNodes = Expand(nextNode);
      if (expandedNodes.Count > 0) {
        ++nodesExpanded_;
        frontierNodes_.AddRange(expandedNodes);
      }
      
      var index = frontierNodes_.IndexOf(nextNode);
      frontierNodes_.RemoveAt(index);
      Console.WriteLine(expandedNodes.Count);
      Console.WriteLine(frontierNodes_.Count);
    }

    if (solutionNode_ is not null) {
      List<Action> solutionActions = new();
      Node node = solutionNode_;
      while (node.parent is not null) {
        solutionActions.Add(node.action);
        node = node.parent;
      }
      solutionActions.Reverse();
      return solutionActions;
    } else {
      return null;
    }
  }

  public List<Action>? DepthFirstSearch() {
    Reset();
    
    while (frontierNodes_.Count != 0) {
      var nextNode = frontierNodes_.Max()!; // finds min depth value in nodes
      exploredNodes_.Add(nextNode);

      if (nextNode.state == goalState_) {
        solutionNode_ = nextNode;
        break;
      }
      
      var expandedNodes = Expand(nextNode);
      if (expandedNodes.Count > 0) {
        ++nodesExpanded_;
        frontierNodes_.AddRange(expandedNodes);
      }
      
      var index = frontierNodes_.IndexOf(nextNode);
      Console.WriteLine(index);
      frontierNodes_.RemoveAt(index);
      Console.WriteLine(expandedNodes.Count);
      Console.WriteLine(frontierNodes_.Count);
    }

    if (solutionNode_ is not null) {
      List<Action> solutionActions = new();
      Node node = solutionNode_;
      while (node.parent is not null) {
        solutionActions.Add(solutionNode_.action);
        node = node.parent;
      }
      solutionActions.Reverse();
      return solutionActions;
    } else {
      return null;
    }
  }

  private List<Node> Expand(Node node) {
    List<Node> expandedNodes = new();
    
    var zeroTile = node.state.GetKeyValuePairFromValue(0)!.Value; // Get key/value pair from value
    bool up = node.state[zeroTile.Key.Item1 - 1, zeroTile.Key.Item2].HasValue;
    bool down = node.state[zeroTile.Key.Item1 + 1, zeroTile.Key.Item2].HasValue;
    bool left = node.state[zeroTile.Key.Item1, zeroTile.Key.Item2 - 1].HasValue;
    bool right = node.state[zeroTile.Key.Item1, zeroTile.Key.Item2 + 1].HasValue;

    if (up) {
      var newState = (State)node.state.Clone();
      newState[zeroTile.Key.Item1 - 1, zeroTile.Key.Item2] = node.state[zeroTile.Key.Item1, zeroTile.Key.Item2];
      newState[zeroTile.Key.Item1, zeroTile.Key.Item2] = node.state[zeroTile.Key.Item1 - 1, zeroTile.Key.Item2];

      var newNode = new Node(newState, node, Action.Up, 1, node.depth + 1);
      if (!frontierNodes_.Contains(newNode) && !exploredNodes_.Contains(newNode)) {
        expandedNodes.Add(newNode);
      }
    }
    if (down) {
      var newState = (State)node.state.Clone();
      newState[zeroTile.Key.Item1 + 1, zeroTile.Key.Item2] = node.state[zeroTile.Key.Item1, zeroTile.Key.Item2];
      newState[zeroTile.Key.Item1, zeroTile.Key.Item2] = node.state[zeroTile.Key.Item1 + 1, zeroTile.Key.Item2];

      var newNode = new Node(newState, node, Action.Down, 1, node.depth + 1);
      if (!frontierNodes_.Contains(newNode) && !exploredNodes_.Contains(newNode)) {
        expandedNodes.Add(newNode);
      }
    }
    if (left) {
      var newState = (State)node.state.Clone();
      newState[zeroTile.Key.Item1, zeroTile.Key.Item2 - 1] = node.state[zeroTile.Key.Item1, zeroTile.Key.Item2];
      newState[zeroTile.Key.Item1, zeroTile.Key.Item2] = node.state[zeroTile.Key.Item1, zeroTile.Key.Item2 - 1];

      var newNode = new Node(newState, node, Action.Left, 1, node.depth + 1);
      if (!frontierNodes_.Contains(newNode) && !exploredNodes_.Contains(newNode)) {
        expandedNodes.Add(newNode);
      }
    }
    if (right) {
      var newState = (State)node.state.Clone();
      newState[zeroTile.Key.Item1, zeroTile.Key.Item2 + 1] = node.state[zeroTile.Key.Item1, zeroTile.Key.Item2];
      newState[zeroTile.Key.Item1, zeroTile.Key.Item2] = node.state[zeroTile.Key.Item1, zeroTile.Key.Item2 + 1];

      var newNode = new Node(newState, node, Action.Right, 1, node.depth + 1);
      if (!frontierNodes_.Contains(newNode) && !exploredNodes_.Contains(newNode)) {
        expandedNodes.Add(newNode);
      }
    }
    
    return expandedNodes;
  }

  private void Reset() {
    frontierNodes_.Clear();
    exploredNodes_.Clear();
    solutionNode_ = null;
    nodesExpanded_ = 0;
    
    frontierNodes_.Add(new Node(initState_, null, Action.None, 0, 0));
  }
}