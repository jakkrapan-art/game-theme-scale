using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthComparer : IComparer<Enemy>
{
  public int Compare(Enemy x, Enemy y)
  {
    int healthA = x.GetCurrentHealth();
    int healthB = y.GetCurrentHealth();

    if (healthA < healthB) return -1;
    else if (healthA > healthB) return 1;
    else return 0;
  }
}
