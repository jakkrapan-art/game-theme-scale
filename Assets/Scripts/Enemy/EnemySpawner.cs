using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
  private List<Enemy> _possibleEnemies = new List<Enemy>();
  private Vector3 _spawnPoint = default;
  private List<Vector3Int> _movePath = new List<Vector3Int>();
  private SpawnType _spawnType = default;
  public enum SpawnType { Random, Fix }

  [SerializeField]
  private ScalableEnemy _boss = default;

  public void Setup(List<Enemy> enemies, Vector3Int spawnPoint, List<Vector3Int> movePath, SpawnType spawnType = SpawnType.Fix)
  {
    _possibleEnemies = enemies;
    _spawnPoint = spawnPoint;
    _movePath = movePath;
  }

  public void Spawn(int count, float spawnInterval)
  {
    StartCoroutine(DoSpawn(count, spawnInterval));
  }

  private IEnumerator DoSpawn(int count, float spawnInterval)
  {
    int spawnedCount = 0;

    Enemy enemy = GetRandomEnemy();
    if (!enemy) yield break;

    while (spawnedCount < count) 
    {
      SpawnEnemy(enemy);
      spawnedCount++;

      yield return new WaitForSeconds(spawnInterval);

      if(_spawnType == SpawnType.Random)
      {
        enemy = GetRandomEnemy();
        if(!enemy) yield break;
      }
    }

    SpawnEnemy(_boss);
  }

  private void SpawnEnemy(Enemy enemy) 
  {
    if (!enemy) return;

    var spawned = Instantiate(enemy, _spawnPoint + new Vector3(enemy.transform.localScale.x/2, enemy.transform.localScale.y/2), Quaternion.identity);
    spawned.SetMovePath(_movePath);
  }

  private Enemy GetRandomEnemy()
  {
    if (_possibleEnemies == null || _possibleEnemies.Count == 0) return null;

    return _possibleEnemies[Random.Range(0, _possibleEnemies.Count)];
  }
}
