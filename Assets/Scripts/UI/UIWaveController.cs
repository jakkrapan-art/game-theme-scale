using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWaveController : MonoBehaviour
{
  [SerializeField]
  private Button _nextWaveBtn = default;
  public void Setup(Action nextWave)
  {
    _nextWaveBtn.onClick.AddListener(() => nextWave?.Invoke());
    Hide();
  }

  public void Show()
  {
    _nextWaveBtn.gameObject.SetActive(true);
  }

  public void Hide()
  {
    _nextWaveBtn.gameObject.SetActive(false);
  }
}
