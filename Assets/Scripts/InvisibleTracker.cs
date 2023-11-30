using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleTracker : MonoBehaviour
{
  private Action _callback = null;
  private float _start = 0;
  private float _time = 0;
  private GameObject _obj = null;

  public static void Create(GameObject obj, float time, Action callback = null)
  {
    GameObject invisibleTrackObj = new GameObject("InvisibleObject");
    obj.transform.SetParent(invisibleTrackObj.transform, true);
    invisibleTrackObj.AddComponent<InvisibleTracker>().Setup(obj, time, callback);
  }

  private void Start()
  {
    _start = Time.time;
  }

  private void Update()
  {
    if(Time.time > _start + _time)
    {
      InvokeCallback();
    }
  }

  public void Setup(GameObject obj, float time, Action callback = null)
  {
    _time = time;
    _obj = obj;
    _callback = callback;
    obj.SetActive(false);
  }

  private void InvokeCallback()
  {
    _obj.SetActive(true);
    _obj.transform.SetParent(null);
    _callback?.Invoke();
    Destroy(this);
  }
}
