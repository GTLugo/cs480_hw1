namespace cs480_hw1; 

public class Puzzle {
  private readonly State goalStateOld_;
  public State InitStateOld { get; }
  public State CurrentStateOld { get; }

  public bool Solved  => CurrentStateOld == goalStateOld_;

  public Puzzle(State goalStateOld, State initStateOld) {
    if (!goalStateOld.DimsMatch(initStateOld)) {
      // Ensures that both goalState and initState match in dimensions
      throw new Exception("Dimensions of state arrays must match");
    }
    goalStateOld_ = goalStateOld;
    CurrentStateOld = InitStateOld = initStateOld;
  }
}