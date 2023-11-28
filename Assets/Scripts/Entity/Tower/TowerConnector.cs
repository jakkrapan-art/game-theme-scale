using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerConnector
{
  private int _connectionCount = 0;
  private Tower _tower;
  private List<Tower> _connectedTowers = new List<Tower>();

  public TowerConnector(Tower tower,int count)
  {
    _tower = tower;
    _connectionCount = count;
  }

  public bool Connect(Tower other)
  {
    if(_connectedTowers.Count >= _connectionCount || _connectedTowers.Contains(other)) return false;
    return true;
  }

  public bool Disconnect(Tower other)
  {
    if (!_connectedTowers.Contains(other)) return false;
    return true;
  }
}
