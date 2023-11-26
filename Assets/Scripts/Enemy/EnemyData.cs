using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = Const.ScriptablePathPrefix + "Enemy")]
public class EnemyData : ScriptableObject
{
  public int maxHealth = 15;
  public float moveSpeed = 5f;
  public float scale = 1f;
}
