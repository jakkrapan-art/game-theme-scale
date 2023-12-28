using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathLine : MonoBehaviour
{
  [SerializeField]
  private LineRenderer _line = default;

  public void SetPoints(List<Vector3Int> points)
  {
    if (!_line) return;
    _line.positionCount = 0; //clear

    var convertedPoints = new List<Vector3>();

    foreach (var p in points)
    {
      convertedPoints.Add(GridHelper.CellToWorld(p) + new Vector3(0.5f, 0.5f));
    }
    
    _line.positionCount = convertedPoints.Count;
    _line.SetPositions(convertedPoints.ToArray());
  }

  public void Show()
  {
    if (_line) _line.enabled = true;
  }

  public void Hide()
  {
    if (_line) _line.enabled = false;
  }
}
