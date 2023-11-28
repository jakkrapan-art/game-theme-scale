using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableDisplay : MonoBehaviour
{
  [SerializeField]
  private SpriteRenderer _sr = null;

  public enum DisplayType
  {
    Placeable = 0, NonPlaceable, Hide
  }

  public void SetDisplay(DisplayType displayType)
  {
    switch(displayType)
    {
      case DisplayType.Placeable:
        SetDisplayColor(new Color32(0, 0, 0, 0));
        break;
      case DisplayType.NonPlaceable:
        SetDisplayColor(new Color32(255, 0, 0, 190));
        break;
      case DisplayType.Hide:
        SetDisplayColor(new Color32(255, 255, 255, 0));
        break;
      default: return;
    }
  }

  private void SetDisplayColor(Color color)
  {
    if(_sr) _sr.color = color;
  }
}
