using System.Collections;
using UnityEngine;



public class DragAndDrop : MonoBehaviour
{
    public GameObject currentActiveFood;
    public GameObject wrongFoodSprite;
    public bool foodWasFed;

    public float speed = 5.0f;
    public float moveSpeed = 3.0f;
   
    int currentAnimalIndex;
    int currentFoodIndex = 0;
   
    private bool isSelectingFood = true; 

    public bool wasChosen;
    public bool isMoving = false;
    bool isHovering;
    

    [SerializeField] Sprite activeFoodBackground;
    [SerializeField] Sprite actuallyActiveFoodBackground;
    [SerializeField] Sprite defaultBackground;

    SpriteRenderer backgroundSpriteRenderer;
    SpriteRenderer spriteRenderer;

    public GameObject previousActiveFood;
    public GameObject activeAnimal;
    public GameObject actuallyActive;
    public GameObject food;
    public GameObject highLight;

    Collider2D foodCollider;
    Vector2 position;
    RaycastHit2D hit;
    [SerializeField] Animator animator;

    public static DragAndDrop activeFood;
 
    void Awake()
    {
        // Check if an instance of this object already exists
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject); // Destroy the duplicate
        }
        else
        {
            DontDestroyOnLoad(gameObject); // Make the object persistent
        }
    }

    void ResetBool()
    {
        foodWasFed = false;
    }

    private void Start()
    {
        activeFood = this;
        previousActiveFood = currentActiveFood;
    }
   
    void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        if (hit.collider != null)
        {

            if (!isHovering)
            {
                HoverOnFood();
            }
            else
            {
                if (isHovering)
                {
                    OnHoverExit();
                }
                isHovering = false;
            }


        }
        if (Input.GetMouseButtonUp(0))
        {
            if (!isMoving)
            {
                foodWasFed = true;
                Invoke("ResetBool", 0.1f);
            }

        }


        HandleKeyboardInput();
        HandleMouseInput();

        if (currentActiveFood != null)
        {
            HandleMovement();
        }
    }

    void HandleKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isSelectingFood = !isSelectingFood;
            SwitchToNextFood();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            animator.SetTrigger("Valinta");
            if (currentActiveFood == null)
            {


                wasChosen = false;
            }
            else
            {
                wasChosen = true;
                ChooseFood(currentActiveFood);
                highLight.SetActive(true);
                highLight.transform.position = RandomAnimalAndFood.randomAnimalAndFood.chosenAnimals[0].transform.position;
            }


        }
    }

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (currentActiveFood)
            {
                wasChosen = false;
                highLight.SetActive(false);
            }
            GameObject newActiveFood = GetClickedFood();

            if (newActiveFood != null && newActiveFood != currentActiveFood && (newActiveFood.CompareTag("Food") || newActiveFood.CompareTag("EiSyötävä")))
            {

                ChooseFood(previousActiveFood);

                if (previousActiveFood != null)
                {
                    ChooseFood(previousActiveFood);
                }

                
                currentActiveFood = newActiveFood;
                previousActiveFood = newActiveFood;
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
        if (RandomAnimalAndFood.randomAnimalAndFood.chosenFoods.Count == 0 && 
            RandomAnimalAndFood.randomAnimalAndFood.chosenFoods != null &&
            RandomAnimalAndFood.randomAnimalAndFood.chosenFoods.Count > 0) 
            //NullReferenceException: Object reference not set to an instance of an object
            //DragAndDrop.SwitchToNextFood()(at Assets / Karri / Karri Scripts / DragAndDrop.cs:181)
            //DragAndDrop.HandleKeyboardInput()(at Assets / Karri / Karri Scripts / DragAndDrop.cs:115)
            //DragAndDrop.Update()(at Assets / Karri / Karri Scripts / DragAndDrop.cs:101)


        {
             if (currentActiveFood != null)
             {
                 ResetBackground(currentActiveFood);
             }


            currentFoodIndex = (currentFoodIndex + 1) % RandomAnimalAndFood.randomAnimalAndFood.chosenFoods.Count;

            currentActiveFood = RandomAnimalAndFood.randomAnimalAndFood.chosenFoods[currentFoodIndex];

            if (currentActiveFood != null)
            {
                ChangeBackground(currentActiveFood);
            }






        }

       

        // Increment the currentFoodIndex to switch to the next food
        currentFoodIndex = (currentFoodIndex + 1) % RandomAnimalAndFood.randomAnimalAndFood.chosenFoods.Count;

        currentActiveFood = RandomAnimalAndFood.randomAnimalAndFood.chosenFoods[currentFoodIndex];

        if (currentActiveFood != null)
        {
            ChangeBackground(currentActiveFood);
        }
    }

    void HoverOnFood()
    {
        food = hit.collider.gameObject;
        if (food != null && food.CompareTag("Food") || food.CompareTag("EiSyötävä"))
        {
            GameObject background = food.transform.GetChild(0).gameObject;
            backgroundSpriteRenderer = background.GetComponent<SpriteRenderer>();
            backgroundSpriteRenderer.sprite = activeFoodBackground;
        }
    }

    void OnHoverExit()
    {
        if (food != null && food.CompareTag("Food") || food.CompareTag("EiSyötävä"))
        {
            GameObject background = food.transform.GetChild(0).gameObject;
            backgroundSpriteRenderer = background.GetComponent<SpriteRenderer>();
            backgroundSpriteRenderer.sprite = defaultBackground;
        }

    }

    

    private void ResetBackground(GameObject food)
    {
        // Implement background reset logic here
        foodCollider = food.GetComponent<Collider2D>();
        foodCollider.isTrigger = false;
        spriteRenderer = food.GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = 30;
        GameObject child = food.transform.GetChild(0).gameObject;
        backgroundSpriteRenderer = child.GetComponent<SpriteRenderer>();
        backgroundSpriteRenderer.sortingOrder = 25;
        backgroundSpriteRenderer.sprite = defaultBackground;
    }

    private void ChangeBackground(GameObject food)
    {
        // Implement background change logic here
        foodCollider = food.GetComponent<Collider2D>();
        foodCollider.isTrigger = true;
        spriteRenderer = food.GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = 32;
        GameObject child = food.transform.GetChild(0).gameObject;
        backgroundSpriteRenderer = child.GetComponent<SpriteRenderer>();
        backgroundSpriteRenderer.sortingOrder = 31;
        backgroundSpriteRenderer.sprite = activeFoodBackground;
    }

    private void ChooseFood(GameObject food)
    {
        // Implement feeding logic here
        wasChosen = true;
        foodCollider = food.GetComponent<Collider2D>();
        foodCollider.isTrigger = true;
        spriteRenderer = food.GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = 32;
        GameObject child = food.transform.GetChild(0).gameObject;
        backgroundSpriteRenderer = child.GetComponent<SpriteRenderer>();
        backgroundSpriteRenderer.sortingOrder = 31;
        backgroundSpriteRenderer.sprite = actuallyActiveFoodBackground;
        actuallyActive = currentActiveFood;

    }

    void ChooseAnAnimal()
    {
        if (actuallyActive != null)
        {
            if (RandomAnimalAndFood.randomAnimalAndFood.chosenAnimals.Count == 0)
            {
                return;
            }

            currentAnimalIndex = (currentAnimalIndex + 1) % RandomAnimalAndFood.randomAnimalAndFood.chosenAnimals.Count;

            activeAnimal = RandomAnimalAndFood.randomAnimalAndFood.chosenAnimals[currentAnimalIndex];

            position = activeAnimal.transform.position;

            highLight.transform.position = position;

        }
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

    private IEnumerator MoveToPosition(GameObject activeFood, GameObject chosenAnimal)
    {
        currentAnimalIndex = 0;
        currentFoodIndex = -1;
        isMoving = true;
        RandomAnimalAndFood.randomAnimalAndFood.timerToChangeFood = 15;
        Vector3 targetPosition = chosenAnimal.transform.position;
        float offset = 2.5f;
        targetPosition.y -= offset;
        Vector3 initialPosition = activeFood.transform.position;
        float journeyLength = Vector3.Distance(initialPosition, targetPosition);
        float journeyDuration = journeyLength / moveSpeed;

        float startTime = Time.time;

        while (Time.time < startTime + journeyDuration)
        {
            float distanceCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;

            activeFood.transform.position = Vector3.Lerp(initialPosition, targetPosition, fractionOfJourney);

            yield return null;
        }

        activeFood.transform.position = targetPosition;

        activeAnimal = null;
        wasChosen = false;
        foodWasFed = true;
        highLight.SetActive(false);
        Invoke("ResetBool", 0.1f);
        currentActiveFood = null;
        isMoving = false;

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
