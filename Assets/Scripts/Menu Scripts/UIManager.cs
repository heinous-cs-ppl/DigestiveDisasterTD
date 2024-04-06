using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    private GameObject studentSelectUI;
    private Slider studentSelectUIStudentHP;
    private Slider studentSelectUIStudentRange;
    private Slider studentSelectUIStudentDamage;
    private Slider studentSelectUIStudentBPS;
    private TextMeshProUGUI purifyCount;
    private TextMeshProUGUI moneyCount;

    private GameObject studentHireUI;
    private Slider studentHireUIStudentHP;
    private Slider studentHireUIStudentRange;
    private Slider studentHireUIStudentDamage;
    private Slider studentHireUIStudentBPS;
    private StudentInfo studentHireUIStudentInfo;
    private TextMeshProUGUI studentHireUIDescription;

    private GameObject fireButton;
    private RectTransform moveButton;

    public GameObject gameOverUI;

    private TextMeshProUGUI studentsDeadText;
    private TextMeshProUGUI roundText;

    private TextMeshProUGUI moveStudentText;

    private GameObject path_0;
    private GameObject path_1;
    private GameObject path_2;
    private GameObject path_01;
    private GameObject path_12;
    private GameObject path_02;
    private GameObject path_012;
    private TextMeshProUGUI fireText;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // initialize the purify counter in the UI to 0
        purifyCount = GameObject.Find("Purify Count").GetComponent<TextMeshProUGUI>();
        UpdateMealCount();

        // initialize the money counter in the UI to the starting amount of money
        moneyCount = GameObject.Find("Money Count").GetComponent<TextMeshProUGUI>();
        UpdateMoney();

        // get the fire and move buttons
        fireButton = GameObject.Find("Fire Button");
        moveButton = GameObject.Find("Move Button").GetComponent<RectTransform>();

        studentSelectUIStudentHP = GameObject.Find("Health bar S").GetComponent<Slider>();
        studentSelectUIStudentDamage = GameObject.Find("Damage bar S").GetComponent<Slider>();
        studentSelectUIStudentRange = GameObject.Find("Range bar S").GetComponent<Slider>();
        studentSelectUIStudentBPS = GameObject.Find("BPS bar S").GetComponent<Slider>();

        moveStudentText = GameObject.Find("Move Text").GetComponent<TextMeshProUGUI>();
        fireText = GameObject.Find("Fire Text").GetComponent<TextMeshProUGUI>();

        // hide the student selected UI
        studentSelectUI = GameObject.Find("Student Menu BG");
        HideStudentSelectedUI();

        // initialize and hide the student hiring UI and related fields
        studentHireUI = GameObject.Find("Student Hire BG");

        studentHireUIStudentHP = GameObject.Find("Health bar").GetComponent<Slider>();
        studentHireUIStudentDamage = GameObject.Find("Damage bar").GetComponent<Slider>();
        studentHireUIStudentRange = GameObject.Find("Range bar").GetComponent<Slider>();
        studentHireUIStudentBPS = GameObject.Find("BPS bar").GetComponent<Slider>();

        // get the student description object
        studentHireUIDescription = GameObject.Find("Description").GetComponent<TextMeshProUGUI>();

        HideStudentHiringUI();

        // get students dead text
        studentsDeadText = GameObject.Find("Students Dead").GetComponent<TextMeshProUGUI>();
        UpdateStudentsDeadText();

        // get round text
        roundText = GameObject.Find("Round Count").GetComponent<TextMeshProUGUI>();
        UpdateRound();

        // get the paths
        path_0 = GameObject.Find("path_0");
        path_1 = GameObject.Find("path_1");
        path_2 = GameObject.Find("path_2");
        path_01 = GameObject.Find("path_01");
        path_12 = GameObject.Find("path_12");
        path_02 = GameObject.Find("path_02");
        path_012 = GameObject.Find("path_012");

        HidePath();
    }

    public void UpdateMealCount()
    {
        purifyCount.text = PurifyManager.instance.GetStringMealCount();
    }

    public void UpdateMoney()
    {
        moneyCount.text = "$" + MoneyManager.instance.GetStringMoneyCount();
    }

    public void ShowStudentSelectedUI()
    {
        studentSelectUI.SetActive(true);

        if (StudentManager.selected.GetComponent<StudentInfo>().cost == 0)
        {
            // vacuous students will be the only student that costs 0
            // if selected student is vacuous, remove the fire button and make the move button wider
            moveButton.sizeDelta = new Vector2(7, moveButton.sizeDelta.y);
            fireButton.SetActive(false);
        }
        else
        {
            moveButton.sizeDelta = new Vector2(3.375f, moveButton.sizeDelta.y);
            fireButton.SetActive(true);
            if (Spawner.instance.GetRound() != -1)
            {
                fireText.text =
                    "Fire (+$" + StudentManager.selected.GetComponent<StudentInfo>().cost / 2 + ")";
            }
            else
            {
                fireText.text =
                    "Fire (+$" + StudentManager.selected.GetComponent<StudentInfo>().cost + ")";
            }
        }

        StudentInfo selectedInfo = StudentManager.selected.GetComponent<StudentInfo>();
        UpdateSelectedBars(selectedInfo);
    }

    public void HideStudentSelectedUI()
    {
        studentSelectUI.SetActive(false);
    }

    public void HideStudentHiringUI()
    {
        studentHireUI.SetActive(false);
    }

    public void ShowStudentHiringUI(GameObject student)
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

    public void UpdateSelectedBars(StudentInfo info)
    {
        studentSelectUIStudentHP.maxValue = info.maxHp;
        studentSelectUIStudentHP.value = info.getHealth();
        studentSelectUIStudentDamage.value = info.damage;
        studentSelectUIStudentRange.value = info.range;
        studentSelectUIStudentBPS.value = info.bps;
    }

    public void UpdateStudentsDeadText()
    {
        studentsDeadText.text =
            "Students Dead: "
            + LevelManager.instance.studentsDead
            + "/"
            + LevelManager.instance.deathLimit;
    }

    public void UpdateRound()
    {
        roundText.text = "Round: " + Spawner.instance.GetRound();
    }

    public void UpdateMove(int val)
    {
        moveStudentText.text = "Move ($" + val + ")";
    }

    public void ShowPath(bool path0, bool path1, bool path2)
    {
        if (path0 && path1 && path2)
        {
            path_012.SetActive(true);
        }
        else if (path0 && path1)
        {
            path_01.SetActive(true);
        }
        else if (path1 && path2)
        {
            path_12.SetActive(true);
        }
        else if (path0 && path2)
        {
            path_02.SetActive(true);
        }
        else if (path0)
        {
            path_0.SetActive(true);
        }
        else if (path1)
        {
            path_1.SetActive(true);
        }
        else if (path2)
        {
            path_2.SetActive(true);
        }
    }

    public void HidePath()
    {
        path_0.SetActive(false);
        path_1.SetActive(false);
        path_2.SetActive(false);
        path_01.SetActive(false);
        path_12.SetActive(false);
        path_02.SetActive(false);
        path_012.SetActive(false);
    }
}
