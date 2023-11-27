using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Data/ScalableEnemy", fileName = "ScalableEnemy")]
public class ScalableEnemyData : EnemyData
{
  public int maxFood = 25;
  public float extraScalePerLevel = 0.3f;
  public float extraHealthPerLevel = 5;
  public float extraMoveSpeedPerLevel = 1;
}
