using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameController : MonoBehaviour
{
  private Map _map = default;
  private EnemySpawner _enemySpawner = default;
  private Player _player;

  private string _mapPath = "Templates/Map";

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

    StartGame();
  }

  private void StartGame()
  {
    _enemySpawner.Spawn(5, 2f);
  }

  private void Update()
  {
    if(Input.GetKeyDown(KeyCode.I)) 
    {
      _player.GetInventory().ToggleUI();
    }
  }
}
