using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class UIHelper : MonoBehaviour
{
  private static GameObject _instance;

  private static void CreateInstance()
  {
    if (_instance) return;

    _instance = new GameObject("MonoBehaviourHelper");
    _instance.AddComponent<UIHelper>();
  }

  public static void CreateUI(string objPath, Action<GameObject> callback, Transform parent = null)
  {
    var uiResource = Resources.Load(objPath);
    if (!uiResource) throw new Exception("Not found UI object at path " + objPath);
    CreateInstance();

    var go = Instantiate(uiResource, parent) as GameObject;
    callback?.Invoke(go);
  }
}
