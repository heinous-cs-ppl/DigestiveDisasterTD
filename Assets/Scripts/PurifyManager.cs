using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
public class PurifyManager
{
    private static int purifiedMealCount = 0;

    public static void GainMeal() {
        purifiedMealCount++;
    }

    public static bool UseMeal() {
        if (purifiedMealCount > 0) {
            // if the player has purified meals, allow them to use a meal
            purifiedMealCount--;
            return true;
        }
        // if the player doesn't have purified meals, don't allow them to use a meal
        return false;    
    }

    public static string GetStringMealCount() {
        return purifiedMealCount.ToString();
    }
}
