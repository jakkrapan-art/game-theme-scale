using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MudCollectorIdleState : MudCollectorState
{
  public MudCollectorIdleState(MudCollectorStateMachine stateMachine, MudCollector mudCollector) : base(stateMachine, mudCollector)
  {
  }

  private class DistanceComparer : IComparer<EnemyFood>
  {
    private readonly Transform referencePoint;

    public DistanceComparer(Transform reference)
    {
      referencePoint = reference;
    }

    public int Compare(EnemyFood x, EnemyFood b)
    {
      float distanceToA = Vector3.Distance(referencePoint.position, x.transform.position);
      float distanceToB = Vector3.Distance(referencePoint.position, b.transform.position);

      return distanceToA.CompareTo(distanceToB);
    }
  }

  public override void LogicUpdate()
  {
    base.LogicUpdate();
    SearchTarget();
  }

  private void SearchTarget()
  {
    var muds = GameObject.FindObjectsOfType<EnemyFood>();
    if (muds == null || muds.Length == 0) return;

    var distanceComparer = new DistanceComparer(_entity.transform);
    muds.ToList().Sort(distanceComparer);

    var newTarget = muds[0];
    _entity.SetTarget(newTarget);

    _stateMachine.ChangeState(_stateMachine.MoveState);
  }
}
