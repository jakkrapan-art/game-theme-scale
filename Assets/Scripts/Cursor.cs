using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
  private static Cursor _instance = null;
  private ISelectableObject _selectingObj = null;

  public static Cursor GetInstance()
  {
    if(!_instance)
    {
      GameObject go = new GameObject("CursorController");
      _instance = go.AddComponent<Cursor>();
      _instance.Setup();
    }

    return _instance;
  }

  public void Setup()
  {

  }

  public void SelectObect(ISelectableObject selectableObject)
  {
    if(_selectingObj != null)
    {
      _selectingObj.Deselect();
      if (_selectingObj == selectableObject)
      {
        _selectingObj = null;
        return;
      }
    }

    _selectingObj = selectableObject;
    _selectingObj.Select();
  }
}
