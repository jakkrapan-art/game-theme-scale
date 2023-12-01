using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
  private Wallet _wallet;
  private TowerContainer _towerContainer;

  private UIInventory _ui;
  private bool _isShowing = false;

  private Action<Tower> _enterBuildModeAction;

  #region Getter
  public List<Tower> GetTowers() => _towerContainer.GetTowers();
  public Wallet GetWallet() => _wallet;
  #endregion
  #region Setter
  #endregion

  public struct ItemData
  {
    public Tower tower;
    public Sprite icon;
  }

  public Inventory(Wallet.SetupData walletSetupData, int towerContainerSize, Action<Tower> enterBuildMode)
  {
    _wallet = new Wallet(walletSetupData);
    _towerContainer= new TowerContainer(towerContainerSize);
    _enterBuildModeAction = enterBuildMode;

    CreateUI();
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
    UIHelper.CreateInventoryUI(ui => 
    {
      _ui = ui;
      _ui.SetOnClickBackpackBtnAction(()=> { ShowUI(); });
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

  public void ShowUI()
  {
    if (!_ui) CreateUI();

    _wallet.SubscribeOnUpdateGold((newVal) => { UpdateGoldUI(); });

    var towers = _towerContainer.GetTowers();
    ItemData[] uiTowerData = new ItemData[towers.Count];
    for (int i = 0; i < towers.Count; i++)
    {
      var tower = towers[i];
      uiTowerData[i] = new ItemData
      { 
        icon = tower.GetSpriteRenderer().sprite,
        tower = tower,
      };
    }

    _ui.Setup(new UIInventory.SetupData
    {
      gold = _wallet.GetCurrency(Wallet.CurrencyType.Gold),
      items = uiTowerData,
      enterBuildMode = (tower) => 
      {
        _enterBuildModeAction?.Invoke(tower);
        RemoveTower(tower);
        HideUI();
      },
      onClose = HideUI
    });

    _ui.Show();
    _isShowing = true;
  }

  public void HideUI()
  {
    if (!_ui) CreateUI();
    _wallet.UnsubscribeOnUpdateGold((newVal) => { UpdateGoldUI(); });
    _ui.Hide();
    _isShowing = false;
  }

  private void UpdateGoldUI()
  {
    var currentGold = _wallet.GetCurrency(Wallet.CurrencyType.Gold);
    _ui.UpdateGold(currentGold);
  }

  public void SetActiveOpenButton(bool active)
  {
    if (active)
    {
      _ui.ShowOpenButton();
    }
    else
    {
      _ui.HideOpenButton();
    }
  }
}