using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridHelper
{
  private static Grid _grid;
  private static Tilemap _tilemap;
  private static TilemapRenderer _tilemapRenderer;
  private static bool _initialized = false;

  public static void Initialize()
  {
    GameObject gridGo = new GameObject("Grid");
    _grid = gridGo.AddComponent<Grid>();

    GameObject tilemapGo = new GameObject("Tilemap");
    tilemapGo.transform.SetParent(gridGo.transform);
    _tilemap = tilemapGo.AddComponent<Tilemap>();
    _tilemapRenderer = tilemapGo.AddComponent<TilemapRenderer>();

    _initialized = true;
  }

  public static Vector3Int WorldToCell(Vector3 worldPos)
  {
    if (!_initialized) Initialize();
    return _grid.WorldToCell(worldPos);
  }

  public static Vector3 CellToWorld(Vector3Int cell)
  {
    if (!_initialized) Initialize();
    return _grid.CellToWorld(cell);
  }

  public static void SetTile(TileBase tile, Vector3Int cell)
  {
    if (!_initialized) Initialize();
    _tilemap.SetTile(cell, tile);
  }
}
