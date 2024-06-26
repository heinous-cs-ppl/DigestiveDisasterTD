using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StudentPlacement : MonoBehaviour
{
    [SerializeField]
    private GameObject student;

    [SerializeField]
    private Sprite studentSprite;
    private GameObject studentPreview;
    private bool canPlace = false;
    private StudentInfo studentInfo;
    private float flashDuration = 0.5f;
    private int numberOfFlashes = 2;
    private Image moneyImage;
    public LayerMask studentLayer;
    private GameObject map;
    private GameObject ui;
    private Bounds mapBounds;
    public TextMeshProUGUI studentPrice;

    [HideInInspector]
    public static bool studentPlacedOnPlot = false;

    // stuff for range circle
    private static GameObject rangeCircle;
    private static SpriteRenderer circle;
    public Sprite circleSprite;

    public AudioClip placeSound;

    void Start()
    {
        // Bounds of the map
        map = GameObject.Find("Map");
        SpriteRenderer mapSprite = map.GetComponent<SpriteRenderer>();
        mapBounds = mapSprite.bounds;

        studentInfo = student.GetComponent<StudentInfo>();

        studentPrice.text = "$" + studentInfo.cost;

        moneyImage = GameObject.Find("Money Icon").GetComponent<Image>();
        rangeCircle = new GameObject("Placement Range Circle");
        circle = rangeCircle.AddComponent<SpriteRenderer>();
        circle.sprite = circleSprite;
        Color BREAKINGMUSCLEMEMORY = new Color(0f, 0f, 0f, 0.25f);
        circle.color = BREAKINGMUSCLEMEMORY;
        rangeCircle.transform.localScale = Vector2.zero;
    }

    // Just deal with placing student
    void Update()
    {
        // if escape is pressed, terminate placement mode
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine(StopPlacingStudent());
            return;
        }
        // get cursor position
        Vector2 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // round the cursor position to the middle of the tile
        cursorPosition.x = Mathf.Ceil(cursorPosition.x) - 0.5f;
        cursorPosition.y = Mathf.Ceil(cursorPosition.y) - 0.5f;

        // Check if the cursor is on the map and placement is allowed
        if (mapBounds.Contains(cursorPosition) && canPlace)
        {
            // create the student preview if it doesn't exist
            // create a circle for range too! :D :D :D :D :D :D :D
            if (!(studentPreview))
            {
                // create a new GameObject with a sprite renderer
                studentPreview = new GameObject("StudentPreview");
                SpriteRenderer previewSprite = studentPreview.AddComponent<SpriteRenderer>();

                // set the sprite to the student sprite
                previewSprite.sprite = studentSprite;
                // set the layer to be above tables
                previewSprite.sortingLayerName = "Students above tables";

                // set the opacity to 50%
                Color preview = studentPreview.GetComponent<SpriteRenderer>().color;
                preview.a = 0.5f; // Set alpha value to 50% opacity
                studentPreview.GetComponent<SpriteRenderer>().color = preview;

                // range circle stuff
                float range = studentInfo.range;
                rangeCircle.transform.localScale = new Vector2(range * 2, range * 2);
            }

            // set the position of the student preview to the cursor's position
            studentPreview.transform.position = new Vector2(
                cursorPosition.x + studentInfo.offsetX,
                cursorPosition.y
            );
            // set range circle position to cursor's position
            rangeCircle.transform.position = new Vector2(cursorPosition.x, cursorPosition.y);

            if (Input.GetMouseButtonDown(0))
            {
                // This placement termination happens unless a student is to be placed. Timed to wait for Plot to finish running.
                if (!studentPlacedOnPlot)
                {
                    StartCoroutine(StopPlacingStudent());
                }
                else
                { // Student was placed, now exit placement mode
                    PlaySound.instance.SFX(placeSound);
                    MoneyManager.instance.TakeMoney(student.GetComponent<StudentInfo>().cost);
                    UIManager.instance.UpdateMoney();
                    studentPlacedOnPlot = false;
                    canPlace = false;
                    // StudentManager.placing = false;
                    Destroy(studentPreview);
                    rangeCircle.transform.localScale = Vector2.zero;
                }
            }
        }
        else if (Input.GetMouseButtonDown(0) && canPlace)
        {
            Debug.Log("clicked off map - destroy preview");
            canPlace = false;
            StudentManager.placing = false;
            Destroy(studentPreview);
            rangeCircle.transform.localScale = Vector2.zero;
        }
    }

    public IEnumerator StopPlacingStudent()
    {
        yield return new WaitForSeconds(0.05f);

        Debug.Log("terminate student placing now");
        canPlace = false;
        StudentManager.placing = false;
        Destroy(studentPreview);
        rangeCircle.transform.localScale = Vector2.zero;
    }

    // called when the button is clicked
    public void StartPlacementMode()
    {
        if (MoneyManager.instance.GetMoneyCount() >= student.GetComponent<StudentInfo>().cost)
        {
            canPlace = true;
            StudentManager.placing = true;
        }
        else
        {
            StartCoroutine(FlashSprite());
        }

        // deselect the selected student
        StudentManager.Deselect();
        StudentManager.selected = student;
        // show UI for hiring a student
        UIManager.instance.ShowStudentHiringUI(student);
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
}
