using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodController : MonoBehaviour
{
    private bool isHeld = false;
    private Vector2 initialPosition;
    private Vector2 offset;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        if (isHeld)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePosition + offset;
        }
    }

    public void PickUp()
    {
        isHeld = true;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        offset = (Vector2)transform.position - mousePosition;
    }

    public bool IsHeld()
    {
        return isHeld;
    }

    public void Release()
    {
        isHeld = false;
    }

    public void ResetFoodPosition()
    {
        transform.position = initialPosition;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isHeld && other.gameObject.layer == LayerMask.NameToLayer("Animal"))
        {
            FeedAnimal(other.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (isHeld && other.gameObject.layer == LayerMask.NameToLayer("Animal"))
        {
            // You can handle some action here when exiting the animal collider if needed.
        }
    }

    public void FeedAnimal(GameObject animal)
    {
        // Handle feeding logic with the animal here.
        // Example: animal.GetComponent<AnimalController>().Feed();
        ResetFoodPosition();
    }
}

