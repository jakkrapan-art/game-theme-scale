using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
  public abstract void OnEnter();
  public abstract void OnExit();
  public abstract void LogicUpdate();
  public abstract void PhysicsUpdate();
}
