using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
  private Map _map = default;
  private EnemySpawner _enemySpawner = default;
  private Player _player;
  private MudCollector _collector;

  private string _mapPath = "Templates/Map";
  private string _mudCollectorPath = "Templates/MudCollector";

  private int _wave = 0;
  private int _defaultWaveCount = 5;
  private float _waveMultiplier = 1;

  public void Initialize()
  {
    _player = new Player(new Player.PlayerInitData
    {
      inventorySize = 15,
      walletSetupData = new Wallet.SetupData 
      { 
        currencyDatas = new List<Wallet.CurrencyData> 
        { 
          new Wallet.CurrencyData(Wallet.CurrencyType.Gold, 0),
          new Wallet.CurrencyData(Wallet.CurrencyType.Meat, 0)
        }
      }
    });

    GridHelper.Initialize();
    var mapGo = Instantiate(Resources.Load(_mapPath));
    _map = mapGo.GetComponent<Map>();
    _map.Setup();
    _enemySpawner = _map.GetEnemySpawner();

    var path = _map.GetPath();
    var townPos = path[path.Count - 1];

    _collector = Instantiate(Resources.Load<MudCollector>(_mudCollectorPath));
    _collector.transform.position = townPos;
    _collector.Setup(new MudCollector.SetupData
    { 
      TownPosition = townPos
    });

    StartGame();
  }

  private void StartGame()
  {
    StartWave();
  }

  private void StartWave()
  {
    int monsterCount = Mathf.RoundToInt(_defaultWaveCount + (_wave * _waveMultiplier));
    _enemySpawner.Spawn(monsterCount, 2f, () => 
    {
      DelayCallFunction(5, () => { StartWave(); });
    });
  }

  private void Update()
  {
    if(Input.GetKeyDown(KeyCode.I)) 
    {
      _player.GetInventory().ToggleUI();
    }
  }

  private void DelayCallFunction(float second, Action callback)
  {
    StartCoroutine(DoDelayCallfunction(second, callback));
  }

  private IEnumerator DoDelayCallfunction(float time, Action callback)
  {
    yield return new WaitForSeconds(time);
    callback?.Invoke();
  }
}
