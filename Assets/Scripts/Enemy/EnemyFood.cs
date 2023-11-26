using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFood : MonoBehaviour
{
  private static readonly string SPLASH_LIST_PATH = "Datas/EnemyFoodList";
  private int _foodValue = 5;

  public struct SetupData
  {
    public int foodValue;
  }

  public static void Create(Vector3 pos, SetupData data)
  {
    EnemyFoodList list = Resources.Load(SPLASH_LIST_PATH) as EnemyFoodList;
    if (!list || list.splashes.Count == 0) return;
    var go = Instantiate(list.splashes[Random.Range(0, list.splashes.Count)], pos, Quaternion.identity);
    if (go.TryGetComponent(out EnemyFood food)) food.Setup(data);
  }

  public void Setup(SetupData data)
  {
    _foodValue = data.foodValue;
  }

  public int Eat()
  {
    Destroy(gameObject);
    return _foodValue;
  }
}