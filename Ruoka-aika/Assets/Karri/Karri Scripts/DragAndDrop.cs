using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private Vector3 offset;

    private bool isDragging;
    public bool move = true;

    public Vector2 initialPosition;
    public float moveSpeed = 3.0f;

    public static DragAndDrop dragAndDrop;
    public void OnMouseDown()
    {
        isDragging = true;
        move = true;

        if (Input.touchCount > 0)
        {
            offset = transform.position - Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
        }

    }

    public void OnMouseUp()
    {
        isDragging = false;
    }

    void Update()
    {

        if (isDragging)
        {

            if (Input.touchCount > 0)
            {
                //Gives the food an offset when player is using a mobile device
                Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position) + offset;
           
                transform.position = new Vector3(touchPosition.x - 1f, touchPosition.y + 1f, touchPosition.z);

            }
            else
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                transform.position = mousePosition;
            }
        }
        else
        {
            if (move && gameObject != ActiveFood.activeFood.currentActiveFood)
            {
                initialPosition = RandomAnimalAndFood.randomAnimalAndFood.FoodPositionDictionary[gameObject];


                float t = Time.deltaTime * moveSpeed;

                transform.position = Vector3.Lerp(transform.position, initialPosition, t);

            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        move = false;

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        move = true;
    }
}
