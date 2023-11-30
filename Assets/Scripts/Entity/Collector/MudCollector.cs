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
  [SerializeField]
  private Animator _anim;
  [SerializeField]
  private Transform _characterModel = null;
  private string _currentAnim = "";

  private Vector3 _townPos = new Vector3(5, 5);
  private bool _moveToTown = false;
  private bool _isFacingRight = false;

  #region Stats
  private Status _moveSpeed;
  private Status _collectRange;
  private Status _collectTime;
  private Status _mudContainerSize;
  private int _currentMudCount = 0;
  #endregion

  private Coroutine _showCollectBarCoroutine = null;

  #region Getter/Setter
  public EnemyFood GetTarget() => _target;
  public void SetTarget(EnemyFood target)
  {
    _target = target;
  }

  public float GetCollectRange() => _collectRange.GetValue();
  public float GetCollectTime() => _collectTime.GetValue();
  public int GetMudCount() => _currentMudCount;
  public bool IsMudFull() => _currentMudCount == Mathf.RoundToInt(_mudContainerSize.GetValue());
  public bool IsMoveToTown() => _moveToTown;
  public Vector3 GetTownPosition() => _townPos;
  #endregion

  #region Unity Functions
  private void Awake()
  {
    _stateMachine = new MudCollectorStateMachine(this);

    _moveSpeed = new Status(1.5f);
    _collectRange = new Status(0.8f);
    _collectTime = new Status(2.5f);
    _mudContainerSize = new Status(5);
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

  public struct SetupData
  {
    public Vector3 TownPosition;
  }

  public void Setup(SetupData setupData)
  {
    _townPos = setupData.TownPosition;
  }

  public void MoveTo(Vector3 pos)
  {
    float direction = pos.x - transform.position.x;
    if((direction < 0 && _isFacingRight) || (direction > 0 && !_isFacingRight)) 
    {
      Flip();
    }

    transform.position = Vector3.MoveTowards(transform.position, pos, _moveSpeed.GetValue() * Time.fixedDeltaTime);
  }

  public void MoveToTown()
  {
    MoveTo(_townPos);
  }

  private void Flip()
  {
    if (!_characterModel) return;
    _characterModel.localScale = new Vector3(-_characterModel.localScale.x, _characterModel.localScale.y, _characterModel.localScale.z);
    _isFacingRight = !_isFacingRight;
  }

  public void Collect(EnemyFood mud)
  {
    if (!mud) return;

    int max = Mathf.RoundToInt(_mudContainerSize.GetValue());
    _currentMudCount = Mathf.Clamp(_currentMudCount + mud.Eat(), 0, max);
    if (_currentMudCount == max) _moveToTown = true;
    UpdateContainingMud();
  }

  public void ShowCollectBar(float sec, Action callback = null)
  {
    _showCollectBarCoroutine = StartCoroutine(DoShowCollectBar(sec, callback));
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

  public void HideCollectBar()
  {
    if (_collectBar)
    {
      if(_showCollectBarCoroutine != null)
      {
        StopCoroutine(_showCollectBarCoroutine);
        _showCollectBarCoroutine = null;
      }

      _collectBar.UpdateBar(0, true, false);
    }
  }

  public void StoreMudToTown()
  {
    _currentMudCount = 0;
    _moveToTown = false;
    UpdateContainingMud();
  }

  public void UpdateContainingMud()
  {
    if(_mudContainerBar) _mudContainerBar.UpdateBar(_currentMudCount, false);
  }

  public void SetAnimation(string name)
  {
    if (!_anim || string.IsNullOrEmpty(name)) return;

    if(!string.IsNullOrEmpty(_currentAnim)) _anim.SetBool(_currentAnim, false);
    _anim.SetBool(name, true);
    _currentAnim = name;
  }
}
