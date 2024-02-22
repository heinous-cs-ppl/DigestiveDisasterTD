using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudentManager : MonoBehaviour
{
    public static GameObject selected;

    public static void Select(GameObject student) {
        selected = student;
        // show selection UI
        UIManager.ShowStudentSelectedUI();
    }

    public static void Deselect() {
        selected = null;
        // hide selection UI
        UIManager.HideStudentSelectedUI();
    }

    void Update()
    {
        // check if mouse is clicked
        if (Input.GetMouseButtonDown(0)) {
            // Create a ray from the camera to the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Perform raycast
            if (Physics.Raycast(ray, out hit))
            {
                // check if a student was clicked
                if (hit.collider.CompareTag("Student"))
                {
                    Debug.Log("Clicked on a turret: ");
                    // Perform turret-related actions
                }
                else
                {
                    // Clicked on something that is not a turret
                    Debug.Log("Clicked on something that is not a turret: ");
                    // Perform other actions
                }
            }
        }
    }
}
