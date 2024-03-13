using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// this script is purely to move the child gameobjects of the button down when it's being pressed
public class ButtonScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        foreach (Transform child in transform)
        {
            child.position -= new Vector3(0, 3f / 16f, 0);
        }
    }
    

    public void OnPointerUp(PointerEventData eventData)
    {
        foreach (Transform child in transform)
        {
            child.position += new Vector3(0, 3f / 16f, 0);
        }
    }
}
