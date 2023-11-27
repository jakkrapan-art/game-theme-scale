using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBar : MonoBehaviour
{
  [SerializeField]
  private Slider _slider = null;
  private float _max = 0;

  private Coroutine _slowUpdateCoroutine = null;

  public void Setup(float maxValue)
  {
    _max = maxValue;
    UpdateBar(maxValue);
    gameObject.SetActive(false);
  }

  public void UpdateMaxValue(float newValue) 
  {
    _max = newValue;
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
      if (_slowUpdateCoroutine != null) StopCoroutine(_slowUpdateCoroutine);
      _slowUpdateCoroutine = StartCoroutine(SlowUpdateBar(barVal, 0.8f));
    }
  }

  private IEnumerator SlowUpdateBar(float target, float time)
  {
    float current = _slider.value;
    float start = Time.time;
    while (Time.time <= start + time) 
    {
      current = Mathf.Lerp(current, target, 0.2f);
      _slider.value = current;
      yield return new WaitForEndOfFrame();
    }

    _slider.value = current;
  }
}
