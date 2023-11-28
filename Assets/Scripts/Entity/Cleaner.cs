using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cleaner : MonoBehaviour
{
  private EnemyFood _taget;

  public void SetTarget(EnemyFood target)
  { 
    _taget = target;
  }
}
