using System.Collections;
using System.Numerics;
using System.Text;

namespace cs480_hw1; 
  
// This class just acts as a wrapper around the SortedList
// The SortedList was chosen to allow for finding keys and values in the array easily
// Used help from microsoft docs to implement the IEnumerable and IEnumerator
// to allow for iteration over the collection

using KVPair = KeyValuePair<Tuple<int, int>, int>;

public class State : IEnumerable, ICloneable {
  public SortedList<Tuple<int, int>, int> Value { get; } = new();
  public int X => x_;
  public int Y => y_;

  private readonly int x_ = 0;
  private readonly int y_ = 0;

  public State(int[,] state) {
    y_ = state.GetLength(0);
    x_ = state.GetLength(1);
    for (int i = 0; i < y_; ++i) {
      for (int j = 0; j < x_; ++j) {
        Value?.Add(new Tuple<int, int>(i,j), state[i,j]);
      }
    }
  }

  public State(State state) {
    y_ = state.y_;
    x_ = state.x_;
    foreach (var keyValuePair in state.Value) {
      Value.Add(keyValuePair.Key, keyValuePair.Value);
    }
  }

  // Get value from key
  public int? this[int i, int j] {
    get {
      try {
        return Value[new Tuple<int, int>(i, j)];
      } catch (KeyNotFoundException) {
        return null;
      }
    }
    set => Value[new Tuple<int, int>(i,j)] = value.Value;
  }

  public KVPair? GetKeyValuePairFromValue(int i) {
    var index = Value.IndexOfValue(0);
    try {
      return Value.ElementAt(index);
    } catch (ArgumentOutOfRangeException) {
      return null;
    }
  }

  public bool DimsMatch(State s) => x_ == s.x_ && y_ == s.y_;

  public override bool Equals(object? obj) => Equals(obj as State ?? throw new InvalidOperationException());

  public bool Equals(State s) => Value.SequenceEqual(s.Value);

  public override int GetHashCode() => Value.GetHashCode();
  public override string ToString() {
    StringBuilder stringBuilder = new();
    for (var i = 0; i < y_; ++i) {
      stringBuilder.Append('+');
      for (var j = 0; j < x_; ++j) {
        stringBuilder.Append("---+");
      }
      stringBuilder.Append("\n|");
      for (var j = 0; j < x_; ++j) {
        stringBuilder.Append(String.Format("{0,3}", this[i, j] == 0 ? " " : this[i, j]));
        if (j != x_ - 1) stringBuilder.Append('|');
      }
      stringBuilder.Append("|\n");
    }
    stringBuilder.Append('+');
    for (var i = 0; i < x_; ++i) {
      stringBuilder.Append("---+");
    }
    return stringBuilder.ToString();
  }

  public object Clone() {
    return new State(this);
  }

  IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
  public StateEnum GetEnumerator() => new(Value);

  public static bool operator ==(State left, State right) => left.Equals(right);
  public static bool operator !=(State left, State right) => !left.Equals(right);
}

public class StateEnum : IEnumerator {
  private readonly SortedList<Tuple<int, int>, int> state_;
  private int pos_ = -1;

  public StateEnum(SortedList<Tuple<int, int>, int> state) {
    state_ = state;
  }

  public bool MoveNext() {
    ++pos_;
    return pos_ < state_.Count;
  }

  public void Reset() => pos_ = -1;

  object IEnumerator.Current => Current;

  public KVPair Current {
    get {
      try {
        // convert 1D index to 2D index
        return state_.ElementAt(pos_);
      } catch (InvalidOperationException) {
        throw new InvalidOperationException();
      }
    }
  }
}