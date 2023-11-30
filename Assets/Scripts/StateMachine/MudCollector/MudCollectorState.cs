using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MudCollectorState : State
{
  protected MudCollectorStateMachine _stateMachine = null;
  protected MudCollector _entity = null;
  protected string _animationName = string.Empty;

  public MudCollectorState(MudCollectorStateMachine stateMachine, MudCollector mudCollector, string animationName)
  {
    _stateMachine = stateMachine;
    _entity = mudCollector;
    _animationName = animationName;
  }

  public override void OnEnter()
  {
    base.OnEnter();
    _entity.SetAnimation(_animationName);
  }
}
