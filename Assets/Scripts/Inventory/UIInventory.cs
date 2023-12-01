using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour, IGameUI
{
  [SerializeField]
  private UIInventorySlot _slotTemplate = default;
  [SerializeField]
  private Transform _slotParent = default;
  [SerializeField]
  private ITransition _transition = default;

  [SerializeField]
  private Text _goldText = default;

  [SerializeField]
  private Button _openBpButton = default;
  [SerializeField]
  private Button _closeButton = default;

  public struct SetupData
  {
    public int gold;
    public Inventory.ItemData[] items;
    public Action<Tower> enterBuildMode;
    public Action onClose;
  }

  public void Setup(SetupData data)
  {
    while (_slotParent.childCount > 0)
    {
      DestroyImmediate(_slotParent.GetChild(0).gameObject);
    }

    if (_goldText) _goldText.text = data.gold.ToString();
    foreach (var item in data.items)
    {
      var slot = Instantiate(_slotTemplate, _slotParent);
      slot.Setup(item.icon, () =>
      {
        data.enterBuildMode?.Invoke(item.tower);
        Destroy(slot);
      });
    }
    if(_closeButton)
    {
      _closeButton.onClick.RemoveAllListeners();
      _closeButton.onClick.AddListener(() => { data.onClose?.Invoke(); });
    }
    TryGetComponent(out _transition);
  }

  public void SetOnClickBackpackBtnAction(Action action)
  {
    if (_openBpButton)
    {
      _openBpButton.onClick.RemoveAllListeners();
      _openBpButton.onClick.AddListener(() => { action?.Invoke(); });
    }
  }


  public void UpdateGold(int amount)
  {
    if(_goldText)_goldText.text = amount.ToString();
    Canvas.ForceUpdateCanvases();
  }

  public void Show()
  {
    _transition?.TransitionIn();
    _openBpButton.gameObject.SetActive(false);
  }

  public void Hide()
  {
    _transition?.TransitionOut();
    _openBpButton.gameObject.SetActive(true);
  }

  public void HideOpenButton()
  {
    _openBpButton?.gameObject.SetActive(false);
  }

  public void ShowOpenButton()
  {
    _openBpButton?.gameObject.SetActive(true);
  }
}
