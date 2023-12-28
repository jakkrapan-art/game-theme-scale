using UnityEngine;

public class Node
{
  public int x { get; }
  public int y { get; }
  public bool available { get; private set; }

  public Node(int x, int y, bool available = true)
  {
    this.x = x;
    this.y = y;
    this.available = available;
  }

  public Vector3Int GetCell() => new Vector3Int(x, y);
  public void SetAvailable(bool available)
  {
    Debug.Log("set node available: " + available);
    this.available = available;
  }
}