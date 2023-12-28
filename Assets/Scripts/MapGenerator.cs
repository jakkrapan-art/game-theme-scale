using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator
{
  private struct AvailableAdjacent
  {
    public bool top;
    public bool bottom;
    public bool left;
    public bool right;
  }

  private enum Direction { UP = 0, RIGHT, DOWN, LEFT}
  private enum TurnDirection { LEFT = -1, RIGHT = 1, NONE = 0}

  private Dictionary<Direction, Vector3Int> _directionValues = new Dictionary<Direction, Vector3Int>()
  {
    { Direction.UP, new Vector3Int(0, 1) },
    { Direction.DOWN, new Vector3Int(0, -1) },
    { Direction.LEFT, new Vector3Int(-1, 0) },
    { Direction.RIGHT, new Vector3Int(1, 0) }
  };

  private Direction _currentDirection;
  private TurnDirection _lastTurn;

  private MonoBehaviour _mono;
  private Vector2Int _size;
  private TileBase _dirtTile;
  private TileBase _pathTile;
  private Dictionary<Vector3Int, Node> _availableNodes = new Dictionary<Vector3Int, Node>();

  public MapGenerator(MonoBehaviour mono,Vector2Int size, TileBase dirtTile, TileBase pathTile)
  {
    _mono = mono;
    _size = size;
    _dirtTile = dirtTile;
    _pathTile = pathTile;
  }

  private int GetHighestRow() { return _size.y - 1; }
  private int GetHighestColumn() { return _size.x - 1; }

  public void Generate()
  {
    _lastTurn = TurnDirection.NONE;
    _mono?.StartCoroutine(DoGenerate());
  }

  private IEnumerator DoGenerate()
  {
    GenerateDirt();
    yield return GeneratePath();
  }

  private void GenerateDirt()
  {
    for (int x = 0; x < _size.x; x++)
    {
      for (int y = 0;y < _size.y; y++)
      {
        SetTile(_dirtTile, new Node(x, y, true));
      }
    }
  }

  private IEnumerator GeneratePath() 
  {
    List<Node> path = new List<Node>();

    Node start = _availableNodes[new Vector3Int(Mathf.RoundToInt(_size.x / 2f), _size.y - 1)];
    Node end = _availableNodes[new Vector3Int(Random.Range(0, _size.x), 0)];

    Node current = start;
    _currentDirection = Direction.DOWN;

    while (true) 
    {
      current.SetAvailable(false);
      SetTile(_pathTile, current);
      path.Add(current);

      if(current.Equals(end)) break;

      var nextPoint = GetNextPoint(current);
      if (nextPoint == null) throw new System.Exception("Can not find next point in available nodes");
      if (!IsCanReachEndPoint(nextPoint, end))
      {
        Debug.Log("next point cannot reach endpoint!");
        nextPoint.SetAvailable(false);
        Turn((TurnDirection)(-((int)_lastTurn)));
      }
      else
      {
        current = nextPoint; //set current to next point
      }

      Debug.Log("current: " + current.GetCell() + ", next: " + nextPoint.GetCell());
      UpdateDirection(current, end);

      yield return new WaitForSeconds(0.25f);
    }

    Debug.Log("Generate path success.");
  }

  private Node GetNextPoint(Node currentPoint)
  {
    var cell = currentPoint.GetCell() + _directionValues[_currentDirection];
    if(_availableNodes.ContainsKey(cell)) return _availableNodes[cell];
    return null;
  }

  private void UpdateDirection(Node currentPoint, Node endPoint)
  {
    switch(_currentDirection)
    {
      case Direction.UP:
        DoFacingUpTurn(currentPoint);
        break;
      case Direction.DOWN:
        DoFactingDownTurn(currentPoint, endPoint);
        break;
      case Direction.LEFT:
        DoFacingLeftTurn(currentPoint);
        break;
      case Direction.RIGHT:
        DoFacingRightTurn(currentPoint);
        break;
    }
  }

  private void DoFacingUpTurn(Node current)
  {
    if(current.y == GetHighestRow())
    {
      bool availableLeft = IsAvailableLeft(current);
      bool availableRight = IsAvailableRight(current);

      if(availableLeft && availableRight)
      {
        int randomDir = Random.Range(0, 2);
        switch(randomDir)
        {
          case 0:
            Turn(TurnDirection.LEFT);
            return;
          case 1:
            Turn(TurnDirection.RIGHT);
            return;
        }
      }
      else if(availableLeft && !availableRight) 
      {
        Turn(TurnDirection.LEFT);
        return;
      }
      else if (!availableLeft && availableRight) 
      {
        Turn(TurnDirection.RIGHT);
        return;
      }
      else
      {
        throw new System.Exception("Don't have available nodes while facing top at highest position");
      }
    }

    int random = Random.Range(0, 2); //0 = same dir

    if(current.y != GetHighestRow() && random == 0 && IsAvailableTop(current)) return;

    Turn((TurnDirection)((int)_lastTurn * -1));
  }

  private void DoFactingDownTurn(Node current, Node endPoint) 
  {
    int random = Random.Range(0, 2);
    bool availableBottom = IsAvailableBottom(current);

    if(random == 0 && availableBottom) return;

    bool availableLeft = IsAvailableLeft(current);
    bool availableRight = IsAvailableRight(current);

    if (current.y == 0)
    {
      TurnDirection turnDirection;
      if (current.x < endPoint.x) turnDirection = TurnDirection.LEFT;
      else turnDirection = TurnDirection.RIGHT;

      Turn(turnDirection);
    }
    else
    {
      if (availableLeft && availableRight)
      {
        int randomDir = Random.Range(0, 2); //0 = left, 1 = right
        switch(randomDir)
        {
          case 0:
            Turn(TurnDirection.RIGHT);
            break;
          default:
            Turn(TurnDirection.LEFT);
            break;
        }
      }
      else
      {
        if (availableLeft) Turn(TurnDirection.RIGHT);
        else if (availableRight) Turn(TurnDirection.LEFT);
      }
    }
  }

  private void DoFacingLeftTurn(Node current) 
  {
    if(current.x == 0)
    {
      Turn(TurnDirection.LEFT);
      return;
    }

    bool availableLeft = IsAvailableLeft(current);
    bool availableTop = IsAvailableTop(current);
    bool availableBottom = IsAvailableBottom(current);

    if(availableLeft && availableTop && availableBottom) 
    {
      int random = Random.Range(0, 3); //0 left, 1 top, 2 bottom
      switch(random)
      {
        case 0: return; //Do nothing
        case 1:
          Turn(TurnDirection.RIGHT);
          break;
        case 2:
          Turn(TurnDirection.LEFT);
          break;
      }
    }
    else if(availableLeft && availableTop && !availableBottom) 
    {
      int random = Random.Range(0, 2); //0 left, 1 top
      switch(random)
      {
        case 0: return;
        case 1:
          Turn(TurnDirection.RIGHT);
          break;
      }
    }
    else if( availableLeft && !availableTop && availableBottom)
    {
      int random = Random.Range(0, 2); //0 left, 1 bottom
      switch (random)
      {
        case 0: return;
        case 1:
          Turn(TurnDirection.LEFT);
          break;
      }
    }
    else if (!availableLeft && availableTop && availableBottom)
    {
      int random = Random.Range(0, 2); //0 top, 1 bottom
      switch (random)
      {
        case 0:
          Turn(TurnDirection.LEFT);
          break;
        case 1:
          Turn(TurnDirection.RIGHT);
          break;
      }
    }
    else if (!availableLeft && availableTop && !availableBottom)
    {
      Turn(TurnDirection.RIGHT);
    }
    else if (!availableLeft && !availableTop && availableBottom)
    {
      Turn(TurnDirection.LEFT);
    }
  }

  private void DoFacingRightTurn(Node current)
  {
    if (current.x == GetHighestColumn())
    {
      Turn(TurnDirection.RIGHT);
      return;
    }

    bool availableRight = IsAvailableRight(current);
    bool availableTop = IsAvailableTop(current);
    bool availableBottom = IsAvailableBottom(current);

    if (availableRight && availableTop && availableBottom)
    {
      int random = Random.Range(0, 3); //0 right, 1 top, 2 bottom
      switch (random)
      {
        case 0: return; //Do nothing
        case 1:
          Turn(TurnDirection.LEFT);
          break;
        case 2:
          Turn(TurnDirection.RIGHT);
          break;
      }
    }
    else if (availableRight && availableTop && !availableBottom)
    {
      int random = Random.Range(0, 2); //0 no turn, 1 turn to top
      switch (random)
      {
        case 0: return;
        case 1:
          Turn(TurnDirection.LEFT);
          break;
      }
    }
    else if (availableRight && !availableTop && availableBottom)
    {
      int random = Random.Range(0, 2); //0 no turn, 1 turn to bottom
      switch (random)
      {
        case 0: return;
        case 1:
          Turn(TurnDirection.RIGHT);
          break;
      }
    }
    else if(!availableRight && availableTop && availableBottom)
    {
      int random = Random.Range(0, 2); //0 top, 1 bottom
      switch (random)
      {
        case 0:
          Turn(TurnDirection.LEFT);
          break;
        case 1:
          Turn(TurnDirection.RIGHT);
          break;
      }
    }
    else if (!availableRight && availableTop && !availableBottom)
    {
      Turn(TurnDirection.LEFT);
    }
    else if (!availableRight && !availableTop && availableBottom)
    {
      Turn(TurnDirection.RIGHT);
    }
  }

  private void Turn(TurnDirection turnDirection)
  {
    _lastTurn = turnDirection;

    Debug.Log("turn direction: " + turnDirection);
    int newDirection = ((int)_currentDirection + (int)turnDirection) % 4;
    if (newDirection < 0) newDirection = 3;
    _currentDirection = (Direction)newDirection;
  }

  private bool IsAvailableTop(Node current)
  {
    var up = GetTopNode(current);
    if(up == null) return false;
    return _availableNodes.ContainsKey(up.GetCell()) && _availableNodes[up.GetCell()].available;
  }

  private bool IsAvailableBottom(Node current) 
  {
    var down = GetBottomNode(current);
    if (down == null) return false;
    return _availableNodes.ContainsKey(down.GetCell()) && _availableNodes[down.GetCell()].available;
  }

  private bool IsAvailableLeft(Node current)
  {
    var left = GetLeftNode(current);
    if (left == null) return false;
    return _availableNodes.ContainsKey(left.GetCell()) && _availableNodes[left.GetCell()].available;
  }

  private bool IsAvailableRight(Node current)
  {
    var right = GetRightNode(current);
    if (right == null) return false;
    return _availableNodes.ContainsKey(right.GetCell()) && _availableNodes[right.GetCell()].available;
  }

  private bool IsCanReachEndPoint(Node from, Node end)
  {
    int i = 0;
    List<Node> visited = new List<Node>();
    Queue<Node> queue = new Queue<Node>();

    queue.Enqueue(from);

    while (queue.Count > 0 && i < 1000) 
    {
      i++;
      var current = queue.Dequeue();
      if (current == end) return true;
      visited.Add(current);

      var adjacents = GetAdjacentCells(current);
      foreach (var adjacent in adjacents)
      {
        if (queue.Contains(adjacent) || visited.Contains(adjacent)) continue;
        queue.Enqueue(adjacent);
      }
    }

    return false;
  }

  private List<Node> GetAdjacentCells(Node current)
  {
    List<Node> adjacents = new List<Node>();
    if (IsAvailableTop(current)) 
    { 
      adjacents.Add(GetTopNode(current));
    }
    if (IsAvailableBottom(current)) 
    { 
      adjacents.Add(GetBottomNode(current));
    }
    if (IsAvailableLeft(current)) 
    { 
      adjacents.Add(GetLeftNode(current));
    }
    if (IsAvailableRight(current))
    {
      adjacents.Add(GetRightNode(current));
    }

    return adjacents;
  }

  private Node GetLeftNode(Node from)
  {
    var cell = from.GetCell() + _directionValues[Direction.LEFT];
    if (_availableNodes.ContainsKey(cell)) return _availableNodes[cell];
    return null;
  }
  private Node GetRightNode(Node from)
  {
    var cell = from.GetCell() + _directionValues[Direction.RIGHT];
    if (_availableNodes.ContainsKey(cell)) return _availableNodes[cell];
    return null;
  }
  private Node GetTopNode(Node from)
  {
    var cell = from.GetCell() + _directionValues[Direction.UP];
    if(_availableNodes.ContainsKey(cell)) return _availableNodes[cell];
    return null;
  }
  private Node GetBottomNode(Node from)
  {
    var cell = from.GetCell() + _directionValues[Direction.DOWN];
    if (_availableNodes.ContainsKey(cell)) return _availableNodes[cell];
    return null;
  }

  private void SetTile(TileBase tile, Node node)
  {
    GridHelper.SetTile(tile, node.GetCell());
    if(_availableNodes.ContainsKey(node.GetCell()))
      _availableNodes[node.GetCell()].SetAvailable(node.available);
    else
      _availableNodes.Add(node.GetCell(), node);
  }
}
