using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

  [SerializeField]
  private List<Vector3Int> _path = default;

  [SerializeField]
  private MapPattern _mapPattern;

  [SerializeField]
  private Vector3Int _endPoint;

  private EnemySpawner _enemySpawner;

  private readonly string _enemySpawnerTemplatePath = "Templates/EnemySpawner";

  public void Setup()
  {
    _path.Clear();

    InitialMap();
    DrawPath();

    Camera.main.transform.position = new Vector3(_mapPattern.mapSize.x / 2, _mapPattern.mapSize.y / 2, Camera.main.transform.position.z);

    CreateEnemySpawner();
  }

  private void InitialMap()
  {
    for (int x = 0; x < _mapPattern.mapSize.x; x++)
    {
      for (int y = 0; y < _mapPattern.mapSize.y; y++)
      {
        GridHelper.SetTile(_dirtTile, new Vector3Int(x, y));
      }
    }
  }

  #region Getter
  public List<Vector3Int> GetPath() => _path;
  public Vector3Int GetStartPoint() => _mapPattern.startPoint;
  public EnemySpawner GetEnemySpawner() => _enemySpawner;
  #endregion

  private void DrawPath()
  {
    SetPathTile(_mapPattern.startPoint);

    var drawPathData = _mapPattern.pathDir;

    Vector3Int currentPoint = _mapPattern.startPoint;

    foreach (var d in drawPathData)
    {
      for (int i = 0; i < d.n; i++)
      {
        if (currentPoint.x < 0 || currentPoint.x >= _mapPattern.mapSize.x || currentPoint.y < 0 || currentPoint.y >= _mapPattern.mapSize.y) break;

        SetPathTile(currentPoint);
        _path.Add(currentPoint);
        switch (d.direction)
        {
          case Direction.Up:
            currentPoint += new Vector3Int(0, 1);
            break;
          case Direction.Down:
            currentPoint -= new Vector3Int(0, 1);
            break;
          case Direction.Left:
            currentPoint -= new Vector3Int(1, 0);
            break;
          case Direction.Right:
            currentPoint += new Vector3Int(1, 0);
            break;
        }

        currentPoint.x = (int)Mathf.Clamp(currentPoint.x, 0, _mapPattern.mapSize.x - 1);
        currentPoint.y = (int)Mathf.Clamp(currentPoint.y, 0, _mapPattern.mapSize.y - 1);
      }
    }

    _endPoint = currentPoint;
  }

  private void SetPathTile(Vector3Int cell)
  {
    GridHelper.SetTile(_pathTile, cell);
  }

  private void CreateEnemySpawner()
  {
    var point = _mapPattern.startPoint;
    _enemySpawner = Instantiate(Resources.Load<EnemySpawner>(_enemySpawnerTemplatePath));
    _enemySpawner.Setup(_mapPattern.possibleEnemies, point, _path, EnemySpawner.SpawnType.Fix);
  }
}
