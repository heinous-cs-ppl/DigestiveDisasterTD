using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StudentManager : MonoBehaviour
{
    public static bool placing = false;
    public static bool moving = false;
    public static bool mouseDragging = false;
    public static Plot draggingOver = null;

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
        selected = Instantiate(
            student,
            new Vector2(
                plot.position.x + student.GetComponent<StudentInfo>().offsetX,
                plot.position.y
            ),
            Quaternion.identity
        );
        plotOfSelected = plot.transform.gameObject;
        plot.transform.gameObject.GetComponent<Plot>().student = selected;
        Select(selected);
        if (aboveTable)
        {
            selected.GetComponent<SpriteRenderer>().sortingLayerName = "Students above tables";
            SpriteRenderer[] chymous = selected.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer chyme in chymous)
            {
                chyme.sortingLayerName = "Students above tables";
            }
        }
        // hide hiring student UI
        UIManager.instance.HideStudentHiringUI();
        // show selection UI
        UIManager.instance.ShowStudentSelectedUI();

        // draw the student's range so it's clear which student is selected
        float range = selected.GetComponent<StudentInfo>().range;
        DrawRange(range);
    }

    public static void Select(GameObject student)
    {
        if (selected != null)
        {
            Deselect();
        }
        selected = student;

        RaycastHit2D plotHit = Physics2D.Raycast(
            student.transform.position,
            Vector2.zero,
            1f,
            LevelManager.instance.plotLayer
        );
        plotOfSelected = plotHit.transform.gameObject;
        // show selection UI
        UIManager.instance.ShowStudentSelectedUI();
        // hide hiring student UI
        UIManager.instance.HideStudentHiringUI();

        // draw the student's range so it's clear which student is selected
        float range = selected.GetComponent<StudentInfo>().range;
        Material outline = selected.GetComponent<StudentInfo>().outline;
        selected.GetComponent<SpriteRenderer>().material = outline;
        SpriteRenderer[] chymous = selected.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer chyme in chymous)
        {
            chyme.material = outline;
        }
        DrawRange(range);
    }

    public static void Deselect()
    {
        if (selected != null)
        {
            Material noOutline = selected.GetComponent<StudentInfo>().noOutline;
            selected.GetComponent<SpriteRenderer>().material = noOutline;
            SpriteRenderer[] chymous = selected.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer chyme in chymous)
            {
                chyme.material = noOutline;
            }
        }

        selected = null;
        plotOfSelected = null;

        // there's a bug I can't be bothered to fix properly for now
        GameObject.Find("Purify Icon").GetComponent<Image>().color = Color.white;
        GameObject.Find("Money Icon").GetComponent<Image>().color = Color.white;

        // hide selection UI
        UIManager.instance.HideStudentSelectedUI();
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
                RaycastHit2D hit = Physics2D.Raycast(
                    cursorPosition,
                    Vector2.zero,
                    1f,
                    studentLayer
                );
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

        // if right click on a student, give them a purified meal
        if (Input.GetMouseButtonDown(1))
        {
            // check if cursor is on map
            Vector2 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // round the cursor position to the middle of the tile
            cursorPosition.x = Mathf.Ceil(cursorPosition.x) - 0.5f;
            cursorPosition.y = Mathf.Ceil(cursorPosition.y) - 0.5f;

            Debug.Log("right click");
            RaycastHit2D hit = Physics2D.Raycast(cursorPosition, Vector2.zero, 1f, studentLayer);
            if (hit)
            {
                Debug.Log("right clicked on student");
                // clicked on a student, select the student
                GameObject student = hit.collider.gameObject;

                // start a coroutine to check when the mouse is released if the student is a commerce student
                if (student.GetComponent<StudentInfo>().commerce)
                {
                    StartCoroutine(FeedContinuously(student));
                    Debug.Log("feeding a commerce student");
                }

                // feed the student
                FeedStudent(student);
            }
        }

        // if escape is pressed, deselect the student
        if (Input.GetKeyDown(KeyCode.Escape) && !placing && !moving)
        {
            Deselect();
            Debug.Log("pressed esc, deselect");
        }
    }

    IEnumerator FeedContinuously(GameObject student)
    {
        // wait a second before continuous feeding
        yield return new WaitForSeconds(1f);
        // feed student every 0.1 seconds if right click not released
        while (Input.GetMouseButton(1))
        {
            FeedStudent(student);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void FeedStudent(GameObject student)
    {
        if (PurifyManager.instance.UseMeal())
        {
            // update the counter on the UI
            UIManager.instance.UpdateMealCount();
            // give buffs to the selected student
            StudentInfo clicked = student.GetComponent<StudentInfo>();
            clicked.Feed();
            if (StudentManager.selected == student)
            {
                UIManager.instance.UpdateSelectedBars(clicked);
                // "reselect" the selected student to redraw the range circle (I'm lazy)
                StudentManager.Select(StudentManager.selected);
            }
        }
    }
}

