using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    public GameObject currentActiveFood;
    public GameObject wrongFoodSprite;
    public bool foodWasFed;

    public float speed = 5.0f;

    private int currentFoodIndex = 0;
    private bool isSelectingFood = true; // Indicates whether you are selecting food or animals.

    void ResetBool()
    {
        foodWasFed = false;
    }

    void Update()
    {
        HandleKeyboardInput();
        HandleMouseInput();

        if (currentActiveFood != null)
        {
            HandleMovement();
        }
    }

    void HandleKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isSelectingFood = !isSelectingFood;
            SwitchToNextFood();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (foodWasFed)
            {
                // Perform actions when confirming the selection (e.g., feeding).
                FeedSelectedFood();
            }
            else
            {
                foodWasFed = true;
                Invoke("ResetBool", 0.1f);
            }
        }
    }

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject clickedFood = GetClickedFood();

            if (clickedFood != null && clickedFood != currentActiveFood)
            {
                ChangeBackground(clickedFood);

                if (currentActiveFood != null)
                {
                    ResetBackground(currentActiveFood);
                }

                currentActiveFood = clickedFood;
            }
        }
    }

    void HandleMovement()
    {
        // Handle movement using arrow keys or WASD
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(horizontalInput, verticalInput) * speed * Time.deltaTime;

        // Move the active food
        currentActiveFood.transform.Translate(movement);
    }

    public void SwitchToNextFood()
    {
        if (RandomAnimalAndFood.randomAnimalAndFood.chosenFoods.Count == 0)
        {
            return;
        }

        if (currentActiveFood != null)
        {
            ResetBackground(currentActiveFood);
        }

        // Increment the currentFoodIndex to switch to the next food
        currentFoodIndex = (currentFoodIndex + 1) % RandomAnimalAndFood.randomAnimalAndFood.chosenFoods.Count;

        currentActiveFood = RandomAnimalAndFood.randomAnimalAndFood.chosenFoods[currentFoodIndex];

        if (currentActiveFood != null)
        {
            ChangeBackground(currentActiveFood);
        }
    }

    private void ResetBackground(GameObject food)
    {
        // Implement background reset logic here
    }

    private void ChangeBackground(GameObject food)
    {
        // Implement background change logic here
    }

    private void FeedSelectedFood()
    {
        // Implement feeding logic here
    }

    private GameObject GetClickedFood()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider != null)
        {
            GameObject clickedFood = hit.collider.gameObject;
            return clickedFood;
        }

        return null;
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
