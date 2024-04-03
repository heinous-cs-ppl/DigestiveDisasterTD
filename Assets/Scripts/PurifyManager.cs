using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
public class PurifyManager : MonoBehaviour
{
    public static PurifyManager instance;
    private int purifiedMealCount = 0;

    private void Awake()
    {
        instance = this;
    }

    public void GainMeal()
    {
        purifiedMealCount++;
    }

    public bool UseMeal()
    {
        if (purifiedMealCount > 0)
        {
            // if the player has purified meals, allow them to use a meal
            purifiedMealCount--;
            return true;
        }
        // if the player doesn't have purified meals, don't allow them to use a meal
        return false;
    }

    public void SetMealCount(int amt)
    {
        if (amt < 0)
        {
            Debug.LogError("Meal count cannot be negative");
            return;
        }
        purifiedMealCount = amt;
    }

    public string GetStringMealCount()
    {
        return purifiedMealCount.ToString();
    }
}
