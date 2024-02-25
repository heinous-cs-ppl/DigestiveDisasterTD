using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveStudent : MonoBehaviour
{
    bool moving = false;
    private GameObject studentPreview;

    private GameObject student;
    private Sprite studentSprite;
    public LayerMask studentLayer;

    private GameObject map;
    private Bounds mapBounds;
    private int moveCost = 50;

    private float flashDuration = 0.5f;
    private int numberOfFlashes = 2;
    private Image moneyImage;
    void Start() {
        map = GameObject.Find("Map");
        SpriteRenderer mapSprite = map.GetComponent<SpriteRenderer>();
        mapBounds = mapSprite.bounds;

        moneyImage = GameObject.Find("Money Icon").GetComponent<Image>();
    }
    public void SetMoving() {
        if (MoneyManager.GetMoneyCount() >= moveCost) {
            student = StudentManager.selected;
            studentSprite = student.GetComponentInChildren<SpriteRenderer>().sprite;

            moving = true;
            StudentManager.moving = true;
        } else {
            StartCoroutine(FlashSprite());
        }
    }

    void Update() {
        if(moving) {
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
                    // disable moving
                    moving = false;
                    StudentManager.moving = false;
                    // destroy the preview if it exists
                    if(studentPreview) Destroy(studentPreview);
                }
            }
        }
    }

    private void Place(Vector2 pos) {
        // check if there is a student at the position
        Debug.Log("detected click: " + pos.x + ", " + pos.y);
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 1f, studentLayer);
        if (!(hit)) {
            // check if a plot was selected as the destination for the student
            RaycastHit2D plotHit = Physics2D.Raycast(pos, Vector2.zero, 1f, LevelManager.instance.plotLayer);
            if (plotHit) {
                // set old plot to have no student
                RaycastHit2D oldPlot = Physics2D.Raycast(student.transform.position, Vector2.zero, 1f, LevelManager.instance.plotLayer);
                oldPlot.transform.gameObject.GetComponent<Plot>().student = null;
                // set the position of the student to the position
                student.transform.position = pos;
                // add cost for moving student here
                MoneyManager.TakeMoney(moveCost);
                UIManager.UpdateMoney();

                // disable moving once the student has been placed
                moving = false;
                StudentManager.moving = false;

                // destroy the preview
                Destroy(studentPreview);

                // "reselect" the selected student to redraw the range circle (I'm lazy)
                StudentManager.Select(StudentManager.selected);
            } else {
                Debug.Log("No plot here");
                Destroy(studentPreview);
                moving = false;
            }

        } else {
            Debug.Log("There's already a student here");
            Destroy(studentPreview);
            moving = false;
        }
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
