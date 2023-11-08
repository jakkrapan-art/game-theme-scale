using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tube : MonoBehaviour
{
  private static string _templatePath = "Prefabs/Tube";
  private LineRenderer _lineRenderer = null;

  public static Tube Create(Vector2 start, Vector2 end)
  {
    Tube tube = Instantiate(Resources.Load<Tube>(_templatePath), start, Quaternion.identity);
    tube.Awake();
    tube.DrawLine(start, end);
    return tube;
  }

  #region UNITY_FUNCTIONS
  private void Awake()
  {
    _lineRenderer = GetComponent<LineRenderer>();
  }
  #endregion

  public void DrawLine(Vector2 start, Vector2 end)
  {
    if (!_lineRenderer) return;

    _lineRenderer.positionCount = 2;
    _lineRenderer.SetPosition(0, start);
    _lineRenderer.SetPosition(1, end);
  }

  public Vector3 GetOppositePoint(Vector3 from)
  {
    if (!_lineRenderer) return default;

    if (from == _lineRenderer.GetPosition(0)) return _lineRenderer.GetPosition(1);
    else return _lineRenderer.GetPosition(0);
  }
}
