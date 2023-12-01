using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blaze : MonoBehaviour
{
  private int _damage;
  
  public void Setup(int damage)
  {
    _damage = damage;
  }

  private void OnDestroy()
  {
    
  }
}
