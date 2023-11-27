using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour, ISelectableObject
{
  [SerializeField]
  private TowerData _towerData;

  private Stat _attackDamage;
  private bool _canAttack = true;
  private Action _onAttack;

  private bool _isMoving = false;

  [SerializeField]
  private UIBar _attackCooldownBar = null;
  [SerializeField]
  private EnemyDetector _enemyDetector = null;
  private TowerConnector _towerConnector = null;

  [SerializeField]
  private PlaceableDisplay _placeableDisplay = default;
  private bool _canPlace = true;

  [SerializeField]
  private SpriteRenderer _spriteRenderer = default;
  [SerializeField]
  private Projectile _bullet = default;
  [SerializeField]
  private Transform _firePoint = default;

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
    UnsubscribeOnAttack(() => ShowAttackCooldownBar());

    _onAttack = null;
  }

  #region Unity Functions
  private void Start()
  {
    _towerConnector = new TowerConnector(this, 2);
    _attackDamage = new Stat(3);

    if (_attackCooldownBar) _attackCooldownBar.Setup(_towerData.attackDamage);
    SubscribeOnAttack(() => ShowAttackCooldownBar());

    _attackCooldownBar.gameObject.SetActive(false);

    _enemyDetector = new EnemyDetector(this, 1.5f);
  }

  private void OnMouseDown()
  {
    var cursor = Cursor.GetInstance();
    var currentSelecting = cursor.GetCurrentSelecting();
    if (currentSelecting != null && currentSelecting.Equals(this))
    {
      cursor.DoSelectingAction();
    }
    else
    {
      cursor.SelectObject(this);
    }
  }

  private void Update()
  {
    if(Input.GetKeyDown(KeyCode.V))
    {
      UpdateSize(UnityEngine.Random.Range(1, 5));
    }

    MoveToMouse();
    Attack();
    UpdateCanPlace();
    _enemyDetector?.Search();
  }

  private void OnDestroy()
  {
    Cleanup();
  }
  #endregion

  private void Attack()
  {
    if (!_canAttack || _enemyDetector == null) return;

    var target = _enemyDetector.GetTargetEnemy();
    if (!target) return;

    _canAttack = false;
    _onAttack?.Invoke();
    SpawnBullet(target);
  }

  private void SpawnBullet(Enemy target)
  {
    if (!_bullet) return;
    var spawnPos = _firePoint ? _firePoint.position : transform.position;

    var bullet = Instantiate(_bullet, spawnPos, Quaternion.identity);
    bullet.Setup(new Projectile.ProjectileData 
    {
      target= target,
      damage = Mathf.RoundToInt(_attackDamage.GetValue()),
      flySpeed = 5f
    });
  }

  private void ShowAttackCooldownBar()
  {
    if (!_attackCooldownBar) return;
    StartCoroutine(DoShowAttackCooldownBar(_towerData.attackInterval));
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

  public void Select()
  {
    _isMoving = true;
    if (_spriteRenderer) _spriteRenderer.sortingOrder += 1;
    transform.SetAsLastSibling();
  }

  public void Deselect()
  {
    _isMoving = false;
    if (_spriteRenderer) _spriteRenderer.sortingOrder -= 1;
  }

  private void MoveToMouse()
  {
    if (!_isMoving) return;

    var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    var newPos = new Vector3(Mathf.Floor(mousePos.x), Mathf.Floor(mousePos.y), transform.position.z) + new Vector3(0.5f, 0.5f);
    transform.position = newPos;
  }

  public Tower Place()
  {
    if(_canPlace)
    {
      Deselect();
      return null;
    }

    return this;
  }

  private void UpdateCanPlace()
  {
    if (!_placeableDisplay) return;

    if (!_isMoving)
    {
      _placeableDisplay.SetDisplay(PlaceableDisplay.DisplayType.Hide);
      return;
    }

    _canPlace = IsCanPlace();

    if (_canPlace)
    {
      _placeableDisplay.SetDisplay(PlaceableDisplay.DisplayType.Placeable);
    }
    else
    {
      _placeableDisplay.SetDisplay(PlaceableDisplay.DisplayType.NonPlaceable);
    }
  }

  private bool IsCanPlace()
  {
    ContactFilter2D filter = new ContactFilter2D();
    Collider2D[] hits = new Collider2D[1];
    int hitCount = Physics2D.OverlapCollider(GetComponent<Collider2D>(), filter, hits);

    if(hitCount == 0) return true;
    foreach (var hit in hits)
    {
      if (hit.gameObject.GetComponent<Tower>()) return false;
    }

    return false;
  }

  public void UpdateSize(float size)
  {
    transform.localScale = new Vector3(size, size, 1);
  }

  private void OnDrawGizmos()
  {
    Gizmos.color = new Color32(255, 255, 0,70);
    if (_enemyDetector != null)
    {
      Gizmos.DrawSphere(transform.position, _enemyDetector.GetRadius());

      var target = _enemyDetector.GetTargetEnemy();
      if (target)
      {
        var distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance <= _enemyDetector.GetRadius())
          Gizmos.color = Color.green;
        else
          Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, target.transform.position);
      }
    }
  }
}
