using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerContainer
{
  private List<Tower> _towerList;
  private int _size;

  #region Getter
  public List<Tower> GetTowers() => _towerList;
  #endregion

  #region Setter
  #endregion

  public TowerContainer(int size)
  {
    _size = size;
  }

  public bool AddTower(Tower tower)
  {
    if (_towerList.Count >= _size) return false;
    _towerList.Add(tower);
    return true;
  }

  public bool RemoveTower(Tower tower)
  {
    if (!_towerList.Contains(tower)) return false;
    _towerList.Remove(tower);
    return true;
  }
}
