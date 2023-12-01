public class TowerReloadState : TowerState
{
  public TowerReloadState(Tower tower, TowerStateMachine stateMachine) : base(tower, stateMachine)
  {
  }

  public override void OnEnter() 
  {
    Reload();
  }

  public override void PhysicsUpdate() {}

  private void Reload()
  {
    _tower.Reload(() =>
    {
      _stateMachine.ChangeState(_stateMachine.IdleState);
    });
  }
}
