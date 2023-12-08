public class TowerStateMachine : StateMachine
{
  private Tower _tower;
  public TowerIdleState IdleState { get; private set; }
  public TowerReloadState ReloadState { get; private set; }
  public TowerAttackState AttackState { get; private set; }
  public TowerDisableState DisableState { get; private set; }
  

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
  }

  public void SetEnable(bool enable)
  {
    if (enable)
      ChangeState(IdleState);
    else 
      ChangeState(DisableState);
  }
}
