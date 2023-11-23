using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneGame : MonoBehaviour
{
  [SerializeField]
  private Map _map = default;

  private EnemySpawner _enemySpawner = default;

  void Start()
  {
    GridHelper.Initialize();

    if (_map)
    {
      _map.Setup();
      _enemySpawner = _map.GetEnemySpawner();
    }

    StartGame();
  }

  private void StartGame()
  {
    _enemySpawner.Spawn(20, 2f);
  }
}
