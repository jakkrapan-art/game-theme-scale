using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
  public struct PlayerInitData
  {
    public int inventorySize;
    public Wallet.SetupData walletSetupData;
  }

  private Inventory _inventory;

  #region Getter
  public Inventory GetInventory() => _inventory;
  #endregion

  public Player(PlayerInitData data)
  {
    _inventory = new Inventory(data.walletSetupData, data.inventorySize);
  }
}
