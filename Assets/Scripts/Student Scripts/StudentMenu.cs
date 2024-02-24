using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  

public class StudentMenu : MonoBehaviour
{
    void OnMouseDown() {
        // Find out what plot this student is on and select the student
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero, 1f, LevelManager.instance.plotLayer);
        GameObject plot = hit.transform.gameObject;
        Debug.Log("clicked" + plot.name);
        StudentManager.Select(gameObject, plot);
    }
}
