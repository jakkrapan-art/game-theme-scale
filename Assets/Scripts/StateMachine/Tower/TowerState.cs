public abstract class TowerState : State
{
  protected TowerStateMachine _stateMachine;
  protected Tower _tower;
  public TowerState(Tower tower, TowerStateMachine stateMachine)
  {
    _stateMachine = stateMachine;
    _tower = tower;
  }
}
