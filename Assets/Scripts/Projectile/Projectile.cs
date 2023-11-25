using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Projectile : MonoBehaviour
{
  private Action _onHit;

  private int _damage;
  private float _flySpeed;
  private Enemy _target;

  private readonly float ROTATE_SPEED = 100f;

  public struct ProjectileData
  {
    public int damage;
    public float flySpeed;
    public Enemy target;
  }

  public void Setup(ProjectileData data)
  {
    _damage = data.damage;
    _flySpeed = data.flySpeed;
    _target = data.target;

    _onHit += () => { if (_target) _target.TakeDamage(_damage); };
  }

  public void FixedUpdate()
  {
    Move();
  }

  private void Move()
  {
    if (!_target)
    {
      Destroy(gameObject);
      return;
    }

    var pos = Vector3.MoveTowards(transform.position, _target.transform.position, _flySpeed * Time.fixedDeltaTime);
    transform.position = pos;

    RotateTowardsTarget();

    if(Mathf.Approximately(Vector3.Distance(transform.position, _target.transform.position), 0))
    {
      _onHit?.Invoke();
      Destroy(gameObject);
    }
  }

  void RotateTowardsTarget()
  {
    Vector2 direction = (_target.transform.position - transform.position).normalized;
    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, angle - 90), ROTATE_SPEED * Time.deltaTime);
  }
}
