using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
  private int _amount = 0;
  private string _name;
  private string _id = "";

  public Item(string id, string name, int amount)
  {
    _id = id;
    _name = name;
    _amount = amount;
  }

  public int UpdateAmount(int amount)
  {
    return _amount + amount;
  }
}
