using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveFood : MonoBehaviour
{
    public GameObject currentActiveFood;
    SpriteRenderer backgroundSpriteRenderer;
    SpriteRenderer spriteRenderer;
    GameObject previousActiveFood;

    Collider2D collider;

    [SerializeField] Sprite activeFoodBackground;
    [SerializeField] Sprite defaultBackground;
    public GameObject wrongFoodSprite;

    public bool foodWasFed;

    public float speed = 5.0f;

    private int currentFoodIndex = 0;

    public static ActiveFood activeFood;

    void ResetBool()
    {
        foodWasFed = false;
    }

    void Start()
    {
        activeFood = this;
        previousActiveFood = currentActiveFood;
    }

    void Update()
    {
     
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchToNextFood();
        }

        //This makes sure that the food can be fed 
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonUp(0))
        {
            foodWasFed = true;
            Invoke("ResetBool", 0.1f);
        }


        //// Handle movement using arrow keys or WASD
        //float horizontalInput = Input.GetAxis("Horizontal");
        //float verticalInput = Input.GetAxis("Vertical");

        //Vector2 movement = new Vector2(horizontalInput, verticalInput) * speed * Time.deltaTime;

        //// Move the active food
        //currentActiveFood.transform.Translate(movement);

        //Check if a new food is clicked by the player
        if (Input.GetMouseButtonDown(0))
        {
            GameObject newActiveFood = GetClickedFood();

            if (newActiveFood != null && newActiveFood != currentActiveFood)
            {
                
                ChangeBackground(newActiveFood);

               
                if (previousActiveFood != null)
                {
                    ChangeBackground(previousActiveFood);
                }

                //Update the active food and the previous active food
                currentActiveFood = newActiveFood;
                previousActiveFood = newActiveFood;
            }
        }
    }

    private GameObject GetClickedFood()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider != null)
        {
            GameObject clickedFood = hit.collider.gameObject;

            //Find the index of the clicked food in the chosen foods list
            int clickedFoodIndex = RandomAnimalAndFood.randomAnimalAndFood.chosenFoods.IndexOf(clickedFood);

            if (clickedFoodIndex != -1)
            {
                //Update the currentFoodIndex
                currentFoodIndex = clickedFoodIndex;
              
                if (currentActiveFood != null)
                {
                    ResetBackground(currentActiveFood);
                }

                currentActiveFood = clickedFood;
                ChangeBackground(currentActiveFood);
            }

            return clickedFood;
        }

        return null;
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

        //Increment the currentFoodIndex to switch to the next food
        currentFoodIndex = (currentFoodIndex + 1) % RandomAnimalAndFood.randomAnimalAndFood.chosenFoods.Count;

        currentActiveFood = RandomAnimalAndFood.randomAnimalAndFood.chosenFoods[currentFoodIndex];

        if (currentActiveFood != null)
        {
            ChangeBackground(currentActiveFood);
        }
    }
    private void ResetBackground(GameObject food)
    {
        collider = food.GetComponent<Collider2D>();
        collider.isTrigger = false;
        spriteRenderer = food.GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = 30;
        GameObject child = food.transform.GetChild(0).gameObject;
        backgroundSpriteRenderer = child.GetComponent<SpriteRenderer>();
        backgroundSpriteRenderer.sortingOrder = 25;
        backgroundSpriteRenderer.sprite = defaultBackground;
    }

    private void ChangeBackground(GameObject food)
    {
        collider = food.GetComponent<Collider2D>();
        collider.isTrigger = true;
        spriteRenderer = food.GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = 32;
        GameObject child = food.transform.GetChild(0).gameObject;
        backgroundSpriteRenderer = child.GetComponent<SpriteRenderer>();
        backgroundSpriteRenderer.sortingOrder = 31;
        backgroundSpriteRenderer.sprite = activeFoodBackground;
    }
}