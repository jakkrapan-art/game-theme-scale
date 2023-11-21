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

  public ISelectableObject GetCurrentSelecting() => _selectingObj;

  public void SelectObject(ISelectableObject selectableObject)
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

  public void DoSelectingAction()
  {
    if (_selectingObj == null) return;

    switch(_selectingObj)
    {
      case Tower tower:
        _selectingObj = tower.Place();
        break;
      default:
        break;
    }
  }
}
