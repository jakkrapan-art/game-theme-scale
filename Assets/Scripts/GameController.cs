using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameController : MonoBehaviour
{
  private Map _map = default;
  private EnemySpawner _enemySpawner = default;
  private Player _player;
  private MudCollector _collector;
  private BuildingSystem _buildingSystem = null;
  private UIHealth _healthUI = null;
  private int _health = 3;
  private GameObject _gameOver;

  private WaveController _waveController;
  private UIWaveController uiWave = default;

  private bool _end = false;

  private const string _mapPath = "Templates/Map";
  private const string _mudCollectorPath = "Templates/MudCollector";
  private const string _towerPath = "Templates/Tower";

  public Map GetMap() => _map;

  public void Initialize(List<Tower> startTowers)
  {
    _gameOver = GameObject.Find("UIGameOver");
    if (_gameOver) _gameOver.SetActive(false);
    _player = new Player(new Player.PlayerInitData
    {
      inventorySize = 15,
      walletSetupData = new Wallet.SetupData 
      { 
        currencyDatas = new List<Wallet.CurrencyData> 
        { 
          new Wallet.CurrencyData(Wallet.CurrencyType.Gold, 0),
          new Wallet.CurrencyData(Wallet.CurrencyType.Meat, 0),
        }
      },
      enterBuildMode = (tower) => 
      {
        if(_buildingSystem)
        {
          _buildingSystem.EnterBuildMode(_map, tower, () => 
          {
            Inventory inv = _player.GetInventory();
            if (inv == null) return;
            inv.ShowUI();
          });
        }
      }
    });

    foreach (var t in startTowers)
    {
      AddTowerToInventory(t);
    }

    GridHelper.Initialize();
    var mapGo = Instantiate(Resources.Load(_mapPath));
    _map = mapGo.GetComponent<Map>();
    _map.Setup((type) => 
    {
      switch(type)
      {
        case EnemySpawner.EnemyType.Minion:
          _health -= 1;
          break;
        case EnemySpawner.EnemyType.Boss:
          _health -= 2;
          break;
        default: return;
      }

      if (_healthUI) _healthUI.UpdateHearth(_health);
      if (_health <= 0) End();
    });
    _enemySpawner = _map.GetEnemySpawner();

    _waveController = new WaveController(new WaveController.SetupParam 
    {
      baseCount = 10,
      spawner = _enemySpawner,
      spawnInterval = 1.25f,
      onWaveEnd = (wave) => 
      {
        if(wave != 0 && wave % 2 == 0)
        {
          _waveController.SpawnBoss();
        }
        else
        {
          EndWave();
        }
      }
    });

    var buildingSysGo = new GameObject("BuildingSystem");
    _buildingSystem = buildingSysGo.AddComponent<BuildingSystem>();

    var townPos = _map.GetEndpoint() + new Vector3(0.5f, 1);

    _collector = Instantiate(Resources.Load<MudCollector>(_mudCollectorPath));
    _collector.transform.position = townPos;
    _collector.Setup(new MudCollector.SetupData
    { 
      TownPosition = townPos,
      ExchangeMudAction = (mud) => 
      {
        int receiveCash = Exchange.ExchangeMud(mud);
        _player.GetInventory().GetWallet().AddCurrency(new Wallet.CurrencyData(Wallet.CurrencyType.Gold, receiveCash));
      }
    });

    UIHelper.CreateHealthUI((ui) =>
    {
      ui.Setup(3);
      _healthUI = ui;
    });

    /*uiWave = FindObjectOfType<UIWaveController>();
    if (uiWave)
    {
      uiWave.Setup(() =>
      {
        var inv = _player.GetInventory();
        inv.HideUI();
        inv.SetActiveOpenButton(false);
        uiWave.Hide();

        if (_buildingSystem.IsInBuildMode())
        {
          var tower = _buildingSystem.ExitBuildMode();
          if (tower) inv.AddTower(tower);
        }

        StartWave();
      });

      uiWave.Show();
    }*/
  }

  private void StartWave()
  {
    _player.GetInventory().SetActiveOpenButton(false);
    _collector.SetAllowCollect(true);
    _waveController?.SpawnEnemy();
  }

  private void EndWave()
  {
    var loadedTowerObj = Resources.Load<Tower>(_towerPath);
    AddTowerToInventory(loadedTowerObj);

    var inv = _player.GetInventory();
    inv.SetActiveOpenButton(true);
    _waveController.ShowUI();
    _collector.SetAllowCollect(false);
    Invoke(nameof(StartWave), 5);
  }

  private void AddTowerToInventory(Tower t)
  {
    var instantiatedObj = Instantiate(t);
    if (_player.GetInventory().AddTower(instantiatedObj))
    {
      instantiatedObj.gameObject.SetActive(false);
    }
  }

  private void End()
  {
    _end = true;
    if (_gameOver) _gameOver.SetActive(true);
  }
}
