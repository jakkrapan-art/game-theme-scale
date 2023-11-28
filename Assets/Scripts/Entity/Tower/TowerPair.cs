using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TowerPair
{
  private static Dictionary<(Tower t1, Tower t2), Tube> _towerPair = new Dictionary<(Tower t1, Tower t2), Tube>();

  public static void PairTower(Tower t1, Tower t2)
  {
    if(!t1 || !t2) return;

    if(!_towerPair.ContainsKey((t1, t2)) && !_towerPair.ContainsKey((t2, t1)))
    {
      Tube tube = Tube.Create(t1.transform.position, t2.transform.position);
      _towerPair.Add((t1, t2), tube);
      t2.Connect(t1);
    }
  }

  public static void UnPairTower(Tower t1, Tower t2)
  {
    if (!t1 || !t2) return;

    (Tower, Tower) pair = (null, null);
    if(_towerPair.ContainsKey((t1,t2)))
    {
      pair = (t1, t2);
    }
    else if(_towerPair.ContainsKey((t2, t1)))
    {
      pair = (t2, t1);
    }

    if(pair != (null, null)) _towerPair.Remove(pair);
  }

  public static void Cleanup()
  {
    _towerPair.Clear();
  }
}
