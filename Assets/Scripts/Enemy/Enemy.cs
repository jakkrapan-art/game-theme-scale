using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyTag
{
  FirstQueue, LowestLifetime, LowestHealth, Normal
}

public class Enemy : MonoBehaviour
{
  private int _maxHealth = 10;
  private int _currentHealth;
  private EnemyTag _tag = EnemyTag.Normal;
  [SerializeField] private UIBar _healthBar;
  private int _queueOrder = -1;
  private float _lifeTime = 30;
  private float _spawnTime = 0;

  private Vector2 _moveTarget;
  private bool _isMoving;

  public EnemyData mockData;

  private List<Vector3Int> _movePath = new List<Vector3Int>();
  private int _currentTargetMoveIndex = 0;

  #region Getter
  public EnemyTag GetTag() => _tag;
  public int GetQueueOrder() => _queueOrder;
  public int GetCurrentHealth() => _currentHealth;
  public float GetLifeTime() => _lifeTime;
  #endregion

  #region Setter
  public void SetTag(EnemyTag tag) => _tag = tag;
  public void SetQueueOrder(int order) => _queueOrder = order;
  #endregion

  #region HealthUpdateAction
  private Action<int> _onHealthUpdated = null;
  public void SubscribeOnHealthUpdated(Action<int> action, bool singleUse = false)
  {
    if (!singleUse)
    {
      _onHealthUpdated += action;
    }
    else
    {
      _onHealthUpdated += (val) =>
      {
        action?.Invoke(val);
        UnsubscribeOnHealthUpdated(action);
      };
    }
  }
  public void UnsubscribeOnHealthUpdated(Action<int> action)
  {
    _onHealthUpdated -= action;
  }
  #endregion
  #region OnDieAction
  private Action _onDie = null;
  public void SubscribeOnDie(Action action)
  {
    _onDie += action;
  }

  public void UnsubscribeOnDie(Action action)
  {
    _onDie -= action;
  }
  #endregion
  #region OnDestroyAction
  private Action _onDestroy = null;
  public void SubscribeOnDestroy(Action action)
  {
    _onDestroy += action;
  }
  public void UnsubscribeOnDestroy(Action action)
  {
    _onDestroy -= action;
  }
  #endregion
  #region OnMoveMoveFinishAction
  private Action _onMoveFinish = null;
  public void SubscribeOnMoveFinish(Action action)
  {
    _onMoveFinish += action;
  }
  public void UnsubscribeOnMoveFinish(Action action)
  {
    _onMoveFinish -= action;
  }
  #endregion

  #region Unity Functions
  private void Start()
  {
    if(mockData) Setup(mockData);
  }

  private void Update()
  {
    CheckLifetime();
  }

  private void FixedUpdate()
  {
    Move();
  }

  private void OnDestroy()
  {
    Cleanup();
  }
  #endregion

  private void Setup(EnemyData data)
  {
    _maxHealth = data.maxHealth;
    _currentHealth = _maxHealth;
    _lifeTime = data.lifeTime;
    _spawnTime = Time.time;

    if (_healthBar) _healthBar.Setup(_maxHealth);
    SubscribeOnHealthUpdated(UpdateHealthBar);

    SubscribeOnMoveFinish(() => 
    {
      if(_currentTargetMoveIndex >= _movePath.Count - 1)
      {
        _isMoving = false;
        Destroy(gameObject);
        return;
      }

      ++_currentTargetMoveIndex;
      SetMoveTarget(GridHelper.CellToWorld(_movePath[_currentTargetMoveIndex]));
    });
  }

  private void Cleanup()
  {
    UnsubscribeOnHealthUpdated(UpdateHealthBar);
  }

  private void UpdateHealthBar(int newValue)
  {
    if (_healthBar)
    {
      _healthBar.gameObject.SetActive(true);
      _healthBar.UpdateBar(newValue);
    }
  }

  public void TakeDamage(int amount)
  {
    UpdateHealth(-amount);
  }

  public void Heal(int amount)
  {
    UpdateHealth(amount);
  }

  private void UpdateHealth(int updateAmount)
  {
    _currentHealth = Math.Clamp(_currentHealth + updateAmount, 0, _maxHealth);
    _onHealthUpdated?.Invoke(_currentHealth);
    if (_currentHealth <= 0) Die();
  }

  private void Die()
  {
    _onDie?.Invoke();
    Destroy();
  }

  private void CheckLifetime()
  {
    if (Time.time >= _spawnTime + _lifeTime) Destroy();
  }

  private void Destroy()
  {
    _onDestroy?.Invoke();
    Destroy(gameObject);
  }

  public void SetMovePath(List<Vector3Int> path)
  {
    _movePath = path;
    if (_movePath.Count <= 0) return;
    SetMoveTarget(GridHelper.CellToWorld(_movePath[0]));
  }

  public void SetMoveTarget(Vector2 targetPosition)
  {
    _moveTarget = targetPosition + new Vector2(transform.localScale.x/2, transform.localScale.y/2);
    _isMoving = true;
  }

  public void Move()
  {
    if (!_isMoving) return;

    transform.position = Vector2.MoveTowards(transform.position, _moveTarget, 2 * Time.fixedDeltaTime);

    if(Mathf.Approximately(Vector2.Distance(transform.position, _moveTarget), 0))
    {
      _onMoveFinish?.Invoke();
    }
  }
}
