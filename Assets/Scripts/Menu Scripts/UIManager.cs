using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static GameObject studentSelectUI;
    private static TextMeshProUGUI purifyCount;
    private static TextMeshProUGUI moneyCount;

    private static GameObject studentHireUI;
    private static Slider studentHireUIStudentHP;
    private static Slider studentHireUIStudentRange;
    private static Slider studentHireUIStudentDamage;
    private static Slider studentHireUIStudentBPS;
    private static StudentInfo studentHireUIStudentInfo;
    private static TextMeshProUGUI studentHireUIDescription;

    private static TextMeshProUGUI gameOver;

    void Start()
    {
        // initialize the purify counter in the UI to 0
        purifyCount = GameObject.Find("Purify Count").GetComponent<TextMeshProUGUI>();
        UIManager.UpdateMealCount();

        // initialize the money counter in the UI to the starting amount of money
        moneyCount = GameObject.Find("Money Count").GetComponent<TextMeshProUGUI>();
        UIManager.UpdateMoney();

        // hide the student selected UI
        studentSelectUI = GameObject.Find("Student Menu BG");
        UIManager.HideStudentSelectedUI();

        // initialize and hide the student hiring UI and related fields
        studentHireUI = GameObject.Find("Student Hire BG");

        studentHireUIStudentHP = GameObject.Find("Health bar").GetComponent<Slider>();
        studentHireUIStudentDamage = GameObject.Find("Damage bar").GetComponent<Slider>();
        studentHireUIStudentRange = GameObject.Find("Range bar").GetComponent<Slider>();
        studentHireUIStudentBPS = GameObject.Find("BPS bar").GetComponent<Slider>();

        studentHireUIDescription = GameObject.Find("Description").GetComponent<TextMeshProUGUI>();

        UIManager.HideStudentHiringUI();

        gameOver = GameObject.Find("Game Over").GetComponent<TextMeshProUGUI>();
        UIManager.HideGameOverUI();
    }

    public static void UpdateMealCount()
    {
        purifyCount.text = PurifyManager.GetStringMealCount();
    }

    public static void UpdateMoney()
    {
        moneyCount.text = "$" + MoneyManager.GetStringMoneyCount();
    }

    public static void ShowStudentSelectedUI()
    {
        studentSelectUI.SetActive(true);
    }

    public static void HideStudentSelectedUI()
    {
        studentSelectUI.SetActive(false);
    }

    public static void HideStudentHiringUI()
    {
        studentHireUI.SetActive(false);
    }

    public static void ShowStudentHiringUI(GameObject student)
    {
        studentHireUI.SetActive(true);
        // get info from the student
        studentHireUIStudentInfo = student.GetComponent<StudentInfo>();

        studentHireUIStudentHP.value = studentHireUIStudentInfo.maxHp;

        studentHireUIStudentDamage.value = studentHireUIStudentInfo.damage;

        studentHireUIStudentRange.value = studentHireUIStudentInfo.range;

        studentHireUIStudentBPS.value = studentHireUIStudentInfo.bps;

        studentHireUIDescription.text = studentHireUIStudentInfo.description;
    }

    public static void ShowGameOverUI()
    {
        gameOver.text = "Game Over!";
    }

    public static void HideGameOverUI()
    {
        gameOver.text = "";
    }
}
