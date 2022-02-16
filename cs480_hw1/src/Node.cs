namespace cs480_hw1; 

// The node acts as a single part of the search tree
public class Node : IComparable {
  public readonly State state;
  public readonly Node? parent;
  public readonly Action action;
  public readonly int cost;
  public readonly int depth;

  public Node(State state, Node? parent, Action action, int cost, int depth) {
    this.state = state;
    this.parent = parent;
    this.action = action;
    this.cost = cost;
    this.depth = depth;
  }
  
  public override bool Equals(object? obj) => Equals(obj as Node ?? throw new InvalidOperationException());
  public bool Equals(Node s) => state == s.state;
  public override int GetHashCode() => state.GetHashCode();

  public int CompareTo(object? obj) {
    return CompareTo(obj as Node ?? throw new InvalidOperationException());
  }

  public int CompareTo(Node node) {
    return depth.CompareTo(node.depth);
  }

  public static bool operator ==(Node? left, Node? right) {
    if (left is null || right is null) {
      return false;
    }

    return left.Equals(right);
  }
  public static bool operator !=(Node? left, Node? right) {
    if (left is null || right is null) {
      return true;
    }

    return !left.Equals(right);
  }
}