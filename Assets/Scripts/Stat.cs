using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
  [SerializeField]
  private int value = 10;
  public Stat(int value)
  {
    this.value = value;
  }

  public int GetValue() => value;
}
