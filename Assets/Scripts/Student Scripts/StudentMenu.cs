using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  

public class StudentMenu : MonoBehaviour
{
    void OnMouseDown() {
        StudentManager.Select(gameObject);
    }
}
