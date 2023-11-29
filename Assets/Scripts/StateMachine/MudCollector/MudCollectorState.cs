using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MudCollectorState : State
{
  protected MudCollectorStateMachine _stateMachine = null;
  protected MudCollector _entity = null;

  public MudCollectorState(MudCollectorStateMachine stateMachine, MudCollector mudCollector)
  {
    _stateMachine = stateMachine;
    _entity = mudCollector;
  }
}
