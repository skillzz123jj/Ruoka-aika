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


//---------------------------------------------------------------------------------------------------------------------
//Below is the code with the keyboard movement

//using System;
//using System.Collections;
//using System.Collections.Generic;
//using Unity.VisualScripting;
//using UnityEngine;
//using System.Collections.Generic;

//public class DragAndDrop : MonoBehaviour
//{


//    public static DragAndDrop dragAndDrop;
//    public Dictionary<GameObject, Vector2> FoodPositionDictionary = new Dictionary<GameObject, Vector2>();

//    public float arrowKeysSpeed = 5.0f; // Adjust this speed as needed

//    private bool isDragging = false;
//    private Vector3 offset;

//    public void OnMouseDown()
//    {


//        isDragging = true;
//        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
//    }

//    public void OnMouseUp()
//    {
//        isDragging = false;
//    }



//    // Update is called once per frame
//    void Update()
//    {
//        if (isDragging)
//        {
//            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//            transform.position = mousePosition + offset;

//            // Update the position in the dictionary
//            if (FoodPositionDictionary.ContainsKey(gameObject))
//            {
//                FoodPositionDictionary[gameObject] = transform.position;
//            }
//        }

//        else
//        {
//            // Get input from the arrow keys only if not dragging with the mouse
//            float horizontalInput = Input.GetAxis("Horizontal");
//            float verticalInput = Input.GetAxis("Vertical");

//            // Calculate the movement direction
//            Vector2 moveDirection = new Vector2(horizontalInput, verticalInput).normalized;

//            // Apply the movement
//            transform.Translate(moveDirection * arrowKeysSpeed * Time.deltaTime);

//            // Update the position in the dictionary
//            if (FoodPositionDictionary.ContainsKey(gameObject))
//            {
//                FoodPositionDictionary[gameObject] = transform.position;
//            }
//        }
//    }
//}
