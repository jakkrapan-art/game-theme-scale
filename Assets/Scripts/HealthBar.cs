using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
  [SerializeField]
  private Slider _slider = null;
  private int _max = 0;
  public void Setup(int maxValue)
  {
    _max = maxValue;
  }

  public void UpdateBar(int value)
  {
    float barVal = (float)value / (float)_max;
    _slider.value = barVal;
  }
}
