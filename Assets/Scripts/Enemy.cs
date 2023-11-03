using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
  private int _maxHealth = 10;
  private int _currentHealth;

  [SerializeField]
  private HealthBar _healthBar;

  private Action<int> onHealthUpdated = null;
  public void SubscribeOnHealthUpdated(Action<int> action, bool singleUse = false)
  {
    if (!singleUse)
    {
      onHealthUpdated += action;
    }
    else
    {
      onHealthUpdated += (val) =>
      {
        action?.Invoke(val);
        UnsubscribeOnHealthUpdated(action);
      };
    }
  }
  public void UnsubscribeOnHealthUpdated(Action<int> action)
  {
    onHealthUpdated -= action;
  }

  #region Unity Functions
  private void Awake()
  {
    _currentHealth = _maxHealth;
  }

  private void Start()
  {
    if (_healthBar) _healthBar.Setup(_maxHealth);
    SubscribeOnHealthUpdated(UpdateHealthBar);
  }

  private void Update()
  {
    if(Input.GetKeyDown(KeyCode.Space))
    {
      Heal(1);
    }
    else if(Input.GetKeyDown(KeyCode.Backspace))
    {
      TakeDamage(1);
    }
  }

  private void OnDestroy()
  {
    Cleanup();
  }
  #endregion

  private void Cleanup()
  {
    UnsubscribeOnHealthUpdated(UpdateHealthBar);
  }

  private void UpdateHealthBar(int newValue)
  {
    if (_healthBar) _healthBar.UpdateBar(newValue);
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
    onHealthUpdated?.Invoke(_currentHealth);
  }
}
