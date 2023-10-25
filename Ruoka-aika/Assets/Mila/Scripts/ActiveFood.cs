using UnityEngine;
using System.Collections;

public class ActiveFood : MonoBehaviour
{
    [SerializeField] Sprite activeFoodBackground;
    [SerializeField] Sprite actuallyActiveFoodBackground;
    [SerializeField] Sprite defaultBackground;

    SpriteRenderer backgroundSpriteRenderer;
    SpriteRenderer spriteRenderer;

    public GameObject currentActiveFood;
    public GameObject wrongFoodSprite;
    public GameObject highLight;
    public GameObject food;
    GameObject previousActiveFood;
    GameObject activeAnimal;
    GameObject actuallyActive;

    public float moveSpeed = 3.0f;
    public float speed = 5.0f;

    int currentFoodIndex = 0;
    int currentAnimalIndex;

    public bool foodWasFed;
    public bool wasChosen;
    public bool isMoving = false;
    bool isHovering;

    Collider2D foodCollider;
    Vector2 position;
    RaycastHit2D hit;
    [SerializeField] Animator animator;

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
        //Convert the mouse position to world coordinates
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //Create a raycast hit variable to store information about the hit
        hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        //Check if the ray hits a collider
        if (hit.collider != null)
        {
            if (!isHovering)
            {
                HoverOnFood();
            }

            isHovering = true;
        }
        else
        {
            if (isHovering)
            {
                OnHoverExit();
            }
            isHovering = false;
        }

        //Cancel animal selection 
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            wasChosen = false;
            highLight.SetActive(false);
            SwitchToNextFood();
        }

        //Choose a food using space then select using enter
        //After that choose an animal using space then feed the chosen food using enter
        if (!wasChosen)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SwitchToNextFood();
            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
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
        else if (currentActiveFood)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (!isMoving)
                {
                    ChooseAnAnimal();
                }

            }

            if (Input.GetKeyDown(KeyCode.Return) && !isMoving)
            {
                animator.SetTrigger("Valinta");
                if (activeAnimal == null)
                {
                    activeAnimal = RandomAnimalAndFood.randomAnimalAndFood.chosenAnimals[0];
                }
                StartCoroutine(MoveToPosition(actuallyActive, activeAnimal));
                activeAnimal = RandomAnimalAndFood.randomAnimalAndFood.chosenAnimals[0];
            }
        }

        //This makes sure that the food can be fed 
        if (Input.GetMouseButtonUp(0))
        {
            if (!isMoving)
            {
                foodWasFed = true;
                Invoke("ResetBool", 0.1f);
            }
           
        }

        //Check if a new food is clicked by the player
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

                //Update the active food and the previous active food
                currentActiveFood = newActiveFood;
                previousActiveFood = newActiveFood;
            }
        }
    }

    //Changes foods background sprite if hovered on 
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
    //Changes it back to normal
    void OnHoverExit()
    {
        if (food != null && food.CompareTag("Food") || food.CompareTag("EiSyötävä"))
        {
            GameObject background = food.transform.GetChild(0).gameObject;
            backgroundSpriteRenderer = background.GetComponent<SpriteRenderer>();
            backgroundSpriteRenderer.sprite = defaultBackground;
        }
    }
    private GameObject GetClickedFood()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider != null && food.CompareTag("Food") || food.CompareTag("EiSyötävä"))
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
                ChooseFood(currentActiveFood);
            }
            RandomAnimalAndFood.randomAnimalAndFood.TimerManager();
            return clickedFood;
        }

        return null;
    }

    //Allows the user to choose foods using space button
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

        currentFoodIndex = (currentFoodIndex + 1) % RandomAnimalAndFood.randomAnimalAndFood.chosenFoods.Count;

        currentActiveFood = RandomAnimalAndFood.randomAnimalAndFood.chosenFoods[currentFoodIndex];

        if (currentActiveFood != null)
        {
            ChangeBackground(currentActiveFood);
        }
    }
    //Resets the food's background sprite when new food was chosen 
    public void ResetBackground(GameObject food)
    {
        foodCollider = food.GetComponent<Collider2D>();
        foodCollider.isTrigger = false;
        spriteRenderer = food.GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = 30;
        GameObject child = food.transform.GetChild(0).gameObject;
        backgroundSpriteRenderer = child.GetComponent<SpriteRenderer>();
        backgroundSpriteRenderer.sortingOrder = 25;
        backgroundSpriteRenderer.sprite = defaultBackground;
    }
    //Changes the food's background sprite if chosen via space bar 
    void ChangeBackground(GameObject food)
    {
        foodCollider = food.GetComponent<Collider2D>();
        foodCollider.isTrigger = true;
        spriteRenderer = food.GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = 32;
        GameObject child = food.transform.GetChild(0).gameObject;
        backgroundSpriteRenderer = child.GetComponent<SpriteRenderer>();
        backgroundSpriteRenderer.sortingOrder = 31;
        backgroundSpriteRenderer.sprite = activeFoodBackground;
    }

    //Changes the food's background sprite to an active/chosen one if food is clicked or chosen with Enter button
    private void ChooseFood(GameObject food)
    {
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

    //Allows the user to choose animals using space button
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

    //This moves the food to the chosen animal and feeds itself
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