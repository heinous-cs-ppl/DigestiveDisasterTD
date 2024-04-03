using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager instance;
    private int money = 250;

    private void Awake()
    {
        instance = this;
    }

    public void AddMoney(int amt)
    {
        money += amt;
    }

    public void TakeMoney(int amt)
    {
        money -= amt;
    }

    public string GetStringMoneyCount()
    {
        return money.ToString();
    }

    public int GetMoneyCount()
    {
        return money;
    }

    public void SetMoneyCount(int amt)
    {
        if (amt < 0)
        {
            Debug.LogError("Money cannot be negative");
            return;
        }
        money = amt;
    }
}
