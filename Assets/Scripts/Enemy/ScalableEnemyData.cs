using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Data/ScalableEnemy", fileName = "ScalableEnemy")]
public class ScalableEnemyData : EnemyData
{
  public int maxFood = 25;
  public float extraScale = 0.3f;
}
