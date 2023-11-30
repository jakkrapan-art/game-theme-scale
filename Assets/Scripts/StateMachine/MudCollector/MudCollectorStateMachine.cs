using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MudCollectorStateMachine : StateMachine
{
  protected MudCollector _entity;

  public MudCollectorIdleState IdleState { get; }
  public MudCollectorMoveState MoveState { get; }
  public MudCollectorCollectState CollectState { get; }
  public MudCollectorInvisibleState InvisibleState { get; }

  public MudCollectorStateMachine(MudCollector entity)
  {
    _entity = entity;

    IdleState = new MudCollectorIdleState(this, entity, "Idle");
    MoveState = new MudCollectorMoveState(this, entity, "Move");
    CollectState = new MudCollectorCollectState(this, entity, "Collect");
    InvisibleState = new MudCollectorInvisibleState(this, entity, "");

    Initialize(IdleState);
  }
}
