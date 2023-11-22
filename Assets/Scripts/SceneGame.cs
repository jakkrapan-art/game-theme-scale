using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneGame : MonoBehaviour
{
  [SerializeField]
  private Map map = default;
  void Start()
  {
    GridHelper.Initialize();

    if (map)
    {
      map.Setup(new Vector2Int(18, 10), new Vector3Int(0, 4), new Vector3Int(0, -5), 10);
    }
  }
}
