using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager
{
    private static int money = 250;

    public static void AddMoney(int amt)
    {
        money += amt;
    }

    public static void TakeMoney(int amt)
    {
        money -= amt;
    }

    public static string GetStringMoneyCount()
    {
        return money.ToString();
    }

    public static int GetMoneyCount()
    {
        return money;
    }

    public static void SetMoneyCount(int amt)
    {
        if (amt < 0)
        {
            Debug.LogError("Money cannot be negative");
            return;
        }
        money = amt;
    }
}
