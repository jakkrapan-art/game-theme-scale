using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHealth : MonoBehaviour
{
  [SerializeField]
  private GameObject _heartTemplate;
  [SerializeField]
  private Transform _heartParent;
  private List<GameObject> _heartObjs = new List<GameObject>();

  public void Setup(int heartCount)
  {
    if (_heartParent && _heartTemplate)
    {
      for (int i = 0; i < heartCount; i++)
      {
        CreateHeart();
      }
    }
  }

  public void UpdateHearth(int currentCount)
  {
    if (currentCount < 0) return;

    if (_heartObjs.Count > currentCount)
    {
      while (_heartObjs.Count > currentCount)
      {
        var heart = _heartObjs[_heartObjs.Count - 1];
        _heartObjs.Remove(heart);
        DestroyImmediate(heart);
      }
    }
    else
    {
      int diff = currentCount - _heartObjs.Count;
      if (diff < 0) return;

      for (int i = 0; i < diff; i++)
      {
        CreateHeart();
      }
    }
  }

  private void CreateHeart()
  {
    var instantiated = Instantiate(_heartTemplate, _heartParent);
    _heartObjs.Add(instantiated);
  }
}
