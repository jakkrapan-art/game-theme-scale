using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerIdleState : TowerState
{
  public TowerIdleState(Tower tower, TowerStateMachine stateMachine) : base(tower, stateMachine)
  {
  }

  public override void LogicUpdate()
  {
    var detector = _tower.GetEnemyDetector();
    if(detector != null) 
    {
      detector.Search();
      if(detector.GetTargetEnemy())
      {
        _stateMachine.ChangeState(_stateMachine.AttackState);
      }
    }
  }

  public override void OnEnter() {}

  public override void OnExit() {}

  public override void PhysicsUpdate() {}
}
