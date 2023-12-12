using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BuildingSystem : MonoBehaviour
{
  private Tower _tower;
  private Map _map;
  private bool _inBuildMode = false;
  private Action _onExit = null;

  public bool IsInBuildMode() => _inBuildMode;

  public void EnterBuildMode(Map map, Tower tower, Action onExit = null)
  {
    _map = map;
    _tower = tower;
    tower.gameObject.SetActive(true);
    _inBuildMode = true;
    _onExit = onExit;
    tower.SetEnable(false);
  }

  public Tower ExitBuildMode()
  {
    var tower = _tower;
    _tower = null;
    _inBuildMode = false;
    _onExit?.Invoke();
    _onExit = null;
    return tower;
  }

  public void Update()
  {
    if (!_inBuildMode || !_tower) return;

    var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    mousePos.z = 0;
    _map.UpdateTowerPosition(_tower, mousePos);

    if(Input.GetMouseButtonUp(0)) 
    {
      Place();
    }
  }

  private void Place()
  {
    _map.PlaceTower(_tower);
    ExitBuildMode();
  }
}
