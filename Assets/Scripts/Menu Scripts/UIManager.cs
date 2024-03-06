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
    private static TextMeshProUGUI studentHireUIStudentCost;
    private static TextMeshProUGUI studentHireUIStudentHP;
    private static TextMeshProUGUI studentHireUIStudentRange;
    private static TextMeshProUGUI studentHireUIStudentDamage;
    private static TextMeshProUGUI studentHireUIStudentBPS;
    private static Image studentHireUIStudentImage;
    private static StudentInfo studentHireUIStudentInfo;

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

        studentHireUIStudentImage = GameObject.Find("Student Hire UI Student Image").GetComponent<Image>();

        studentHireUIStudentCost = GameObject.Find("Student Hire UI Student Cost").GetComponent<TextMeshProUGUI>();
        studentHireUIStudentHP = GameObject.Find("Student Hire UI Student HP").GetComponent<TextMeshProUGUI>();
        studentHireUIStudentRange = GameObject.Find("Student Hire UI Student Range").GetComponent<TextMeshProUGUI>();
        studentHireUIStudentDamage = GameObject.Find("Student Hire UI Student Damage").GetComponent<TextMeshProUGUI>();
        studentHireUIStudentBPS = GameObject.Find("Student Hire UI Student BPS").GetComponent<TextMeshProUGUI>();

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

    public static void ShowStudentHiringUI(GameObject student, Sprite studentSprite)
    {
        studentHireUI.SetActive(true);
        studentHireUIStudentImage.sprite = studentSprite;

        // get info from the student
        studentHireUIStudentInfo = student.GetComponent<StudentInfo>();

        // display student's info in the UI
        studentHireUIStudentCost.text = "$" + studentHireUIStudentInfo.cost.ToString();
        studentHireUIStudentHP.text = "HP: " + studentHireUIStudentInfo.maxHp.ToString();
        studentHireUIStudentRange.text = "Range: " + studentHireUIStudentInfo.range.ToString();
        studentHireUIStudentDamage.text = "Damage: " + studentHireUIStudentInfo.damage.ToString();
        studentHireUIStudentBPS.text = "Attack Speed: " + studentHireUIStudentInfo.bps.ToString();
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
