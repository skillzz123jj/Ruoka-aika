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

    [SerializeField] ActiveFood activeFood;

    public static DragAndDrop dragAndDrop;

    private void Start()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer && Application.isMobilePlatform)
        {
            gameObject.transform.localScale = new Vector2(1.1f, 1.1f);

        }

    }
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
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        if (screenPosition.x > Screen.width + 10)
        {
            initialPosition = RandomAnimalAndFood.randomAnimalAndFood.FoodPositionDictionary[gameObject];


            float t = Time.deltaTime * moveSpeed;

            transform.position = Vector3.Lerp(transform.position, initialPosition, t);
        }

        if (isDragging)
        {
            if (Input.mousePosition.x < 0 || Input.mousePosition.x > Screen.width ||
          Input.mousePosition.y < 0 || Input.mousePosition.y > Screen.height)
            {
                isDragging = false;

            }
            else
            {
                isDragging = true;
            }

            if (Input.touchCount > 0)
            {
                //Gives the food an offset when player is using a mobile device
                Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position) + offset;


                transform.position = touchPosition;//new Vector3(touchPosition.x - 1f, touchPosition.y + 1f, touchPosition.z);

                GameObject currentActiveFood = activeFood.GetClickedFood(gameObject);
                activeFood.ValidFood(currentActiveFood);
                //activeFood.ChooseFood(currentActiveFood);

            }
            else
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                transform.position = mousePosition;

                GameObject currentActiveFood = activeFood.GetClickedFood(gameObject);
                activeFood.ValidFood(currentActiveFood);
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
