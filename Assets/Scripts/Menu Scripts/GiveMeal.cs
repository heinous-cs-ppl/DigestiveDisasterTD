using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveMeal : MonoBehaviour
{
    public void UsePurifiedMeal() {
        // get the currently selected student
        GameObject selected = StudentManager.selected;

        // if the player has meals (subtracts a meal if they have one)
        if (PurifyManager.UseMeal()) {
            // update the counter on the UI
            UIManager.UpdateMealCount();
            // give buffs to the selected student
            selected.GetComponent<StudentInfo>().Feed();
        } else Debug.Log("No purified meals");
    }
}
