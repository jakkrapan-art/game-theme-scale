using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
  private List<Enemy> _possibleEnemies = new List<Enemy>();
  private Vector3 _spawnPoint = default;
  private List<Vector3Int> _movePath = new List<Vector3Int>();
  private SpawnType _spawnType = default;
  public enum SpawnType { Random, Fix }
  private Action<EnemyType> _onEnemyMoveFinish;
  public enum EnemyType { Minion, Boss }

  [SerializeField]
  private ScalableEnemy _boss = default;

  public struct MoveData
  {
    public List<Vector3Int> movePath;
    public Action<EnemyType> onMoveFinish;
  }

  public void Setup(List<Enemy> enemies, Vector3Int spawnPoint, MoveData moveData, SpawnType spawnType = SpawnType.Fix)
  {
    _possibleEnemies = enemies;
    _spawnPoint = spawnPoint;
    _movePath = moveData.movePath;
    _onEnemyMoveFinish = moveData.onMoveFinish;
    _spawnType = spawnType;
  }

  public void Spawn(int count, float spawnInterval, Action onAllDie = null)
  {
    StartCoroutine(DoSpawn(count, spawnInterval, onAllDie));
  }

  private IEnumerator DoSpawn(int count, float spawnInterval, Action callback)
  {
    int spawnedCount = 0;

    Enemy enemy = GetRandomEnemy();
    if (!enemy) yield break;

    int remainCount = count;

    while (spawnedCount < count) 
    {
      var minion = SpawnEnemy(enemy);
      if (!minion) continue;

      minion.SubscribeOnReachTarget(() => _onEnemyMoveFinish?.Invoke(EnemyType.Minion));
      minion.SubscribeOnDestroy(() =>
      {
        remainCount--;
        if (remainCount == 0)
        {
          callback?.Invoke();
        }
      });

      spawnedCount++;

      yield return new WaitForSeconds(spawnInterval);

      if(_spawnType == SpawnType.Random)
      {
        enemy = GetRandomEnemy();
        if(!enemy) yield break;
      }
    }
  }

  public void SpawnBoss(Action callback)
  {
    var boss = SpawnEnemy(_boss, callback);
    if (boss) boss.SubscribeOnReachTarget(() => _onEnemyMoveFinish?.Invoke(EnemyType.Boss));
  }

  private Enemy SpawnEnemy(Enemy enemy, Action onDestroy = null, Action onDieCallback = null) 
  {
    if (!enemy) return null;

    var spawned = Instantiate(enemy, _spawnPoint + new Vector3(enemy.transform.localScale.x/2, enemy.transform.localScale.y/2), Quaternion.identity);
    spawned.SetMovePath(_movePath);

    var enemyCompo = spawned.GetComponent<Enemy>();
    enemyCompo?.SubscribeOnDie(onDieCallback);
    enemyCompo?.SubscribeOnDestroy(onDestroy);
    return spawned;
  }

  private Enemy GetRandomEnemy()
  {
    if (_possibleEnemies == null || _possibleEnemies.Count == 0) return null;

    return _possibleEnemies[Random.Range(0, _possibleEnemies.Count)];
  }
}
