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
}
