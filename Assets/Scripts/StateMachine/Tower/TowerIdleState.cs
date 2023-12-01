public class TowerIdleState : TowerState
{
  public TowerIdleState(Tower tower, TowerStateMachine stateMachine) : base(tower, stateMachine)
  {
  }

  public override void LogicUpdate()
  {
    var target = _tower.SearchEnemy();
    if (target) { _stateMachine.ChangeState(_stateMachine.AttackState); }
  }

  public override void OnEnter() { }

  public override void OnExit() { }

  public override void PhysicsUpdate() { }
}
