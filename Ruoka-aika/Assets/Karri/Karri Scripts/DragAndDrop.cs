using UnityEngine;
using System.Collections.Generic;

public class DragAndDrop : MonoBehaviour
{
    public Dictionary<GameObject, Vector2> FoodPositionDictionary = new Dictionary<GameObject, Vector2>();

    public float arrowKeysSpeed = 5.0f; // Adjust this speed as needed

    private bool isDragging = false;
    private Vector3 offset;

    public void OnMouseDown()
    {
        isDragging = true;
        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
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
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePosition + offset;

            // Update the position in the dictionary
            if (FoodPositionDictionary.ContainsKey(gameObject))
            {
                FoodPositionDictionary[gameObject] = transform.position;
            }
        }
        else
        {
            // Get input from the arrow keys only if not dragging with the mouse
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            // Calculate the movement direction
            Vector2 moveDirection = new Vector2(horizontalInput, verticalInput).normalized;

            // Apply the movement
            transform.Translate(moveDirection * arrowKeysSpeed * Time.deltaTime);

            // Update the position in the dictionary
            if (FoodPositionDictionary.ContainsKey(gameObject))
            {
                FoodPositionDictionary[gameObject] = transform.position;
            }
        }
    }
}
