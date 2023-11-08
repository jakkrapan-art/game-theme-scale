using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
  private EnemyContainer _enemyContainer = null;
  [SerializeField]
  private LineRenderer _lineRenderer = default;

  public void Setup(EnemyContainer enemyContainer)
  {
    _enemyContainer = enemyContainer;
  }

  public void Update()
  {
    DrawLineToTarget();
  }

  private void DrawLineToTarget()
  {
    if (!_lineRenderer) return;
    //clear line
    _lineRenderer.positionCount = 0;
    if (_enemyContainer == null || (_enemyContainer != null && !_enemyContainer.GetTargetEnemy())) return;

    Vector2 target = _enemyContainer.GetTargetEnemy().transform.position;

    _lineRenderer.positionCount = 2;
    _lineRenderer.SetPosition(0, transform.position);
    _lineRenderer.SetPosition(1, target);
  }
}
