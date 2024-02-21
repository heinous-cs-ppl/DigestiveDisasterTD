using UnityEngine;

public class StudentPlacement : MonoBehaviour
{
    public GameObject student;
    private bool canPlace = false;

    void Update()
    {
        // check for mouse click and if player can place the student
        if (canPlace && Input.GetMouseButtonDown(0))
        {
            // get cursor position
            Vector2 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // round the cursor position so the student always gets placed in the middle of the tile
            cursorPosition.x = Mathf.Ceil(cursorPosition.x) - 0.5f;
            cursorPosition.y = Mathf.Ceil(cursorPosition.y) - 0.5f;
            // create student at cursor position
            Instantiate(student, cursorPosition, Quaternion.identity);

            // disable placing once the student has been placed
            canPlace = false;
        }
    }

    // called when the button is clicked
    public void StartPlacementMode()
    {
        canPlace = true;
    }
}