using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MudCollectorStateMachine : StateMachine
{
  protected MudCollector _entity;

  public MudCollectorIdleState IdleState { get; }
  public MudCollectorMoveState MoveState { get; }
  public MudCollectorCollectState CollectState { get; }

  public MudCollectorStateMachine(MudCollector entity)
  {
    _entity = entity;

    IdleState = new MudCollectorIdleState(this, entity);
    MoveState = new MudCollectorMoveState(this, entity);
    CollectState = new MudCollectorCollectState(this, entity);

    Initialize(IdleState);
  }
}
