using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MudCollectorIdleState : MudCollectorState
{
  public MudCollectorIdleState(MudCollectorStateMachine stateMachine, MudCollector mudCollector, string animationName) : base(stateMachine, mudCollector, animationName)
  {
  }

  public override void LogicUpdate()
  {
    base.LogicUpdate();

    if (!_entity.GetAllowCollect()) return;
    SearchTarget();
  }

  private void SearchTarget()
  {
    var muds = GameObject.FindObjectsOfType<EnemyFood>();
    if (muds == null || muds.Length == 0) return;

    if (_entity.IsMudFull())
    {
      _stateMachine.ChangeState(_stateMachine.MoveState);
      return;
    }

    var newTarget = getNearestTarget(muds);
    _entity.SetTarget(newTarget);
    _stateMachine.ChangeState(_stateMachine.MoveState);
  }

  private EnemyFood getNearestTarget(EnemyFood[] list)
  {
    if (list == null || list.Length == 0) return null;

    EnemyFood nearest = null;

    foreach (EnemyFood enemy in list)
    {
      if (nearest == null)
      {
        nearest = enemy;
      }
      else
      {
        float distanceA = Vector2.Distance(_entity.transform.position, nearest.transform.position);
        float distanceB = Vector2.Distance(_entity.transform.position, enemy.transform.position);

        if (distanceB < distanceA) nearest = enemy;
      }
    }

    return nearest;
  }
}
