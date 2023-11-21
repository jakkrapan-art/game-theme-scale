using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridHelper
{
  private static Grid _grid;
  private static Tilemap _tilemap;
  private static bool _initialized = false;

  public static void Initialize()
  {
    GameObject gridGo = new GameObject("Grid");
    _grid = gridGo.AddComponent<Grid>();

    _initialized = true;
  }

  public static Vector3Int WorldToCell(Vector3 worldPos)
  {
    if (!_initialized) throw new System.Exception("Cannot call GridHelper functions without initialize.");
    return _grid.WorldToCell(worldPos);
  }

  public static Vector3 CellToWorld(Vector3Int cell)
  {
    if (!_initialized) throw new System.Exception("Cannot call GridHelper functions without initialize.");
    return _grid.CellToWorld(cell);
  }
}
