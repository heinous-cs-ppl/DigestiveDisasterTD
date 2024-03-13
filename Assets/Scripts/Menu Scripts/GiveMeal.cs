using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GiveMeal : MonoBehaviour
{
    private Image foodImage;
    private float flashDuration = 0.5f;
    private int numberOfFlashes = 2;
    private Coroutine flashCoroutine;

    void Start()
    {
        foodImage = GameObject.Find("Purify Icon").GetComponent<Image>();

    }
    public void UsePurifiedMeal()
    {
        // get the currently selected student
        GameObject selected = StudentManager.selected;

        // if the player has meals (subtracts a meal if they have one)
        if (PurifyManager.UseMeal())
        {
            // update the counter on the UI
            UIManager.UpdateMealCount();
            // give buffs to the selected student
            StudentInfo selectedInfo = selected.GetComponent<StudentInfo>();
            selectedInfo.Feed();
            UIManager.UpdateSelectedBars(selectedInfo);
            // "reselect" the selected student to redraw the range circle (I'm lazy)
            StudentManager.Select(StudentManager.selected);
        }
        else
        {

            if (flashCoroutine != null)
            {
                StopCoroutine(flashCoroutine);
                foodImage.color = Color.white;
            }
            // flash the purified meals red
            flashCoroutine = StartCoroutine(FlashSprite());
        }
    }

    IEnumerator FlashSprite()
    {
        for (int i = 0; i < numberOfFlashes; i++)
        {
            // gradually change the sprite color to red
            float elapsedTime = 0f;
            while (elapsedTime < flashDuration)
            {
                foodImage.color = Color.Lerp(Color.white, Color.red, elapsedTime / flashDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // gradually change the sprite color back to its original color
            elapsedTime = 0f;
            while (elapsedTime < flashDuration)
            {
                foodImage.color = Color.Lerp(Color.red, Color.white, elapsedTime / flashDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
    }
}
