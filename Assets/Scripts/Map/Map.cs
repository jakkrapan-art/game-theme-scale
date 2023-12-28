using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class Map : MonoBehaviour
{
  private enum UpdateAvailableNodeType { Add, Remove, None };

  [Header("Tiles")]
  [SerializeField]
  private TileBase _dirtTile;
  [SerializeField]
  private TileBase _dirtBorderTile;
  [SerializeField]
  private TileBase _pathTile;

  private List<Vector3Int> _path = new List<Vector3Int>();
  [SerializeField]
  private MapPattern _mapPattern;
  [SerializeField]
  private Vector3Int _endPoint;

  [SerializeField]
  private PathLine _pathLine;
  private EnemySpawner _enemySpawner;
  private List<Vector3Int> _avaliableNode = new List<Vector3Int>();
  private Dictionary<Tower, Vector3Int> _towerDict = new Dictionary<Tower, Vector3Int>();

  private readonly string _enemySpawnerTemplatePath = "Templates/EnemySpawner";

  public void Setup(Action<EnemySpawner.EnemyType> onEnemyReachTarget)
  {
    Camera.main.transform.position = new Vector3(_mapPattern.mapSize.x / 2, _mapPattern.mapSize.y / 2, Camera.main.transform.position.z);

    var generator = new MapGenerator(this, new Vector2Int((int)_mapPattern.mapSize.x, (int)_mapPattern.mapSize.y), _dirtTile, _pathTile);
    generator.Generate();
    /*_path.Clear();
    StartCoroutine(GenerateMap(new Vector2Int((int)_mapPattern.mapSize.x, (int)_mapPattern.mapSize.y)));*/

    /*InitialMap();
    DrawPath(new PathUpdateData
    {
      add = new List<Vector3Int>()
    });

    CreateEnemySpawner(onEnemyReachTarget);*/
  }

  private IEnumerator GenerateMap(Vector2Int size)
  {
    Camera.main.transform.position = new Vector3(size.x / 2, size.y / 2, Camera.main.transform.position.z);
    GenerateFloor(size);
    yield return GeneratePath(_mapPattern.townPoint, size);
  }

  private void GenerateFloor(Vector2Int size)
  {
    for (int x = 0; x < size.x; x++)
    {
      for (int y = 0; y < size.y; y++)
      {
        var cell = new Vector3Int(x, y);
        SetTile(cell, _dirtTile, UpdateAvailableNodeType.Add);
      }
    }
  }

  private IEnumerator GeneratePath(Vector3Int start, Vector2Int size)
  {
    Vector3Int current = new Vector3Int(Random.Range(0, size.x), size.y - 1);
    List<Vector3Int> path = new List<Vector3Int>();

    while(current.y >= 0)
    {
      path.Add(current);
      SetTile(current, _pathTile, UpdateAvailableNodeType.Remove);
      if (current.y == 0) break;

      var adjacents = GetAdjacents(current, _avaliableNode);
      for (int i = 0; i < 2; i++)
      {
        int index = Random.Range(0, adjacents.Count);
        var newNode = adjacents[index];
        adjacents.RemoveAt(index);

        if(newNode.y <= current.y)
        {
          current = newNode;
          break;
        }
      }

      yield return new WaitForFixedUpdate();
    }
  }

  private void InitialMap()
  {
    _avaliableNode.Clear();
    for (int x = 0; x < _mapPattern.mapSize.x; x++)
    {
      for (int y = 0; y < _mapPattern.mapSize.y; y++)
      {
        var cell = new Vector3Int(x, y);
        SetTile(cell, _dirtTile, UpdateAvailableNodeType.Add);
        _avaliableNode.Add(cell);
      }
    }
  }

  #region Getter
  public List<Vector3Int> GetPath() => _path;
  public Vector3Int GetStartPoint() => _mapPattern.enemySpawnPoint;
  public EnemySpawner GetEnemySpawner() => _enemySpawner;
  public Vector2 GetSize() => _mapPattern.mapSize;
  public Vector3 GetEndpoint() => _endPoint;
  #endregion

  private void CreatePath()
  {

  }

  private void AddPath(Vector3Int cell)
  {
    if (_path.Contains(cell)) return;

    _path.Add(cell);
    SetPathTile(cell);
  }

  private void DrawPath(PathUpdateData updateData)
  {
    if (updateData.remove != null)
    {
      foreach (var remove in updateData.remove)
      {
        SetTile(remove, _dirtTile, UpdateAvailableNodeType.Add);
      }
    }

    if (updateData.add != null)
    {
      foreach (var add in updateData.add)
      {
        SetTile(add, _pathTile, UpdateAvailableNodeType.Remove);
      }
    }
    //SetPathTile(_mapPattern.enemySpawnPoint);

    /*var drawPathData = _mapPattern.pathDir;

    Vector3Int currentPoint = _mapPattern.enemySpawnPoint;

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
    }*/

    _endPoint = _mapPattern.townPoint;
    if (_pathLine) _pathLine.SetPoints(_path);
  }

  private void SetTile(Vector3Int cell, TileBase tile, UpdateAvailableNodeType type)
  {
    GridHelper.SetTile(tile, cell);
    UpdateAvailable(cell, type);
  }

  private void UpdateAvailable(Vector3Int cell, UpdateAvailableNodeType type)
  {
    switch (type)
    {
      case UpdateAvailableNodeType.Add:
        if (!_avaliableNode.Contains(cell)) _avaliableNode.Add(cell);
        break;
      case UpdateAvailableNodeType.Remove:
        if (_avaliableNode.Contains(cell)) _avaliableNode.Remove(cell);
        break;
      default: break;
    }
  }

  private void SetPathTile(Vector3Int cell)
  {
    if (!_avaliableNode.Contains(cell)) return;

    GridHelper.SetTile(_pathTile, cell);
    _avaliableNode.Remove(cell);
  }

  private void CreateEnemySpawner(Action<EnemySpawner.EnemyType> onEnemyReachTarget)
  {
    var point = _mapPattern.enemySpawnPoint;
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
    tower.SetEnable(true);
    if (_towerDict.TryGetValue(tower, out var oldCell)) _avaliableNode.Add(oldCell);
    _avaliableNode.Remove(cell);
  }

  private List<Vector3Int> FindShortestPath(Vector3Int start, Vector3Int end)
  {
    List<Vector3Int> path = new List<Vector3Int>();
    Queue<Vector3Int> queue = new Queue<Vector3Int>();
    List<Vector3Int> visited = new List<Vector3Int>();
    queue.Enqueue(start);

    while (queue.Count > 0)
    {

    }

    return path;
  }

  private List<Vector3Int> GetAdjacents(Vector3Int cell, List<Vector3Int> availableNode)
  {
    List<Vector3Int> adjacents = new List<Vector3Int>();
    var left = cell + new Vector3Int(-1, 0);
    var right = cell + new Vector3Int(1, 0);
    var top = cell + new Vector3Int(0, 1);
    var bottom = cell + new Vector3Int(0, -1);

    if (availableNode.Contains(left)) adjacents.Add(left);
    if (availableNode.Contains(right)) adjacents.Add(right);
    if (availableNode.Contains(top)) adjacents.Add(top);
    if (availableNode.Contains(bottom)) adjacents.Add(bottom);

    return adjacents;
  }
}

public struct PathUpdateData
{
  public List<Vector3Int> add { get; set; }
  public List<Vector3Int> remove { get; set; }
}