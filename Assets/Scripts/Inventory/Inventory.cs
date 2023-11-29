using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
  private Wallet _wallet;
  private TowerContainer _towerContainer;

  private UIInventory _ui;
  private bool _isShowing = false;
  private string _inventoryPath = "Templates/UI/UIInventory";

  #region Getter
  public List<Tower> GetTowers() => _towerContainer.GetTowers();
  public Wallet GetWallet() => _wallet;
  #endregion
  #region Setter
  #endregion

  public Inventory(Wallet.SetupData walletSetupData, int towerContainerSize)
  {
    _wallet = new Wallet(walletSetupData);
    _towerContainer= new TowerContainer(towerContainerSize);
  }

  public bool AddTower(Tower tower)
  {
    return _towerContainer.AddTower(tower);
  }

  public bool RemoveTower(Tower tower) 
  {
    return _towerContainer.RemoveTower(tower);
  }

  public void CreateUI()
  {
    UIHelper.CreateUI(_inventoryPath, uiObj => 
    {
      uiObj.TryGetComponent(out _ui);
    });
  }

  public void ToggleUI()
  {
    _isShowing = !_isShowing;

    if(_isShowing)
    {
      ShowUI();
    }
    else
    {
      HideUI();
    }
  }

  private void ShowUI()
  {
    if (!_ui) CreateUI();

    _ui.Setup(new UIInventory.SetupData
    {
      gold = _wallet.GetCurrency(Wallet.CurrencyType.Gold),
    });

    _ui.Show();
  }

  private void HideUI()
  {
    if (!_ui) CreateUI();
    _ui.Hide();
  }
}