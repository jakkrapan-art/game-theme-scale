using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
  public struct PlayerInitData
  {
    public int inventorySize;
  }

  private Inventory _inventory;
  private Wallet _wallet;

  public Player(PlayerInitData data)
  {
    _inventory = new Inventory(data.inventorySize);
    _wallet = new Wallet(new List<Wallet.CurrencyData>());
  }
}
