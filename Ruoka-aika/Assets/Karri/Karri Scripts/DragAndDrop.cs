using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private bool isDragging;
    public bool move = true;

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
                transform.position = RandomAnimalAndFood.randomAnimalAndFood.FoodPositionDictionary[gameObject];
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