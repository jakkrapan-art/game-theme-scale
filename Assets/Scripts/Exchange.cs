using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Exchange
{
  public static int ExchangeMud(int mudAmount)
  {
    int reward = Mathf.RoundToInt(mudAmount * 1.35f); 
    return reward;
  }
}
