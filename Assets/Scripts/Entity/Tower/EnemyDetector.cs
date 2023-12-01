using UnityEngine;

public class EnemyDetector
{
  private float _radius = 0;
  private Enemy _target = default;

  public EnemyDetector(float radius)
  {
    _radius = radius;
  }

  #region Getter
  public Enemy GetTargetEnemy() => _target;
  public float GetRadius() => _radius;
  #endregion

  public Enemy Search(Vector3 towerPos)
  {
    if (_target)
    {
      CheckDistanceBetweenTarget(towerPos);
    }
    else
    {
      var hits = Physics2D.CircleCastAll(towerPos, _radius, Vector2.zero);
      if (hits == null || hits.Length == 0)
      {
        ClearTarget();
        return null;
      }

      Enemy nearestTarget = null;
      float nearestDistance = float.MaxValue;

      foreach (var hit in hits)
      {
        if(hit.collider.gameObject.TryGetComponent<Enemy>(out var e))
        {
          float distance = Vector3.Distance(towerPos, hit.point);
          if (distance < nearestDistance) 
          {
            nearestTarget = e;
            nearestDistance = distance;
          }
        }
      }
      if (!nearestTarget) return null;
      _target = nearestTarget;
      _target.SubscribeOnDie(() =>
      {
        ClearTarget();
      });
    }

    return _target;
  }

  private void CheckDistanceBetweenTarget(Vector3 towerPos)
  {
    if (_target == null) return;

    var distance = Vector3.Distance(towerPos, _target.transform.position);
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
