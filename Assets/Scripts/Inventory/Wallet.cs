using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallet
{
  public enum CurrencyType
  {
    Gold, Meat
  }

  public interface CurrencyData
  {
    public CurrencyType Type { get; }
    public int amount { get; }
  }

  public Dictionary<CurrencyType, int> _currencies = new Dictionary<CurrencyType, int>();

  public Wallet(List<CurrencyData> initialCurrencies)
  {
    foreach (var curr in initialCurrencies)
    {
      AddCurrency(curr);
    }
  }

  public void AddCurrency(CurrencyData data)
  {
    if(_currencies.ContainsKey(data.Type))
    {
      _currencies[data.Type] += data.amount;
    }
    else
    {
      _currencies.Add(data.Type, data.amount);
    }
  }
}
