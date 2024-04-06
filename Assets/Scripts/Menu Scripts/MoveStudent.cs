using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveStudent : MonoBehaviour
{
    public static MoveStudent instance;

    private Plot oldPlot;

    [HideInInspector]
    public GameObject studentPreview;
    private GameObject student;
    private Sprite studentSprite;
    public LayerMask studentLayer;

    private GameObject map;
    private Bounds mapBounds;
    public static int moveCost = 20;

    private float flashDuration = 0.5f;
    private int numberOfFlashes = 2;
    private Image moneyImage;

    public AudioClip placeSound;

    void Start()
    {
        instance = this;
        map = GameObject.Find("Map");
        SpriteRenderer mapSprite = map.GetComponent<SpriteRenderer>();
        mapBounds = mapSprite.bounds;
        moneyImage = GameObject.Find("Money Icon").GetComponent<Image>();
    }

    // Called when button is clicked
    public void SetMoving()
    {
        if (MoneyManager.instance.GetMoneyCount() >= moveCost || Spawner.instance.waveEnd)
        {
            student = StudentManager.selected;
            oldPlot = StudentManager.plotOfSelected.GetComponent<Plot>();
            studentSprite = student.GetComponentInChildren<SpriteRenderer>().sprite;

            StudentManager.moving = true;
        }
        else
        {
            StartCoroutine(FlashSprite());
        }
    }

    void Update()
    {
        if (StudentManager.moving)
        {
            // if escape is pressed, terminate moving
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                StudentManager.moving = false;
                if (studentPreview)
                    Destroy(studentPreview);
                return;
            }

            // get cursor position
            Vector2 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // round the cursor position to the middle of the tile
            cursorPosition.x = Mathf.Ceil(cursorPosition.x) - 0.5f;
            cursorPosition.y = Mathf.Ceil(cursorPosition.y) - 0.5f;

            // check if the cursor is on the map
            if (mapBounds.Contains(cursorPosition))
            {
                // create the student preview if it doesn't exist
                if (!(studentPreview))
                {
                    // create a new GameObject with a sprite renderer
                    studentPreview = new GameObject("StudentPreview");
                    SpriteRenderer previewSprite = studentPreview.AddComponent<SpriteRenderer>();

                    // set the sprite to the student sprite
                    previewSprite.sprite = studentSprite;

                    // set the layer of the sprite to Student Preview so it shows above other students
                    previewSprite.sortingLayerName = "Student Preview";

                    // set the opacity to 50%
                    Color preview = studentPreview.GetComponent<SpriteRenderer>().color;
                    preview.a = 0.5f; // Set alpha value to 50% opacity
                    studentPreview.GetComponent<SpriteRenderer>().color = preview;
                }
                // set the position of the student preview to the cursor's position
                studentPreview.transform.position = cursorPosition;
            }
            else
            {
                // if the player clicks somewhere not on the map
                if (Input.GetMouseButtonDown(0))
                {
                    // disable moving
                    StudentManager.moving = false;
                    // destroy the preview if it exists
                    if (studentPreview)
                        Destroy(studentPreview);
                }
            }
        }
    }

    // Called from Plot.cs by the plot to move onto
    public void Place(Transform newPlot)
    {
        // Let Update() in StudentManager.cs run first, or student will not be selected after move.
        StartCoroutine(DelayAndFinishMove(newPlot));

        // play sound effect for placing student
        PlaySound.instance.SFX(placeSound);
    }

    IEnumerator FlashSprite()
    {
        for (int i = 0; i < numberOfFlashes; i++)
        {
            // gradually change the sprite color to red
            float elapsedTime = 0f;
            while (elapsedTime < flashDuration)
            {
                moneyImage.color = Color.Lerp(Color.white, Color.red, elapsedTime / flashDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // gradually change the sprite color back to its original color
            elapsedTime = 0f;
            while (elapsedTime < flashDuration)
            {
                moneyImage.color = Color.Lerp(Color.red, Color.white, elapsedTime / flashDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
    }

    private IEnumerator DelayAndFinishMove(Transform newPlot)
    {
        yield return new WaitForSeconds(0.05f);

        // set the position of the student to the position
        if (student.GetComponent<StudentInfo>().turret is MachineStudent) // Handle Engineer student
        {
            Plot leftPlot = null;
            if (newPlot.gameObject.GetComponent<Plot>().plotOnLeft)
            {
                leftPlot = newPlot
                    .gameObject.GetComponent<Plot>()
                    .plotOnLeft.gameObject.GetComponent<Plot>();
            }
            Plot oldLeftPlot = null;
            if (oldPlot.plotOnLeft)
            {
                oldLeftPlot = oldPlot.plotOnLeft.gameObject.GetComponent<Plot>();
            }
            if (leftPlot)
            {
                // Handle moving right one tile
                if (leftPlot == oldPlot)
                {
                    Debug.Log("right one tile");
                    student.transform.position = newPlot.position;
                    leftPlot.student = LevelManager.instance.machineRepresentation;
                    if (oldLeftPlot)
                    {
                        oldLeftPlot.student = null;
                    }
                }
                else if (oldLeftPlot && oldLeftPlot.transform == newPlot)
                {
                    Debug.Log("left one tile");
                    student.transform.position = newPlot.position;
                    leftPlot.student = LevelManager.instance.machineRepresentation;
                    oldPlot.student = null;
                }
                else
                {
                    Debug.Log("eee");
                    student.transform.position = newPlot.position;
                    leftPlot.student = LevelManager.instance.machineRepresentation;
                    if (oldLeftPlot)
                    {
                        oldLeftPlot.student = null;
                    }
                    oldPlot.student = null;
                }
            }
            else
            {
                student.transform.position = newPlot.position;
                if (oldLeftPlot)
                {
                    oldLeftPlot.student = null;
                }
                oldPlot.student = null;
            }
        }
        else
        {
            student.transform.position = newPlot.position;
            oldPlot.student = null;
        }

        if (newPlot.GetComponent<Plot>().aboveTable)
        {
            student.GetComponent<SpriteRenderer>().sortingLayerName = "Students above tables";
            SpriteRenderer[] chymous = student.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer chyme in chymous)
            {
                chyme.sortingLayerName = "Students above tables";
            }
        }
        else
        {
            student.GetComponent<SpriteRenderer>().sortingLayerName = "Students behind tables";
            SpriteRenderer[] chymous = student.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer chyme in chymous)
            {
                chyme.sortingLayerName = "Students behind tables";
            }
        }

        // add cost for moving student here unless between rounds
        if (!Spawner.instance.waveEnd)
        {
            MoneyManager.instance.TakeMoney(moveCost);
            UIManager.instance.UpdateMoney();
        }

        // destroy the preview
        Destroy(studentPreview);

        // "reselect" the selected student to redraw the range circle (I'm lazy)
        StudentManager.Select(StudentManager.selected);

        // disable moving once the student has been placed
        StudentManager.moving = false;
    }
}

