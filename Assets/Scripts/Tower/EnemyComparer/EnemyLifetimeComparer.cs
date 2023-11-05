using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLifetimeComparer : IComparer<Enemy>
{
  public int Compare(Enemy x, Enemy y)
  {
    float lifetimeA = x.GetLifeTime();
    float lifetimeB = y.GetLifeTime();
    if (lifetimeA < lifetimeB) return -1;
    else if (lifetimeA > lifetimeB) return 1;
    else return 0;
  }
}
