using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MudCollectorMoveState : MudCollectorState
{
  private EnemyFood _target;

  public MudCollectorMoveState(MudCollectorStateMachine stateMachine, MudCollector mudCollector) : base(stateMachine, mudCollector)
  {
  }

  public override void OnEnter()
  {
    base.OnEnter();

    _target = _entity.GetTarget();
  }

  public override void LogicUpdate()
  {
    base.LogicUpdate();

    if (!_target)
    {
      _stateMachine.ChangeState(_stateMachine.IdleState);
    }
    else if(Vector3.Distance(_target.transform.position, _entity.transform.position) <= _entity.GetCollectRange())
    {
      _stateMachine.ChangeState(_stateMachine.CollectState);
    }
  }

  public override void PhysicsUpdate()
  {
    base.PhysicsUpdate();

    if (!_target) return;
    _entity.MoveTo(_target.transform.position);
  }
}
