using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MudCollectorInvisibleState : MudCollectorState
{
  public MudCollectorInvisibleState(MudCollectorStateMachine stateMachine, MudCollector mudCollector, string animationName) : base(stateMachine, mudCollector, animationName)
  {
  }

  public override void OnEnter()
  {
    base.OnEnter();
    InvisibleTracker.Create(_entity.gameObject, 5, () => 
    {
      _entity.SellToTown();
      _stateMachine.ChangeState(_stateMachine.IdleState);
    });
  }
}
