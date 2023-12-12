using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController
{
  private EnemySpawner _spawner;
  private int _baseCount;
  private int _currentWave;
  private float _spawnInterval;
  private float _increaseMultiply;

  private Action<int> _onWaveEndAction;

  public struct SetupParam
  {
    public EnemySpawner spawner;
    public int baseCount;
    public float spawnInterval;
    public Action<int> onWaveEnd;
  }

  public WaveController(SetupParam param)
  {
    _spawner = param.spawner;
    _baseCount = param.baseCount;
    _spawnInterval = param.spawnInterval;
    _currentWave = 0;
    _onWaveEndAction = param.onWaveEnd;
  }

  public void SpawnEnemy()
  {
    _currentWave++;
    _spawner.Spawn(GetCurrentCount(), _spawnInterval, () => { _onWaveEndAction?.Invoke(_currentWave); });
  }

  public void SpawnBoss()
  {
    _currentWave++;
    _spawner.SpawnBoss(()=> { _onWaveEndAction?.Invoke(_currentWave); });
  }

  private int GetCurrentCount()
  {
    return Mathf.RoundToInt(_baseCount + ((_currentWave - 1) * _increaseMultiply));
  }
}
