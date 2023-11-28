using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerStateMachine : StateMachine
{
  private Tower _tower;

  public TowerIdleState IdleState { get; private set; }
  public TowerReloadState ReloadState { get; private set; }
  public TowerAttackState AttackState { get; private set; }
  

  public TowerStateMachine(Tower tower)
  {
    _tower = tower;

    IdleState = new TowerIdleState(tower, this);
    ReloadState = new TowerReloadState(tower, this);
    AttackState = new TowerAttackState(tower, this);

    Initialize(IdleState);
  }
}
