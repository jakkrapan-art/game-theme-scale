using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
  private float _attackInterval = 1.2f;
  private bool _canAttack = true;
  private Action _onAttack;

  [SerializeField]
  private UIBar _attackCooldownBar = null;
  private EnemyContainer _enemyContainer = null;

  public Enemy _enemy = null;

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

    if (_attackCooldownBar) _attackCooldownBar.Setup(_attackInterval);
    SubscribeOnAttack(() => ShowAttackCooldownBar(_attackInterval));

    _attackCooldownBar.gameObject.SetActive(false);
  }

  private void Update()
  {
    if(Input.GetKeyDown(KeyCode.Space))
    {
      if (_enemy) _enemyContainer.AddEnemyToList(_enemy);
    }
    else if(Input.GetKeyDown(KeyCode.LeftShift)) 
    {
      if (_enemy) _enemyContainer.RemoveEnemyFromList(_enemy);
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

    target.TakeDamage(1);
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
}
