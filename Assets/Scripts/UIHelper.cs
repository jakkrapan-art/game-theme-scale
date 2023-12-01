using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class UIHelper : MonoBehaviour
{
  private static readonly string UI_PREFIX = "Templates/UI/";

  private static GameObject _instance;

  private static void CreateInstance()
  {
    if (_instance) return;

    _instance = new GameObject("UIHelper");
    _instance.AddComponent<UIHelper>();
  }

  public static void CreateUI(string objPath, Action<GameObject> callback, Transform parent = null)
  {
    var uiResource = Resources.Load(UI_PREFIX + objPath);
    if (!uiResource) throw new Exception("Not found UI object at path " + objPath);
    CreateInstance();

    var go = Instantiate(uiResource, parent) as GameObject;
    callback?.Invoke(go);
  }

  public static void CreateInventoryUI(Action<UIInventory> callback)
  {
    CreateUI("UIInventory", (ui) => 
    {
      if (ui.TryGetComponent<UIInventory>(out var inventory)) callback?.Invoke(inventory);
    });
  }

  public static void CreateHealthUI(Action<UIHealth> callback)
  {
    CreateUI("UIHealth", (ui) =>
    {
      if (ui.TryGetComponent<UIHealth>(out var health)) callback?.Invoke(health);
    });
  }
}
