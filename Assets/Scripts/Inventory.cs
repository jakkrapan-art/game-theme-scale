using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
  private TowerContainer towerContainer;

  #region Getter
  #endregion
  #region Setter
  #endregion

  public Inventory(int towerContainerSize)
  {
    towerContainer= new TowerContainer(towerContainerSize);
  }

  public bool AddTower(Tower tower)
  {
    return towerContainer.AddTower(tower);
  }

  public bool RemoveTower(Tower tower) 
  {
    return towerContainer.RemoveTower(tower);
  }
}
