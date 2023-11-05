using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyQueueComparer : IComparer<Enemy>
{
  public int Compare(Enemy x, Enemy y)
  {
    int queueA = x.GetQueueOrder();
    int queueB = y.GetQueueOrder();

    return queueA.CompareTo(queueB);
  }
}
