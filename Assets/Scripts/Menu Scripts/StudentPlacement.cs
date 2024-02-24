using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StudentPlacement : MonoBehaviour
{
    [SerializeField] private GameObject student;
    [SerializeField] private Sprite studentSprite;
    private GameObject studentPreview;
    private bool canPlace = false;
    private GameObject map;
    private Bounds mapBounds;
    public LayerMask studentLayer;
    private StudentInfo studentInfo;

    private float flashDuration = 0.5f;
    private int numberOfFlashes = 2;
    private Image moneyImage;

    void Start() {
        map = GameObject.Find("Map");
        SpriteRenderer mapSprite = map.GetComponent<SpriteRenderer>();
        mapBounds = mapSprite.bounds;

        studentInfo = student.GetComponent<StudentInfo>();

        moneyImage = GameObject.Find("Money Icon").GetComponent<Image>();
        
    }

    void Update()
    {
        // check for mouse click and if player can place the student
        if (canPlace) {
            // get cursor position
            Vector2 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // round the cursor position to the middle of the tile
            cursorPosition.x = Mathf.Ceil(cursorPosition.x) - 0.5f;
            cursorPosition.y = Mathf.Ceil(cursorPosition.y) - 0.5f;

            // check if the cursor is on the map
            if (mapBounds.Contains(cursorPosition)) {
                // create the student preview if it doesn't exist
                if (!(studentPreview)) {
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

                if (Input.GetMouseButtonDown(0)) {
                    Place(cursorPosition);
                }
            } else {
                // if the player clicks somewhere not on the map
                if (Input.GetMouseButtonDown(0)) {
                    // disable placing
                    canPlace = false;
                    StudentManager.placing = false;
                    // destroy the preview if it exists
                    if(studentPreview) Destroy(studentPreview);

                    UIManager.HideStudentHiringUI();
                }
            }
        }
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