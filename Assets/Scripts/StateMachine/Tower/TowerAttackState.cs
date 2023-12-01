public class TowerAttackState : TowerState
{
  private Enemy _target = null;
  public TowerAttackState(Tower tower, TowerStateMachine stateMachine) : base(tower, stateMachine)
  {
  }

  public override void OnEnter()
  {
    _target = _tower.GetEnemyDetector().GetTargetEnemy();
  }

  public override void OnExit() 
  {
    _target = null;
  }

  public override void LogicUpdate() 
  {
    if (_target)
    {
      _tower.Attack(_target);
      _stateMachine.ChangeState(_stateMachine.ReloadState);
    }
    else
    {
      _stateMachine.ChangeState(_stateMachine.IdleState);
    }
  }
  public override void PhysicsUpdate() {}
}
