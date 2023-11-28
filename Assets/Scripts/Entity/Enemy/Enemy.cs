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
  private Stat _maxHealth;
  private int _currentHealth;
  private Stat _moveSpeed;
  private EnemyTag _tag = EnemyTag.Normal;
  [SerializeField] private UIBar _healthBar;
  private int _queueOrder = -1;

  private Vector2 _moveTarget;
  private bool _isMoving;
  private bool _updatingSclae = false;

  [SerializeField]
  private EnemyData data;

  private List<Vector3Int> _movePath = new List<Vector3Int>();
  private int _currentTargetMoveIndex = 0;

  #region Getter
  public EnemyTag GetTag() => _tag;
  public int GetQueueOrder() => _queueOrder;
  public int GetCurrentHealth() => _currentHealth;
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
  #region OnUpdateAction
  private Action _onUpdateAction = default;
  protected void SubscribeOnUpdateAction(Action action)
  {
    _onUpdateAction += action;
  }

  protected void UnsubscribeOnUpdateAction(Action action)
  {
    _onUpdateAction -= action;
  }
  #endregion

  #region Unity Functions
  protected virtual void Start()
  {
    if(data) Setup(data);
  }

  private void Update()
  {
    _onUpdateAction?.Invoke();
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

  protected virtual void Setup(EnemyData data)
  {
    _maxHealth = new Stat(data.maxHealth);
    _currentHealth = (int)_maxHealth.GetValue();

    _moveSpeed = new Stat(data.moveSpeed);
    SetScale(data.scale);

    if (_healthBar) _healthBar.Setup(_maxHealth.GetValue());

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

    SubscribeOnDie(() => 
    {
      EnemyFood.Create(transform.position, new EnemyFood.SetupData { foodValue = 5 });
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
      _healthBar.UpdateBar(newValue, false);
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
    _currentHealth = Math.Clamp(_currentHealth + updateAmount, 0, Mathf.RoundToInt(_maxHealth.GetValue()));
    _onHealthUpdated?.Invoke(_currentHealth);
    if (_currentHealth <= 0) Die();
  }

  private void Die()
  {
    _onDie?.Invoke();
    Destroy();
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
    _moveTarget = targetPosition + new Vector2(0.5f, 0.5f);
    _isMoving = true;
  }

  public void Move()
  {
    if (!_isMoving || _updatingSclae) return;

    transform.position = Vector2.MoveTowards(transform.position, _moveTarget, _moveSpeed.GetValue() * Time.fixedDeltaTime);
    if(Mathf.Approximately(Vector2.Distance(transform.position, _moveTarget), 0))
    {
      _onMoveFinish?.Invoke();
    }
  }

  protected void SetScale(float scale, bool instant = true)
  {
    if(!instant)
      StartCoroutine(DoUpdateScale(scale));
    else
      transform.localScale = new Vector3(scale, scale, 1);
  }

  private IEnumerator DoUpdateScale(float scale)
  {
    float time = 1.25f;
    float startTime = Time.time;
    float currentScale = transform.localScale.x;
    _updatingSclae = true;

    while(Time.time <= startTime + time)
    {
      currentScale = Mathf.Lerp(currentScale, scale, 0.14f);
      transform.localScale = new Vector3(currentScale, currentScale, 1);
      yield return new WaitForEndOfFrame();
    }

    transform.localScale = new Vector3(scale, scale, 1);
    _updatingSclae = false;
  }

  protected void UpdateMoveSpeedStat(float amount)
  {
    _moveSpeed.UpdateExtraValue(amount);
  }

  protected void UpdateHealthStat(float amount)
  {
    _maxHealth.UpdateExtraValue(amount);
    _currentHealth += Mathf.RoundToInt(amount);
    _healthBar.UpdateMaxValue(_maxHealth.GetValue());
    UpdateHealthBar(Mathf.RoundToInt(_currentHealth));
  }
}
