using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

public class DragAndDrop : MonoBehaviour
{
    private bool isDragging;
    public bool move = true;

    public Vector2 initialPosition;
    public float moveSpeed = 3.0f;

    public static DragAndDrop dragAndDrop;
    public void OnMouseDown()
    {
        isDragging = true;
        move = true;

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
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePosition;

            // Check if this GameObject is in the dictionary
            if (!RandomAnimalAndFood.randomAnimalAndFood.FoodPositionDictionary.ContainsKey(gameObject))
            {
                RandomAnimalAndFood.randomAnimalAndFood.FoodPositionDictionary.Add(gameObject, mousePosition);
            }

            //if (!move)
            //{
            //    // Update the position in the dictionary
            //    RandomAnimalAndFood.randomAnimalAndFood.FoodPositionDictionary[gameObject] = mousePosition;

            //}
            //else
            //{
            //    gameObject.transform.position = intialPosition;
            //}
        }
        else
        {
          if (move)
            {
                //Moves the food slowly back to its initial position if it isnt on any animal
                initialPosition = RandomAnimalAndFood.randomAnimalAndFood.FoodPositionDictionary[gameObject];
                Vector3 newPosition = Vector2.Lerp(transform.position, initialPosition, Time.deltaTime * moveSpeed);
                transform.position = newPosition;

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
