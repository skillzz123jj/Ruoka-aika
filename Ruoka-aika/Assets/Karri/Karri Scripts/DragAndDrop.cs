using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private bool isDragging;
  
    public static DragAndDrop dragAndDrop;
    public void OnMouseDown()
    {
        isDragging = true;        

    }

    public void OnMouseUp()
    {
        isDragging = false;
    }


    // Update is called once per frame
    void Update()
    {
        if (isDragging)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            transform.Translate(mousePosition);
        }
    }
}




