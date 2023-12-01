using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalableEnemy : Enemy
{
  [SerializeField]
  private LayerMask _foodLayerMask;

  private int _maxFood = 20;
  private int _currentFood = 0;
  private bool _eatingFood = false;

  private ScalableEnemyData _data;

  #region Unity Functions
  protected override void Start()
  {
    base.Start();
  }
  protected override void Update()
  {
    base.Update();
    SearchFood();
  }
  #endregion


  protected override void Setup(EnemyData data)
  {
    _data = data as ScalableEnemyData;
    _maxFood = _data.maxFood;

    base.Setup(data);
  }

  private void SearchFood()
  {
    if (_eatingFood) return;

    ContactFilter2D filter = new ContactFilter2D();
    filter.layerMask = _foodLayerMask;

    var hit = Physics2D.OverlapCircleAll(transform.position, 0.25f);
    foreach (var h in hit)
    {
      if (h.gameObject.TryGetComponent(out EnemyFood food))
      {
        UpdateFood(food.Eat());
        break;
      }
    }
  }

  private void UpdateFood(int amount)
  {
    _eatingFood = true;
    _currentFood += amount;

    if (_currentFood >= _maxFood) 
    {
      int increaseCount = Mathf.FloorToInt(_currentFood / _maxFood);
      for (int i = 0; i < increaseCount; i++)
      {
        //increase scale and stats
        float scale = transform.localScale.x + _data.extraScalePerLevel;
        SetScale(scale, false, () => 
        {
          _eatingFood = false;
        });
        UpdateHealthStat(_data.extraHealthPerLevel);
        UpdateMoveSpeedStat(_data.extraMoveSpeedPerLevel);
      }
    }

    _currentFood %= _maxFood;
  }

  private void OnDrawGizmos()
  {
    Gizmos.color = Color.yellow;
    Gizmos.DrawSphere(transform.position, 1);
  }
}
