using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Data/Tower", fileName = "Tower Data")]
public class TowerData : ScriptableObject
{
  public AudioClip shootSound = null;
  public float attackInterval = 0.6f;
  public int attackDamage = 5;
  public ProjectileData projectileData = default;
}
