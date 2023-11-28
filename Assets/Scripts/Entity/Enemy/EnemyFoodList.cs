using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyFoodList", menuName = "ScriptableObject/EnemyFoodList")]
public class EnemyFoodList : ScriptableObject
{
  public List<EnemyFood> splashes = new List<EnemyFood>();
}