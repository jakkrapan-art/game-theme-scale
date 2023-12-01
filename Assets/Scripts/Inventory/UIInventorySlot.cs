using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIInventorySlot : MonoBehaviour, IDragHandler, IPointerUpHandler
{
  [SerializeField]
  private Image _image;

  private Action _enterBuildAction;

  public void Setup(Sprite image, Action enterBuildMode)
  {
    if(_image) _image.sprite = image;
    _enterBuildAction = enterBuildMode;
  }

  public void OnDrag(PointerEventData eventData)
  {
    _image.transform.position = eventData.position;
    _image.raycastTarget = false;

    EventSystem eventSystem = EventSystem.current;
    List<RaycastResult> raycastHit = new List<RaycastResult>();
    eventSystem.RaycastAll(eventData, raycastHit);
    if (raycastHit.Count <= 1)
    {
      _enterBuildAction?.Invoke();
    }
  }

  public void OnPointerUp(PointerEventData eventData)
  {
    _image.raycastTarget = true;
    _image.transform.localPosition = Vector2.zero;
  }
}
