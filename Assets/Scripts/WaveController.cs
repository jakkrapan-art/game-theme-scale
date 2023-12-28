using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController
{
  private readonly EnemySpawner _spawner;
  private readonly int _baseCount;
  private readonly float _spawnInterval;
  private readonly float _increaseMultiply;

  private int _currentWave;

  private UIWaveController _ui;

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

    CreateUI();
  }

  private void CreateUI()
  {
    UIHelper.CreateWaveUI((ui) =>
    {
      _ui = ui;
      ui.Setup(() =>
      {
      });
    });
  }

  public void ShowUI()
  {
    if (!_ui) return;
    _ui.Show(_currentWave);
  }

  public void HideUI()
  {
    _ui?.Hide();
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
