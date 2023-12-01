using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
  public struct PlayerInitData
  {
    public int inventorySize;
    public Wallet.SetupData walletSetupData;

    public Action<Tower> enterBuildMode;
  }

  private Inventory _inventory;

  #region Getter
  public Inventory GetInventory() => _inventory;
  #endregion

  public Player(PlayerInitData data)
  {
    _inventory = new Inventory(data.walletSetupData, data.inventorySize, data.enterBuildMode);
  }
}
