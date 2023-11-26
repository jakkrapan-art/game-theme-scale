using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalableEnemy : Enemy
{
  [SerializeField]
  private LayerMask _foodLayerMask;

  private int _maxFood = 20;
  private int _currenFood = 0;

  protected override void Start()
  {
    base.Start();

    SubscribeOnUpdateAction(SearchFood);
  }

  private void SearchFood()
  {
    ContactFilter2D filter = new ContactFilter2D();
    filter.layerMask = _foodLayerMask;

    Collider2D[] hits = new Collider2D[1];
    int overlappedCount = Physics2D.OverlapCollider(GetComponent<Collider2D>(), filter, hits);
    if (overlappedCount <= 0) return;

    foreach (var hitObj in hits)
    {
      if (hitObj.TryGetComponent(out EnemyFood food)) UpdateFood(food.Eat()); 
    }
  }

  private void UpdateFood(int amount)
  {
    _currenFood += amount;

    if(_currenFood > _maxFood) 
    {
      int increaseCount = Mathf.FloorToInt(_currenFood / _maxFood);
      for (int i = 0; i < increaseCount; i++)
      {
        //increase scale and stats
      }
    }

    _currenFood %= _maxFood;
  }
}
