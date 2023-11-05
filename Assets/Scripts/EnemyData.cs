using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = Const.ScriptablePathPrefix + "Enemy")]
public class EnemyData : ScriptableObject
{
  public float lifeTime = 30;
  public int maxHealth = 15;
}
