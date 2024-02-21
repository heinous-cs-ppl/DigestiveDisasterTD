using UnityEngine;

public class StudentPlacement : MonoBehaviour
{
    [SerializeField] private GameObject student;
    [SerializeField] private Sprite studentSprite;
    private GameObject studentPreview;
    private bool canPlace = false;
    private GameObject map;
    private Bounds mapBounds;

    void Start() {
        map = GameObject.Find("Map");
        SpriteRenderer mapSprite = map.GetComponent<SpriteRenderer>();
        mapBounds = mapSprite.bounds;
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

                    // set the opacity to 50%
                    Color preview = studentPreview.GetComponent<SpriteRenderer>().color;
                    preview.a = 0.5f; // Set alpha value to 50% opacity
                    studentPreview.GetComponent<SpriteRenderer>().color = preview;
                }
                // set the position of the student preview to the cursor's position
                studentPreview.transform.position = cursorPosition;

                if (Input.GetMouseButtonDown(0)) {
                    // create student at cursor position on mouse click
                    Instantiate(student, cursorPosition, Quaternion.identity);

                    // disable placing once the student has been placed
                    canPlace = false;

                    // destroy the preview
                    Destroy(studentPreview);
                }
            } else {
                // if the player clicks somewhere not on the map
                if (Input.GetMouseButtonDown(0)) {
                    // disable placing
                    canPlace = false;
                    // destroy the preview if it exists
                    if(studentPreview) Destroy(studentPreview);
                }
            }
        }
    }

    // called when the button is clicked
    public void StartPlacementMode()
    {
        canPlace = true;
    }
}