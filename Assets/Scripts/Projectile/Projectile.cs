using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
  private Action _onHit;

  private int _damage;
  private float _flySpeed;
  private Enemy _target;

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
  }
}
