using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StudentManager : MonoBehaviour
{
    public static bool placing = false;
    public static bool moving = false;

    // public static GameObject placementSelected;     // Only has a student if in placement mode
    public static GameObject plotOfSelected;

    public static GameObject selected;
    public LayerMask studentLayer;
    private GameObject map;
    private Bounds mapBounds;

    private static GameObject rangeCircle;
    private static SpriteRenderer circle;
    public Sprite circleSprite;

    public static void Place(GameObject student, Transform plot, bool aboveTable)
    {
        selected = Instantiate(student, plot.position, Quaternion.identity);
        plotOfSelected = plot.transform.gameObject;
        Select(selected);
        if (aboveTable) selected.GetComponent<SpriteRenderer>().sortingLayerName = "Students above tables";
        

        // show selection UI
        UIManager.ShowStudentSelectedUI();
        // hide hiring student UI
        UIManager.HideStudentHiringUI();

        // draw the student's range so it's clear which student is selected
        float range = selected.GetComponent<StudentInfo>().range;
        DrawRange(range);
    }

    public static void Select(GameObject student)
    {
        if (selected != null) {
            Deselect();
        }
        selected = student;

        RaycastHit2D plotHit = Physics2D.Raycast(student.transform.position, Vector2.zero, 1f, LevelManager.instance.plotLayer);
        plotOfSelected = plotHit.transform.gameObject;
        // show selection UI
        UIManager.ShowStudentSelectedUI();
        // hide hiring student UI
        UIManager.HideStudentHiringUI();

        // draw the student's range so it's clear which student is selected
        float range = selected.GetComponent<StudentInfo>().range;
        selected.GetComponent<SpriteRenderer>().material = selected.GetComponent<StudentInfo>().outline;
        DrawRange(range);
    }

    public static void Deselect()
    {
        if (selected != null) {
            selected.GetComponent<SpriteRenderer>().material = selected.GetComponent<StudentInfo>().noOutline;
        }
        selected = null;
        plotOfSelected = null;

        // there's a bug I can't be bothered to fix properly for now
        GameObject.Find("Purify Icon").GetComponent<Image>().color = Color.white;
        GameObject.Find("Money Icon").GetComponent<Image>().color = Color.white;

        // hide selection UI
        UIManager.HideStudentSelectedUI();
        rangeCircle.transform.localScale = Vector2.zero;
    }

    public static void DrawRange(float range)
    {
        rangeCircle.transform.localScale = new Vector2(range * 2, range * 2);
        rangeCircle.transform.position = selected.transform.position;
    }

    void Start()
    {
        map = GameObject.Find("Map");
        SpriteRenderer mapSprite = map.GetComponent<SpriteRenderer>();
        mapBounds = mapSprite.bounds;

        // set up the range circle for selected students
        rangeCircle = new GameObject("Range Circle");
        circle = rangeCircle.AddComponent<SpriteRenderer>();
        circle.sprite = circleSprite;
        Color IHATETHEAMERICANSPELLINGOFCOLOUR = new Color(0f, 0f, 0f, 0.5f);
        circle.color = IHATETHEAMERICANSPELLINGOFCOLOUR;
        rangeCircle.transform.localScale = Vector2.zero;
    }

    // check for clicks on the map, if a student is clicked, then select, otherwise deselect.
    void Update()
    {
        // check for click, make sure student placing is off, make sure no student is being moved
        if (Input.GetMouseButtonDown(0) && !(placing) && !(moving))
        {
            Debug.Log("clicked");
            // check if cursor is on map
            Vector2 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // round the cursor position to the middle of the tile
            cursorPosition.x = Mathf.Ceil(cursorPosition.x) - 0.5f;
            cursorPosition.y = Mathf.Ceil(cursorPosition.y) - 0.5f;

            if (mapBounds.Contains(cursorPosition))
            {
                Debug.Log("clicked on map");
                RaycastHit2D hit = Physics2D.Raycast(cursorPosition, Vector2.zero, 1f, studentLayer);
                if (hit)
                {
                    Debug.Log("clicked on student");
                    // clicked on a student, select the student
                    GameObject student = hit.collider.gameObject;
                    StudentManager.Select(student);
                }
                else
                {
                    Debug.Log("didn't click on student - deselect");
                    // didn't click on a student, deselect
                    StudentManager.Deselect();
                }
            }
        }
    }
}
