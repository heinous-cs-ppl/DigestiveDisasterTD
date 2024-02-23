using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    private static GameObject studentSelectUI;
    private static TextMeshProUGUI purifyCount;
    private static TextMeshProUGUI moneyCount;

    void Start() {
        // initialize the purify counter in the UI to 0
        purifyCount = GameObject.Find("Purify Count").GetComponent<TextMeshProUGUI>();
        UIManager.UpdateMealCount();

        // initialize the money counter in the UI to the starting amount of money
        moneyCount = GameObject.Find("Money Count").GetComponent<TextMeshProUGUI>();
        UIManager.UpdateMoney();

        // hide the student selected UI
        studentSelectUI = GameObject.Find("Student Menu BG");
        UIManager.HideStudentSelectedUI();
    }

    public static void UpdateMealCount() {
        purifyCount.text = PurifyManager.GetStringMealCount();
    }

    public static void UpdateMoney() {
        moneyCount.text = "$" + MoneyManager.GetStringMoneyCount();
    }

    public static void ShowStudentSelectedUI() {
        studentSelectUI.SetActive(true);
    }

    public static void HideStudentSelectedUI() {
        studentSelectUI.SetActive(false);
    }
}
