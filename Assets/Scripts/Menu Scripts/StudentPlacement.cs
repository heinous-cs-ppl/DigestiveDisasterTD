using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StudentPlacement : MonoBehaviour
{
    [SerializeField] private GameObject student;
    [SerializeField] private Sprite studentSprite;
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
    private float[] sideUIdim = new float[4];  // Left, Right, Top, Bottom side
    [HideInInspector] public static bool plotPlaced = false; 

    void Start() {
        // Bounds of the map
        map = GameObject.Find("Map");
        SpriteRenderer mapSprite = map.GetComponent<SpriteRenderer>();
        mapBounds = mapSprite.bounds;

        studentInfo = student.GetComponent<StudentInfo>();

        moneyImage = GameObject.Find("Money Icon").GetComponent<Image>();
        
        // Bounds of the side UI canvas
        ui = GameObject.Find("UI Canvas");
        RectTransform sideUItransform = ui.GetComponent<RectTransform>();
        Vector2 position = sideUItransform.position;
        Vector2 size = sideUItransform.rect.size;
        sideUIdim[0] = position.x - size.x/2;   // Left
        sideUIdim[1] = position.x + size.x/2;   // Right
        sideUIdim[2] = position.y + size.y/2;   // Top
        sideUIdim[3] = position.y - size.y/2;   // Bottom
    }

    void Update()
    {
        // get cursor position
        Vector2 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // round the cursor position to the middle of the tile
        cursorPosition.x = Mathf.Ceil(cursorPosition.x) - 0.5f;
        cursorPosition.y = Mathf.Ceil(cursorPosition.y) - 0.5f;

        // Check if the cursor is on the map and placement is allowed
        if (mapBounds.Contains(cursorPosition) && canPlace) {
            Debug.Log("ON MAP AND CAN PLACE");
            // create the student preview if it doesn't exist
            if (!(studentPreview)) {
                // create a new GameObject with a sprite renderer
                studentPreview = new GameObject("StudentPreview");
                SpriteRenderer previewSprite = studentPreview.AddComponent<SpriteRenderer>();

                // set the sprite to the student sprite
                previewSprite.sprite = studentSprite;

                // set the opacity to 50%
                Color preview = studentPreview.GetComponent<SpriteRenderer>().color;
                preview.a = 0.5f; // Set alpha value to 50% opacity
                studentPreview.GetComponent<SpriteRenderer>().color = preview;
            }
            // set the position of the student preview to the cursor's position
            studentPreview.transform.position = cursorPosition;

            if (Input.GetMouseButtonDown(0)) {
                // check if there is a student at the cursor's position
                Debug.Log("detected click: " + cursorPosition.x + ", " + cursorPosition.y);
                RaycastHit2D hit = Physics2D.Raycast(cursorPosition, Vector2.zero, 1f, studentLayer);
                if (hit) {
                    Debug.Log("There's already a student here");
                } else {
                    // Student is created from Plots.cs if a plot was clicked. The student will be selected by default.
                    Debug.Log("place student from plots");
                    
                    // This deselection happens unless a student was placed. Timed to wait for Plot to finish running.
                    if (!plotPlaced) {StartCoroutine(DeselectStudent());}
                    else {plotPlaced = false;}
                }
            }
        }
        else if (!(cursorPosition.x >= sideUIdim[0] && cursorPosition.x <= sideUIdim[1] && cursorPosition.y <= sideUIdim[2] 
                    && cursorPosition.y >= sideUIdim[3]) && StudentManager.placementSelected == null) {
            /* Must check if StudentManager.placementSelected is null, instead of !canPLace. This is because there are multiple instances of this StudentPlacement.cs running, and
                only one instance of StudentManager running. Will error otherwise */

            Debug.Log("OFF UI & NOT IN PLACEMENT");

            if (Input.GetMouseButtonDown(0)) {
                RaycastHit2D stuHit = Physics2D.Raycast(cursorPosition, Vector2.zero, 1f, studentLayer);

                // If the player clicks on a student, select them
                if (stuHit) {
                    // Debug.Log("There's already a student here");
                    RaycastHit2D plotHit = Physics2D.Raycast(cursorPosition, Vector2.zero, 1f, LevelManager.instance.plotLayer);
                    StudentManager.Select(stuHit.transform.gameObject, plotHit.transform.gameObject);
                } 
                // If the player clicks somewhere random, deselect any selected student
                else {
                    StudentManager.Deselect();
                }
            }
        }

        // Deselect student for placement if another student was selected for placement. Prevents duplicate selection for placing
        if (student != StudentManager.placementSelected) {
            canPlace = false;
            Destroy(studentPreview);
        }
    }

    public IEnumerator DeselectStudent() {
        yield return new WaitForSeconds(0.05f);

        Debug.Log("deselect student now");
        StudentManager.Deselect();
        canPlace = false;
        Destroy(studentPreview);
    }

    private void Place(Vector2 position) {
        // check if there is a student at the cursor's position
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero, 1f, studentLayer);
        if (!(hit)) {
            // create student at cursor position on mouse click, the student will be selected by default
            StudentManager.Select(Instantiate(student, position, Quaternion.identity));

            // hide the student hiring UI
            UIManager.HideStudentHiringUI();

            // do money related actions
            MoneyManager.TakeMoney(studentInfo.cost);
            UIManager.UpdateMoney();

            // disable placing once the student has been placed
            canPlace = false;
            StudentManager.placing = false;

            // destroy the preview
            Destroy(studentPreview);

        } else Debug.Log("There's already a student here");
    }

    // called when the button is clicked
    public void StartPlacementMode() {
        if (MoneyManager.GetMoneyCount() >= student.GetComponent<StudentInfo>().cost) {
            canPlace = true;
            StudentManager.placing = true;
        } else {
            StartCoroutine(FlashSprite());
        }

        // deselect the selected student
        StudentManager.Deselect();
        StudentManager.placementSelected = student;
        // show UI for hiring a student
        UIManager.ShowStudentHiringUI(student, studentSprite);
    }

    IEnumerator FlashSprite() {
        for (int i = 0; i < numberOfFlashes; i++) {
            // gradually change the sprite color to red
            float elapsedTime = 0f;
            while (elapsedTime < flashDuration) {
                moneyImage.color = Color.Lerp(Color.white, Color.red, elapsedTime / flashDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // gradually change the sprite color back to its original color
            elapsedTime = 0f;
            while (elapsedTime < flashDuration) {
                moneyImage.color = Color.Lerp(Color.red, Color.white, elapsedTime / flashDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
    }
}