using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MudCollectorMoveState : MudCollectorState
{
  private EnemyFood _target;
  private bool _moveToTown = false;

  public MudCollectorMoveState(MudCollectorStateMachine stateMachine, MudCollector mudCollector, string animationName) : base(stateMachine, mudCollector, animationName)
  {

  }

  public override void OnEnter()
  {
    base.OnEnter();

    _target = _entity.GetTarget();
    _moveToTown = _entity.IsMoveToTown();
  }

  public override void LogicUpdate()
  {
    base.LogicUpdate();

    if(!_entity.GetAllowCollect()) { _stateMachine.ChangeState(_stateMachine.IdleState); }

    if (!_moveToTown)
    {
      if (!_target)
      {
        _stateMachine.ChangeState(_stateMachine.IdleState);
      }
      else if (Vector3.Distance(_target.transform.position, _entity.transform.position) <= _entity.GetCollectRange())
      {
        _stateMachine.ChangeState(_stateMachine.CollectState);
      }
    }
    else
    {
      if (Mathf.Approximately(Vector3.Distance(_entity.transform.position, _entity.GetTownPosition()), 0))
      {
        _stateMachine.ChangeState(_stateMachine.InvisibleState);
      }
    }
  }

  public override void PhysicsUpdate()
  {
    base.PhysicsUpdate();

    if (_moveToTown)
    {
      _entity.MoveToTown();
    }
    else
    {
      if (!_target) return;
      _entity.MoveTo(_target.transform.position);
    }
  }
}
