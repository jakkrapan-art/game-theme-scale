using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
  [SerializeField]
  private Vector3Int[] cells = default;

  public void Spawn(Enemy enemy)
  {
    enemy.transform.position = GridHelper.CellToWorld(cells[0]);
  }
}
