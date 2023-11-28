using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine
{
  private State _currentState = null;

  public virtual void Initialize(State initialState)
  {
    _currentState = initialState;
    _currentState.OnEnter();
  }

  public void ChangeState(State state)
  {
    if(_currentState != null)
    {
      _currentState.OnExit();
    }

    _currentState = state;
    _currentState.OnEnter();
  }

  public virtual void LogicUpdate()
  {
    _currentState.LogicUpdate();
  }
  public virtual void PhysicsUpdate()
  {
    _currentState.PhysicsUpdate();
  }
}
