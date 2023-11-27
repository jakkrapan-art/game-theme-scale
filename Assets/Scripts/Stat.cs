using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
  [SerializeField]
  private float baseValue = 10;
  private float extraVal = 0;

  public Stat(float baseValue)
  {
    this.baseValue = baseValue;
  }

  public void UpdateExtraValue(float val)
  {
    extraVal = Mathf.Clamp(extraVal + val, 0, float.MaxValue);
  }

  public float GetValue() 
  {
    return baseValue + extraVal;
  }
}
