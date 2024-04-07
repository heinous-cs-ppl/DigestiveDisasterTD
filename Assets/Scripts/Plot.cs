using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private SpriteRenderer plotSR;
    [SerializeField] private Color hoverColor;
    public bool aboveTable;

    [HideInInspector]
    public GameObject student = null;
    private GameObject selectedStu;
    private Color startColor;
    private Transform plot;
    [HideInInspector] 
    public Transform plotOnLeft;

    /* Triggered when the mouse hovers over a plot with a collider on it */
    private void OnMouseEnter()
    {
        plotSR.color = hoverColor;
    }

    private void OnMouseExit()
    {
        plotSR.color = startColor;
    }

    // Wait for a few frames before placing student so that code in StudentPlacement executes first
    private IEnumerator PlaceStudent()
    {
        yield return new WaitForSeconds(0.05f);
        // Debug.Log("Place student here" + name);
        // Occupy plot on left if placing Engineer
        if (selectedStu.GetComponent<StudentInfo>().turret is MachineStudent && plotOnLeft) {
            plotOnLeft.gameObject.GetComponent<Plot>().student = LevelManager.instance.machineRepresentation;
        }
        StudentManager.Place(selectedStu, plot, aboveTable);
        StudentManager.placing = false;
    }

    /* If the plot is clicked */
    private void OnMouseDown()
    {
        // Place student if seat is empty and in placement mode
        if (student == null && StudentManager.placing == true)
        {
            // If selectedStu is an Engineer, is there a free tile on the left for a machine? Do not place if not
            if (selectedStu.GetComponent<StudentInfo>().turret is MachineStudent)
            {
                Debug.Log("MACHINE");
                Debug.Log(plot);
                Debug.Log(plotOnLeft);
                if (plotOnLeft && plotOnLeft.gameObject.GetComponent<Plot>().student != null)
                {
                    Debug.Log("no placey");
                    return;
                }
            }
            StudentPlacement.studentPlacedOnPlot = true;
            // Wait for some code in StudentPlacement to run first
            StartCoroutine(PlaceStudent());
        }
        else if (StudentManager.moving == true)
        {
            if (selectedStu.GetComponent<StudentInfo>().turret is MachineStudent)
            {
                if (this.student && this.student != LevelManager.instance.machineRepresentation && (this.student.GetComponent<StudentInfo>().turret is MachineStudent)) {return;}    // Return if moving onto self
                // Left one tile case
                if (this.student && this.transform == StudentManager.plotOfSelected.GetComponent<Plot>().plotOnLeft)
                {
                    if (this.plotOnLeft && !this.plotOnLeft.gameObject.GetComponent<Plot>().student)
                    {
                        Debug.Log("left move");
                        this.student = selectedStu;
                        MoveStudent.instance.Place(plot);
                    }
                    else if (!this.plotOnLeft)
                    {
                        Debug.Log("left move to left edge");
                        this.student = selectedStu;
                        MoveStudent.instance.Place(plot);
                    }
                }
                // Right one tile case
                else if (!this.student && this.plotOnLeft == StudentManager.plotOfSelected.transform)
                {
                    Debug.Log("right move");
                    this.student = selectedStu;
                    MoveStudent.instance.Place(plot);
                }
                // Other valid moves
                else if (!this.student)
                {
                    if (!this.plotOnLeft)    // Left edge of table 
                    {
                        this.student = selectedStu;
                        MoveStudent.instance.Place(plot);
                    }
                    else if (this.plotOnLeft && !this.plotOnLeft.gameObject.GetComponent<Plot>().student)
                    {
                        Debug.Log("general move");
                        this.student = selectedStu;
                        MoveStudent.instance.Place(plot);
                    }
                    else
                    {
                        Debug.Log("no movey - student on left");
                    }
                }
                else
                {
                    Debug.Log("no movey - student here");
                }
            }
            else if (this.student == null)
            {
                this.student = selectedStu;
                MoveStudent.instance.Place(plot);
            }
        }
        // Start mouse drag if student was not placed or moved here on this click
        else if (!StudentManager.placing && this.student && this.student != LevelManager.instance.machineRepresentation)   
        {
            Debug.Log("Started drag over: " + this);
            StudentManager.mouseDragging = true;
            StartCoroutine(DelayAndSetMoving());       // Preview updates in MoveStudent
        }
    }

    private IEnumerator DelayAndSetMoving()     // Wait for Update() in StudentManager to run before calling MoveStudent.SetMoving or you won't have a good time :D
    {
        yield return new WaitForSeconds(0.05f);
        MoveStudent.instance.SetMoving();
    }

    /* The tile that the drag started on will know if the mouse is still being dragged even if it leaves the tile. */
    private void OnMouseDrag()
    {
        if (StudentManager.mouseDragging)
        {
            StudentManager.draggingOver = null;
            RaycastHit2D plotHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 1f, LevelManager.instance.plotLayer);
            if (plotHit) {StudentManager.draggingOver = plotHit.transform.gameObject.GetComponent<Plot>();}
            // Debug.Log("Dragging over: " + StudentManager.draggingOver);
        }
        // Debug.Log("dragging");
    }

    /* Mouse was held down, and gets released over this plot */
    private void OnMouseUp()
    {
        StartCoroutine(DelayAndHandleMouseUp());
    }

    private IEnumerator DelayAndHandleMouseUp()     // Makes sure DelayAndSetMoving() can finish running to avoid issues on fast clicks
    {
        yield return new WaitForSeconds(0.05f);
        if (StudentManager.mouseDragging)
        {
            Debug.Log("Released drag over: " + StudentManager.draggingOver);
            StudentManager.mouseDragging = false;
            Plot plotDraggedOver = StudentManager.draggingOver;

            if (plotDraggedOver == this)   // Same time, treat as just a click
            {
                StudentManager.moving = false;
                Destroy(MoveStudent.instance.studentPreview);
                MoveStudent.instance.rangeCircle.transform.localScale = Vector2.zero;
            }
            else if (plotDraggedOver == null)   // Do the move if released over a plot
            {
                BadDragSoEndDragMove();
            }
            else
            {
                Debug.Log("try to drag movey");
                if (this.student.GetComponent<StudentInfo>().turret is MachineStudent)    // Handle drag moving a machine student
                {
                    if (!plotDraggedOver.student && (!plotDraggedOver.plotOnLeft || !plotDraggedOver.plotOnLeft.gameObject.GetComponent<Plot>().student)) 
                    {
                        MoveStudent.instance.Place(StudentManager.draggingOver.transform);
                        StudentManager.draggingOver.student = selectedStu;
                    }
                    // Allow a machine student to be dragged one tile left, but don't let other machine students be moved onto machine tiles not belonging to them
                    //      also make sure when moving a machine student, their machine does not get moved on top of another student
                    else if (this.plotOnLeft == plotDraggedOver.transform && ((plotDraggedOver.plotOnLeft && !plotDraggedOver.plotOnLeft.gameObject.GetComponent<Plot>().student) || !plotDraggedOver.plotOnLeft)) 
                    {
                        MoveStudent.instance.Place(StudentManager.draggingOver.transform);
                        StudentManager.draggingOver.student = selectedStu;
                    }
                    // Allow a machine student to be dragged one tile right
                    else if (!plotDraggedOver.student && this.transform == plotDraggedOver.plotOnLeft)
                    {
                        MoveStudent.instance.Place(StudentManager.draggingOver.transform);
                        StudentManager.draggingOver.student = selectedStu;
                    }
                    else
                    {
                        BadDragSoEndDragMove();   
                    }
                }
                else if (!plotDraggedOver.student)      
                {
                    MoveStudent.instance.Place(StudentManager.draggingOver.transform);
                    StudentManager.draggingOver.student = selectedStu;
                }
                else
                {
                    BadDragSoEndDragMove();
                }
            }
        }
    }

    private void BadDragSoEndDragMove()
    {
        StudentManager.moving = false;
        StudentManager.Deselect();
        Destroy(MoveStudent.instance.studentPreview);  
        MoveStudent.instance.rangeCircle.transform.localScale = Vector2.zero;
    }

    // Start is called before the first frame update
    void Start()
    {
        plot = this.transform;
        RaycastHit2D plotHit = Physics2D.Raycast((Vector2)this.transform.position + Vector2.left, Vector2.left, 1f, LevelManager.instance.plotLayer);
        plotOnLeft = plotHit.transform;
        startColor = plotSR.color;
    }

    // Update is called once per frame
    void Update()
    {
        selectedStu = StudentManager.selected;
    }
}