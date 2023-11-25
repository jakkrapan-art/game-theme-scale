using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventory
{
  [SerializeField]
  private UIInventorySlot _slotTemplate = default;

  public struct SetupData
  {
    public Wallet.CurrencyData gold;
  }

  public void Setup(SetupData data)
  {

  }
}
