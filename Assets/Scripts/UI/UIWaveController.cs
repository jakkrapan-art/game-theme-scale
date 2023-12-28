using System;
using UnityEngine;
using UnityEngine.UI;

public class UIWaveController : MonoBehaviour
{
  [SerializeField]
  private Button _nextWaveBtn = default;
  [SerializeField]
  private Text _nextWaveText = default;

  public void Setup(Action nextWave)
  {
    _nextWaveBtn.onClick.AddListener(() => nextWave?.Invoke());
    Hide();
  }

  public void Show(int nextWave)
  {
    _nextWaveBtn.gameObject.SetActive(true);

    if (_nextWaveText) _nextWaveText.text = nextWave.ToString();
  }

  public void Hide()
  {
    _nextWaveBtn.gameObject.SetActive(false);
  }
}
