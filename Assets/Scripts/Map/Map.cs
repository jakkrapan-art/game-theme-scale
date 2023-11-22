using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.WSA;

public class Map : MonoBehaviour
{
  [SerializeField]
  private TileBase _dirtTile;
  [SerializeField]
  private TileBase _dirtBorderTile;
  [SerializeField]
  private TileBase _pathTile;

  private Vector2 _mapSize = new Vector2(18, 10);
  private Vector3Int _startPoint = default;
  private Vector3Int _endPoint = default;
  private Vector3Int _path = default;

  public void Setup(Vector2Int mapSize, Vector3Int startPoint, Vector3Int endPoint, int pathTileCount)
  {
    _startPoint = startPoint;
    _endPoint = endPoint;
    _mapSize = mapSize;

    DrawPath(pathTileCount);
  }

  private void DrawPath(int tileCount)
  {
    for (int x = -Mathf.FloorToInt(_mapSize.x/2); x < _mapSize.x/2; x++)
    {
      for (int y = -Mathf.FloorToInt(_mapSize.y / 2); y < _mapSize.y/2; y++)
      {
        var cell = new Vector3Int(x, y);
        if ((x == _startPoint.x && y == _startPoint.y) || (x == _endPoint.x && y == _endPoint.y)) continue;
        
        GridHelper.SetTile(_dirtTile, cell);
      }
    }

    SetPathTile(_startPoint);
    SetPathTile(_endPoint);
  }

  private void SetPathTile(Vector3Int cell)
  {
    GridHelper.SetTile(_pathTile, cell);
    Debug.Log("cell y: " + cell.y + " map y: " + _mapSize.y / 2);
    if(cell.y + 1 < _mapSize.y / 2) GridHelper.SetTile(_dirtBorderTile, cell + new Vector3Int(0, 1));
  }
}
