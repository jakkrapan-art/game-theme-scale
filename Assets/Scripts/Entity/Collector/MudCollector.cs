using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MudCollector : MonoBehaviour
{
  private EnemyFood _target;
  private MudCollectorStateMachine _stateMachine;

  [SerializeField]
  private UIBar _collectBar = default;
  [SerializeField]
  private UIBar _mudContainerBar = default;

  #region Stats
  private Status _moveSpeed;
  private Status _collectRange;
  private Status _collectTime;
  private Status _mudContainerSize;
  private int _currentMudCount = 0;
  #endregion

  #region Getter/Setter
  public EnemyFood GetTarget() => _target;
  public void SetTarget(EnemyFood target)
  {
    _target = target;
  }

  public float GetCollectRange() => _collectRange.GetValue();
  public float GetCollectTime() => _collectTime.GetValue();
  #endregion

  #region Unity Functions
  private void Awake()
  {
    _stateMachine = new MudCollectorStateMachine(this);

    _moveSpeed = new Status(0.5f);
    _collectRange = new Status(0.8f);
    _collectTime = new Status(2.5f);
    _mudContainerSize = new Status(100);
  }

  private void Start()
  {
    if(_collectBar) _collectBar.Setup(GetCollectTime());
    if(_mudContainerBar)
    {
      _mudContainerBar.Setup(Mathf.Round(_mudContainerSize.GetValue()));
      _mudContainerBar.UpdateBar(0, true, false);
    }
  }

  private void Update()
  {
    if (_stateMachine != null) _stateMachine.LogicUpdate();
  }

  private void FixedUpdate()
  {
    if (_stateMachine != null) _stateMachine.PhysicsUpdate();
  }
  #endregion

  public void MoveTo(Vector3 pos)
  {
    transform.position = Vector3.MoveTowards(transform.position, pos, _moveSpeed.GetValue() * Time.fixedDeltaTime);
  }

  public void Collect(EnemyFood mud)
  {
    if (!mud) return;
    _currentMudCount = Mathf.Clamp(_currentMudCount + mud.Eat(), 0, Mathf.RoundToInt(_mudContainerSize.GetValue()));
    UpdateContainingMud();
  }

  public void ShowCollectBar(float sec, Action callback = null)
  {
    StartCoroutine(DoShowCollectBar(sec, callback));
  }

  private IEnumerator DoShowCollectBar(float sec, Action callback)
  {
    if(!_collectBar)
    {
      callback?.Invoke();
      yield break;
    }

    _collectBar.gameObject.SetActive(true);

    float start = Time.time;
    do
    {
      float barVal = Time.time - start;
      _collectBar.UpdateBar(barVal);
      yield return new WaitForEndOfFrame();
    }
    while (Time.time < start + sec);

    _collectBar.gameObject.SetActive(false);
    callback?.Invoke();
  }

  public void UpdateContainingMud()
  {
    if(_mudContainerBar) _mudContainerBar.UpdateBar(_currentMudCount, false);
  }
}
