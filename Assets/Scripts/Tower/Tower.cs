using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
  private Stat _attackDamage;
  private float _attackInterval = 1.2f;
  private bool _canAttack = true;
  private Action _onAttack;

  [SerializeField]
  private UIBar _attackCooldownBar = null;
  [SerializeField]
  private EnemyDetector _enemyDetector = null;
  private EnemyContainer _enemyContainer = null;

  public Enemy[] enemies = null;
  public Tower otherTower = null;

  private TowerConnector _towerConnector = null;

  private void SubscribeOnAttack(Action action)
  {
    _onAttack += action;
  }

  private void UnsubscribeOnAttack(Action action)
  {
    _onAttack -= action;
  }

  private void Cleanup()
  {
    UnsubscribeOnAttack(() => ShowAttackCooldownBar(_attackInterval));

    _onAttack = null;
  }

  #region Unity Functions
  private void Start()
  {
    _enemyContainer = new EnemyContainer(5);
    _towerConnector = new TowerConnector(this, 2);
    _attackDamage = new Stat(3);

    if (_attackCooldownBar) _attackCooldownBar.Setup(_attackInterval);
    SubscribeOnAttack(() => ShowAttackCooldownBar(_attackInterval));

    _attackCooldownBar.gameObject.SetActive(false);

    if (_enemyDetector) _enemyDetector.Setup(_enemyContainer);
  }

  private void Update()
  {
    if(Input.GetKeyDown(KeyCode.Space))
    {
      if (enemies != null)
      {
        foreach (var e in enemies)
        {
          _enemyContainer.AddEnemyToList(e);
        }
      }
    }
    else if(Input.GetKeyDown(KeyCode.C))
    {
      if(otherTower)
      {
        Connect(otherTower);
      }
    }

    Attack();
  }

  private void OnDestroy()
  {
    Cleanup();
  }
  #endregion

  private void Attack()
  {
    if (!_canAttack || _enemyContainer == null) return;

    var target = _enemyContainer.GetTargetEnemy();
    if (!target) return;

    target.TakeDamage(_attackDamage.GetValue());
    _canAttack = false;
    _onAttack?.Invoke();
  }

  private void ShowAttackCooldownBar(float second)
  {
    if (!_attackCooldownBar) return;
    StartCoroutine(DoShowAttackCooldownBar(second));
  }

  private IEnumerator DoShowAttackCooldownBar(float second)
  {
    _attackCooldownBar.gameObject.SetActive(true);
    float start = Time.time;
    while(Time.time < start + second)
    {
      _attackCooldownBar.UpdateBar(Time.time - start);
      yield return new WaitForEndOfFrame();
    }
    _attackCooldownBar.gameObject.SetActive(false);
    _canAttack = true;
  }

  public void Connect(Tower other)
  {
    if(_towerConnector.Connect(other))
    {
      TowerPair.PairTower(this, other);
    }
  }

  public void Disconnect(Tower other)
  {
    if(_towerConnector.Disconnect(other))
    {
      TowerPair.UnPairTower(this, other);
    }
  }
}
