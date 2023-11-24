using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class EnemyDetector
{
  private float _radius = 0;
  private Tower _tower = default;
  private Enemy _target = default;

  public EnemyDetector(Tower tower, float radius)
  {
    _tower = tower;
    _radius = radius;
  }

  #region Getter
  public Enemy GetTargetEnemy() => _target;
  public float GetRadius() => _radius;
  #endregion

  public void Search()
  {
    if (_target)
    {
      CheckDistanceBetweenTarget();
      return;
    }

    var hit = Physics2D.OverlapCircle(_tower.transform.position, _radius);
    if (!hit || !hit.gameObject.TryGetComponent(out Enemy e))
    {      
      ClearTarget();
      return;
    }

    _target = e;
    _target.SubscribeOnDie(() =>
    {
      ClearTarget();
    });
  }

  private void CheckDistanceBetweenTarget()
  {
    if (_target == null) return;

    var distance = Vector3.Distance(_tower.transform.position, _target.transform.position);
    if (distance > _radius)
    {
      ClearTarget();
    }
  }

  private void ClearTarget()
  {
    _target = null;
  }
}
