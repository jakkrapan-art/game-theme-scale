using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITransition
{
  public void TransitionIn();
  public void TransitionOut();
}

public enum TransitionType { None, In, Out }
public enum TransitionDirection { Left, Right, Up, Down }
