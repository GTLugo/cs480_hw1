namespace cs480_hw1;

using KVPair = KeyValuePair<Tuple<int, int>, int>;

public class Agent {
  
  private readonly State goalState_;
  private readonly State initState_;
  
  private List<Node> frontierNodes_ = new();
  private List<Node> exploredNodes_ = new();
  
  private Node? solutionNode_ = null;
  private int nodesExpanded_ = 0;
  
  //private const int depthCutoff_ = 10;

  public int NodesExpanded => nodesExpanded_;

  public Agent(State goalState, State initState) {
    if (!goalState.DimsMatch(initState)) {
      // Ensures that both goalState and initState match in dimensions
      throw new Exception("Dimensions of state arrays must match");
    }
    goalState_ = goalState;
    initState_ = initState;
  }

  public List<Action> BreadthFirstSearch() {
    return Search(frontierNodes_.Min);
  }

  public List<Action> DepthFirstSearch() {
    return Search(frontierNodes_.Max);
  }

  // The parameter to the search function acts as the method
  // for obtaining the priority node in the list of nodes.
  // This allows for eliminating redundant code while preserving functionality,
  // while only sacrificing a little bit of readability.
  public List<Action> Search(Func<Node?> priorityNodeFunc) {
    Reset();
    
    while (frontierNodes_.Count != 0) {
      // finds min depth value in nodes
      var nextNode = priorityNodeFunc()!;
      exploredNodes_.Add(nextNode);
      frontierNodes_.Remove(nextNode);
      
      // Check for goal state
      if (nextNode.state == goalState_) {
        solutionNode_ = nextNode;
        break;
      }
      
      // Expand nodes
      var expandedNodes = Expand(nextNode);
      if (expandedNodes.Count > 0) {
        ++nodesExpanded_;
        frontierNodes_.AddRange(expandedNodes);
      }
    }
    
    // Follow trail back up the tree to construct list of actions
    List<Action> solutionActions = new();
    if (solutionNode_ is not null) {
      Node node = solutionNode_;
      while (node.parent is not null) {
        solutionActions.Add(node.action);
        node = node.parent;
      }
      solutionActions.Reverse();
    }
    return solutionActions;
  }

  private List<Node> Expand(Node node) {
    List<Node> expandedNodes = new();
    
    // Find valid movements of tile
    var zeroTile = node.state.GetKeyValuePairFromValue(0)!.Value; // Get key/value pair from value
    bool up = node.state[zeroTile.Key.Item1 - 1, zeroTile.Key.Item2].HasValue;
    bool down = node.state[zeroTile.Key.Item1 + 1, zeroTile.Key.Item2].HasValue;
    bool left = node.state[zeroTile.Key.Item1, zeroTile.Key.Item2 - 1].HasValue;
    bool right = node.state[zeroTile.Key.Item1, zeroTile.Key.Item2 + 1].HasValue;
    
    if (up) {
      // Swap tile and blank tile
      var newState = (State)node.state.Clone();
      newState[zeroTile.Key.Item1 - 1, zeroTile.Key.Item2] = node.state[zeroTile.Key.Item1, zeroTile.Key.Item2];
      newState[zeroTile.Key.Item1, zeroTile.Key.Item2] = node.state[zeroTile.Key.Item1 - 1, zeroTile.Key.Item2];
      
      // Check to see if node should be added to frontier
      var newNode = new Node(newState, node, Action.Up, 1, node.depth + 1);
      if (!frontierNodes_.Contains(newNode) && !exploredNodes_.Contains(newNode) /*&& newNode.depth < depthCutoff_*/) {
        expandedNodes.Add(newNode);
      }
    }
    if (down) {
      var newState = (State)node.state.Clone();
      newState[zeroTile.Key.Item1 + 1, zeroTile.Key.Item2] = node.state[zeroTile.Key.Item1, zeroTile.Key.Item2];
      newState[zeroTile.Key.Item1, zeroTile.Key.Item2] = node.state[zeroTile.Key.Item1 + 1, zeroTile.Key.Item2];

      var newNode = new Node(newState, node, Action.Down, 1, node.depth + 1);
      if (!frontierNodes_.Contains(newNode) && !exploredNodes_.Contains(newNode) /*&& newNode.depth < depthCutoff_*/) {
        expandedNodes.Add(newNode);
      }
    }
    if (left) {
      var newState = (State)node.state.Clone();
      newState[zeroTile.Key.Item1, zeroTile.Key.Item2 - 1] = node.state[zeroTile.Key.Item1, zeroTile.Key.Item2];
      newState[zeroTile.Key.Item1, zeroTile.Key.Item2] = node.state[zeroTile.Key.Item1, zeroTile.Key.Item2 - 1];

      var newNode = new Node(newState, node, Action.Left, 1, node.depth + 1);
      if (!frontierNodes_.Contains(newNode) && !exploredNodes_.Contains(newNode) /*&& newNode.depth < depthCutoff_*/) {
        expandedNodes.Add(newNode);
      }
    }
    if (right) {
      var newState = (State)node.state.Clone();
      newState[zeroTile.Key.Item1, zeroTile.Key.Item2 + 1] = node.state[zeroTile.Key.Item1, zeroTile.Key.Item2];
      newState[zeroTile.Key.Item1, zeroTile.Key.Item2] = node.state[zeroTile.Key.Item1, zeroTile.Key.Item2 + 1];

      var newNode = new Node(newState, node, Action.Right, 1, node.depth + 1);
      if (!frontierNodes_.Contains(newNode) && !exploredNodes_.Contains(newNode) /*&& newNode.depth < depthCutoff_*/) {
        expandedNodes.Add(newNode);
      }
    }
    
    return expandedNodes;
  }
  
  // Reset agent to prepare for search
  private void Reset() {
    frontierNodes_.Clear();
    exploredNodes_.Clear();
    solutionNode_ = null;
    nodesExpanded_ = 0;
    
    frontierNodes_.Add(new Node(initState_, null, Action.None, 0, 0));
  }
}