using UnityEngine;

public class TowerStateMachine : StateMachine
{
  private Tower _tower;
  public string currentState;
  [field: SerializeField] public TowerIdleState IdleState { get; private set; }
  [field: SerializeField] public TowerReloadState ReloadState { get; private set; }
  [field: SerializeField] public TowerAttackState AttackState { get; private set; }
  

  public TowerStateMachine(Tower tower)
  {
    _tower = tower;

    IdleState = new TowerIdleState(tower, this);
    ReloadState = new TowerReloadState(tower, this);
    AttackState = new TowerAttackState(tower, this);

    Initialize(IdleState);
  }

  public override void LogicUpdate()
  {
    base.LogicUpdate();
    currentState = _currentState.ToString();
  }
}
