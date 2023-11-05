using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBar : MonoBehaviour
{
  [SerializeField]
  private Slider _slider = null;
  private float _max = 0;
  public void Setup(float maxValue)
  {
    _max = maxValue;
    gameObject.SetActive(false);
  }

  public void UpdateBar(float value)
  {
    float barVal = value / _max;
    _slider.value = barVal;
  }
}
