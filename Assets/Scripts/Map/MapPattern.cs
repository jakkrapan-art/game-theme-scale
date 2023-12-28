using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapPattern", menuName = "ScriptableObject/MapPattern")]
public class MapPattern : ScriptableObject
{
  public Vector2 mapSize = new Vector2(18, 10);
  public Vector3Int enemySpawnPoint = new Vector3Int(0, 0);
  public Vector3Int townPoint = new Vector3Int(15, 5);
  public int pathCount = 15;
  //public List<DrawPathData> pathDir = new List<DrawPathData>();
  public List<Enemy> possibleEnemies = new List<Enemy>();
}

[Serializable]
public class DrawPathData
{
  public Direction direction;
  public int n;
}

public enum Direction
{
  Up, Down, Left, Right
}
