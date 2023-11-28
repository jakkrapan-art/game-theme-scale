using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyContainer
{
  private int _size = 4;
  private List<Enemy> _enemyList = new List<Enemy>();

  private IComparer<Enemy> _enemyComparer = null;
  private Action<List<Enemy>> _onUpdated = null;
  private Action _onRemoveEnemy = null;

  public void SubscribeOnUpdated(Action<List<Enemy>> action)
  {
    _onUpdated += action;
  }
  public void UnsubscribeOnUpdated(Action<List<Enemy>> action)
  {
    _onUpdated -= action;
  }

  public void SubscribeOnRemoveEnemy(Action action)
  {
    _onRemoveEnemy += action;
  }

  public void UnsubscribeOnRemoveEnemy(Action action)
  {
    _onRemoveEnemy -= action;
  }

  #region Getter
  public List<Enemy> GetEnemyList() => _enemyList;
  #endregion

  #region Setter
  public void SetComparer(IComparer<Enemy> comparer) => _enemyComparer = comparer;
  #endregion

  public EnemyContainer(int size)
  {
    _size = size;
  }

  public bool AddEnemyToList(Enemy enemy)
  {
    if (_enemyList.Count >= _size || _enemyList.Contains(enemy)) return false;

    _enemyList.Add(enemy);
    _onUpdated?.Invoke(_enemyList);
    SortEnemy();

    enemy.SubscribeOnDestroy(()=>
    {
      RemoveEnemyFromList(enemy);
    });
    return true;
  }
  
  public bool RemoveEnemyFromList(Enemy enemy)
  {
    if (!_enemyList.Contains(enemy)) return false;

    _enemyList.Remove(enemy);
    _onUpdated?.Invoke(_enemyList);
    _onRemoveEnemy?.Invoke();
    return true;
  }

  public Enemy GetTargetEnemy()
  {
    if (_enemyList.Count <= 0) return null;
    return _enemyList[0];
  }

  private void SortEnemy()
  {
    if (_enemyComparer == null) return;
    _enemyList.Sort(_enemyComparer);
  }
}
