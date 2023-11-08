using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop
{
  private TowerContainer _towerContainer;

  public struct ShopConfig
  {
    public int TowerContainerSize;
  }

  public Shop(ShopConfig config)
  {
    _towerContainer = new TowerContainer(config.TowerContainerSize);
  }
}
