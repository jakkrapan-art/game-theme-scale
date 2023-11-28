using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAttackState : TowerState
{
  public TowerAttackState(Tower tower, TowerStateMachine stateMachine) : base(tower, stateMachine)
  {
  }

  public override void OnEnter()
  {
    var target = _tower.GetEnemyDetector().GetTargetEnemy();

    if (target)
    {
      _tower.Attack(target);
      _stateMachine.ChangeState(_stateMachine.ReloadState);
    }
    else
    {
      _stateMachine.ChangeState(_stateMachine.IdleState);
    }
  }

  public override void OnExit() {}
  public override void LogicUpdate() {}
  public override void PhysicsUpdate() {}
}
