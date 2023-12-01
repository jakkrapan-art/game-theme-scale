using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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
  private List<Vector3Int> _avaliableNode = new List<Vector3Int>();
  private Dictionary<Tower, Vector3Int> _towerDict = new Dictionary<Tower, Vector3Int>();

  private readonly string _enemySpawnerTemplatePath = "Templates/EnemySpawner";

  public void Setup(Action<EnemySpawner.EnemyType> onEnemyReachTarget)
  {
    _path.Clear();

    InitialMap();
    DrawPath();

    Camera.main.transform.position = new Vector3(_mapPattern.mapSize.x / 2, _mapPattern.mapSize.y / 2, Camera.main.transform.position.z);
    CreateEnemySpawner(onEnemyReachTarget);
  }

  private void InitialMap()
  {
    _avaliableNode.Clear();
    for (int x = 0; x < _mapPattern.mapSize.x; x++)
    {
      for (int y = 0; y < _mapPattern.mapSize.y; y++)
      {
        var cell = new Vector3Int(x, y);
        GridHelper.SetTile(_dirtTile, cell);
        _avaliableNode.Add(cell);
      }
    }
  }

  #region Getter
  public List<Vector3Int> GetPath() => _path;
  public Vector3Int GetStartPoint() => _mapPattern.startPoint;
  public EnemySpawner GetEnemySpawner() => _enemySpawner;
  public Vector2 GetSize() => _mapPattern.mapSize;
  public Vector3 GetEndpoint() => _endPoint;
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
    if (!_avaliableNode.Contains(cell)) return;

    GridHelper.SetTile(_pathTile, cell);
    _avaliableNode.Remove(cell);
  }

  private void CreateEnemySpawner(Action<EnemySpawner.EnemyType> onEnemyReachTarget)
  {
    var point = _mapPattern.startPoint;
    _enemySpawner = Instantiate(Resources.Load<EnemySpawner>(_enemySpawnerTemplatePath));
    _enemySpawner.Setup(_mapPattern.possibleEnemies, point, new EnemySpawner.MoveData
    {
      movePath = _path,
      onMoveFinish = onEnemyReachTarget
    }, EnemySpawner.SpawnType.Fix);
  }

  public bool PlaceTower(Tower tower, Vector3 pos)
  {
    if (!tower) return false;

    var cell = GridHelper.WorldToCell(pos);
    cell.y = 0;
    if (!_avaliableNode.Contains(cell)) return false;

    _avaliableNode.Remove(cell);
    _towerDict.Add(tower, cell);
    return true;
  }

  public void UpdateTowerPosition(Tower tower, Vector3 mousePos)
  {
    var cell = GridHelper.WorldToCell(mousePos);
    var newPos = new Vector2(cell.x + 0.5f, cell.y + 0.5f);
    tower.transform.position = newPos;
    tower.UpdateCanplaceDisplay(CheckCanPlace(newPos));
  }

  private bool CheckCanPlace(Vector3 position)
  {
    var cell = GridHelper.WorldToCell(position);
    cell.z = 0;
    return _avaliableNode.Contains(cell);
  }

  public void PlaceTower(Tower tower)
  {
    var cell = GridHelper.WorldToCell(tower.transform.position);
    cell.z = 0;

    if (_towerDict.TryGetValue(tower, out var oldCell)) _avaliableNode.Add(oldCell);
    _avaliableNode.Remove(cell);
  }
}
