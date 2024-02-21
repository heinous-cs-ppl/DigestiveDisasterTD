using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private RectTransform buttonTransform;
    private Vector2 startPointerPosition;
    private Vector2 startButtonPosition;
    private bool isDragging;

    [SerializeField] private GameObject student;
    
    void Start()
    {
        buttonTransform = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData pointer) {
        // store the starting pointer position and rectTransform position (position of the button)
        startPointerPosition = pointer.position;
        startButtonPosition = buttonTransform.anchoredPosition;
        isDragging = true;
    }

    public void OnDrag(PointerEventData pointer) {
        // update the position of the pointer and button
        if(isDragging) {
            Vector2 pointerDelta = pointer.position - startPointerPosition;
            // Convert pointer movement delta to local space of RectTransform
            Vector2 localPointerDelta = new Vector2(
                pointerDelta.x / buttonTransform.lossyScale.x,
                pointerDelta.y / buttonTransform.lossyScale.y
            );
            buttonTransform.anchoredPosition = startButtonPosition + pointerDelta;
        }
    }

    public void OnPointerUp(PointerEventData pointer) {
        isDragging = false;
        // check if the current pointer position is on a tile where students can be placed
        Instantiate(student, pointer.position, Quaternion.identity);

    }
    
}
