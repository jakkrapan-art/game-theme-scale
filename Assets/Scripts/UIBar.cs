using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBar : MonoBehaviour
{
  [SerializeField]
  private Slider _slider = null;
  private float _max = 0;

  private float _targetValue = 0;
  private bool _slowUpdateBar = false;

  public void Setup(float maxValue)
  {
    _max = maxValue;
    UpdateBar(maxValue);
    gameObject.SetActive(false);
  }

  private void Update()
  {
    SlowUpdateBar();
  }

  public void UpdateBar(float value, bool instant = true)
  {
    float barVal = value / _max;
    if (instant)
    {
      _slider.value = barVal;
    }
    else
    {
      _targetValue = barVal;
      _slowUpdateBar = true;
    }
  }

  private void SlowUpdateBar()
  {
    if (!_slowUpdateBar) return;

    if(!Mathf.Approximately(_slider.value, _targetValue))
    {
      float newVal = Mathf.Lerp(_slider.value, _targetValue, 0.12f);
      _slider.value = newVal;
    }
    else
    {
      _slider.value = _targetValue;
      _slowUpdateBar = false;
    }
  }
}
