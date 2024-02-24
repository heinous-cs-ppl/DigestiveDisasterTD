using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot : MonoBehaviour {
    
    [Header("References")]
    [SerializeField] private SpriteRenderer plotSR;
    [SerializeField] private Color hoverColor;

    [HideInInspector]
    public GameObject student = null;
    private GameObject selectedStu;
    private Color startColor;
    private Transform plot;

    /* Triggered when the mouse hovers over a plot with a collider on it */
    private void OnMouseEnter() {
        plotSR.color = hoverColor;
    }

    private void OnMouseExit() {
        plotSR.color = startColor;
    }

    // Wait for a few frames before placing student so that code in StudentPlacement executes first
    private IEnumerator PlaceStudent() {
        yield return new WaitForSeconds(0.05f);
        Debug.Log("Place student here" + name);
        StudentManager.Select(Instantiate(selectedStu, transform.position, Quaternion.identity), gameObject);
    }

    /* If the plot is clicked */
    private void OnMouseDown() {
        // If no student is selected, select the student on this tile
        if (selectedStu == null) {
            StudentManager.selected = student;
            return;
        }

        // Place student if seat is empty
        if (student == null) {
            StudentPlacement.plotPlaced = true;
            // Wait for some code in StudentPlacement to run first
            StartCoroutine(PlaceStudent());
        }
        // else {
        //     Debug.Log("Student already here" + name);
        // }

    }

    // Start is called before the first frame update
    void Start()
    {
        plot = this.transform;
        startColor = plotSR.color;
    }

    // Update is called once per frame
    void Update()
    {
        selectedStu = StudentManager.placementSelected;
    }
}
