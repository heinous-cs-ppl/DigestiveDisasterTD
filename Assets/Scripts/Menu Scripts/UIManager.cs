using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    private static GameObject studentSelectUI;
    private static TextMeshProUGUI purifyCount;

    void Start() {
        // initialize the purify counter in the UI to 0
        purifyCount = GameObject.Find("Purify Count").GetComponent<TextMeshProUGUI>();
        UIManager.UpdateMealCount();

        // hide the student selected UI
        studentSelectUI = GameObject.Find("Student Menu BG");
        UIManager.HideStudentSelectedUI();
    }

    public static void UpdateMealCount() {
        purifyCount.text = PurifyManager.GetStringMealCount();
    }

    public static void ShowStudentSelectedUI() {
        studentSelectUI.SetActive(true);
    }

    public static void HideStudentSelectedUI() {
        studentSelectUI.SetActive(false);
    }
}
