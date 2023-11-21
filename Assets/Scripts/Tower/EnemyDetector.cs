using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetector
{
  private LineRenderer _lineRenderer = default;

  private float _findRadius = 0;
  private Tower _tower = default;
  private Enemy _target = default;

  public EnemyDetector(Tower tower, float radius)
  {
    _tower = tower;
    _findRadius = radius;

    if(tower)
    {
      GameObject detectorGo = tower.transform.Find("EnemyDetector")?.gameObject;
      if (detectorGo) detectorGo.TryGetComponent(out _lineRenderer);
    }
  }

  private void DrawLineToTarget()
  {
    if (!_lineRenderer || !_lineRenderer.enabled || !_target) return;
    //clear line
    _lineRenderer.positionCount = 0;
    
    _lineRenderer.positionCount = 2;
    _lineRenderer.SetPosition(0, _tower.transform.position);
    _lineRenderer.SetPosition(1, _target.transform.position);
  }

  public void Search()
  {
    if (_target) return;

    var hit = Physics2D.OverlapCircle(_tower.transform.position, _findRadius);
    if (!hit || !hit.gameObject.TryGetComponent(out Enemy e))
    {
      _target = null;
      return;
    }

    _target = e;
    _target.SubscribeOnDie(() =>
    {
      _target = null;
    });
  }

  public void SetEnableLine(bool enable)
  {
    _lineRenderer.enabled = enable;
  }
}
