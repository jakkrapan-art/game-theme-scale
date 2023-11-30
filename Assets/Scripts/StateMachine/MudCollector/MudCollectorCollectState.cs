using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MudCollectorCollectState : MudCollectorState
{
  private float _collectTime = 0;
  private EnemyFood _target;

  public MudCollectorCollectState(MudCollectorStateMachine stateMachine, MudCollector mudCollector, string animationName) : base(stateMachine, mudCollector, animationName)
  {
  }

  public override void OnEnter()
  {
    base.OnEnter();
    _target = _entity.GetTarget();
    if(!_target)
    {
      ChangeToIdleState();
      return;
    }

    _collectTime = _entity.GetCollectTime();
    _entity.ShowCollectBar(_collectTime, () => 
    {
      if(_target) _entity.Collect(_target);
      ChangeToIdleState();
    });
  }

  public override void OnExit()
  {
    _entity.HideCollectBar();
    base.OnExit();
  }

  public override void LogicUpdate()
  {
    base.LogicUpdate();

    if (!_target) ChangeToIdleState();
  }

  private void ChangeToIdleState()
  {
    _stateMachine.ChangeState(_stateMachine.IdleState);
  }
}
