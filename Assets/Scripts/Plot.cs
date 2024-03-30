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
                // if (!plotOnLeft)    // Left edge case
                // {
                //     Debug.Log("can place");
                // }
                // else if (plotOnLeft.gameObject.GetComponent<Plot>().student == null)    // Left tile exists and is empty
                // {
                //     Debug.Log("can place"); 
                // }
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
                // Turret studentOnLeftType = this.plotOnLeft.gameObject.GetComponent<Plot>().student.GetComponent<StudentInfo>().turret;
                if (plotOnLeft && plotOnLeft.gameObject.GetComponent<Plot>().student != null && (!(this.plotOnLeft.gameObject.GetComponent<Plot>().student.GetComponent<StudentInfo>().turret is MachineStudent)))
                {
                    Debug.Log("no movey");
                    return;
                }
                // Allow moving of machine by one tile to the left
                if (this.student == LevelManager.instance.machineRepresentation )
                {
                    this.student = selectedStu;
                    MoveStudent.instance.Place(plot);
                    return;
                }
            }
            if (this.student == null)
            {
                this.student = selectedStu;
                MoveStudent.instance.Place(plot);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        plot = this.transform;
        RaycastHit2D plotHit = Physics2D.Raycast((Vector2)this.transform.position + Vector2.left, Vector2.left, 1f, LevelManager.instance.plotLayer);
        plotOnLeft = plotHit.transform;
        // if (plotHit) 
        // {
        //     plotOnLeft = plotHit.transform;
        // }
        // else
        // {
        //     plotOnLeft = null;
        // }
        startColor = plotSR.color;
    }

    // Update is called once per frame
    void Update()
    {
        selectedStu = StudentManager.selected;
    }
}
