using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallet
{
  public enum CurrencyType
  {
    Gold, Meat
  }

  public struct SetupData
  {
    public List<CurrencyData> currencyDatas;
  }

  public struct CurrencyData
  {
    public CurrencyType Type { get; }
    public int Amount { get; }

    public CurrencyData(CurrencyType type, int amount)
    {
      Type = type;
      Amount = amount;
    }
  }

  public Dictionary<CurrencyType, int> _currencies = new Dictionary<CurrencyType, int>();

  public Wallet(SetupData setupData)
  {
    foreach (var curr in setupData.currencyDatas)
    {
      AddCurrency(curr);
    }
  }

  public int GetCurrency(CurrencyType type)
  {
    return _currencies[type];
  }

  public void AddCurrency(CurrencyData data)
  {
    if(_currencies.ContainsKey(data.Type))
    {
      _currencies[data.Type] += data.Amount;
    }
    else
    {
      _currencies.Add(data.Type, data.Amount);
    }
  }
}
