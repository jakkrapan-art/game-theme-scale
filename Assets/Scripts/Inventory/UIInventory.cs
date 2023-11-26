using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour, IGameUI
{
  [SerializeField]
  private UIInventorySlot _slotTemplate = default;
  [SerializeField]
  private ITransition _transition = default;

  [SerializeField]
  private Text _goldText = default;

  public struct SetupData
  {
    public int gold;
  }

  public void Setup(SetupData data)
  {
    if (_goldText) _goldText.text = data.gold.ToString();
    TryGetComponent(out _transition);
  }

  public void Show()
  {
    _transition?.TransitionIn();
  }

  public void Hide()
  {
    _transition?.TransitionOut();
  }
}
