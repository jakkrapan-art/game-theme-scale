using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
  [SerializeField]
  private int baseValue = 10;
  private Dictionary<Tower, List<int>> buffDict;

  public Stat(int baseValue)
  {
    this.baseValue = baseValue;
    buffDict = new Dictionary<Tower, List<int>>();
  }

  public void AddModifier(Tower buffer, int newBuff)
  {
    if(buffDict.TryGetValue(buffer, out List<int> buffList))
    {
      buffList.Add(newBuff);
    }
    else
    {
      buffDict.Add(buffer, new List<int>(newBuff));
    }
  }

  public void RemoveModifier(Tower buffer) 
  {
    if (!buffDict.ContainsKey(buffer)) return;

    buffDict.Remove(buffer);
  }

  public int GetValue() 
  {
    int totalVal = baseValue;
    foreach (var modifyList in buffDict.Values)
    {
      foreach (var value in modifyList)
      {
        totalVal += value;
      }
    }

    return totalVal;
  }
}
