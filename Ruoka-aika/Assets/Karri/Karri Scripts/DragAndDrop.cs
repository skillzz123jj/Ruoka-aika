using UnityEngine;
public class DragAndDrop : MonoBehaviour
{
    Vector3 offset;
    Vector2 initialPosition;
    Vector2 screenSize;

    bool isDragging;
    bool moveFood = true;

    [SerializeField] float moveSpeed;
    float boundaryOffsetHeight;
    float boundaryOffsetWidth;
    float objectWidth;
    float objectHeight;
    float objectX;
    float objectY;

    SpriteRenderer spriteRenderer;
    [SerializeField] ActiveFood activeFood;
    public static DragAndDrop dragAndDrop;


    private void Start()
    {
        //Checks the food has a child object and calculates it's sprite size 
        spriteRenderer = transform.GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            objectWidth = spriteRenderer.bounds.size.x / 2;
            objectHeight = spriteRenderer.bounds.size.y / 2;
        }
        //If game is running on mobile foods are bigger for better visibility
        if (Application.platform == RuntimePlatform.WebGLPlayer && Application.isMobilePlatform)
        {
            gameObject.transform.localScale = new Vector2(1.15f, 1.15f);

        }
    }
    public void OnMouseDown()
    {
        isDragging = true;
        moveFood = true;
        activeFood.isDragging = true;

    }

    public void OnMouseUp()
    {
        isDragging = false;
        activeFood.isDragging = false;
    }

    void Update()
    {
        //Checks if there are any changes to the screen size 
        if (screenSize.x != Screen.width || screenSize.y != Screen.height)
        {
            screenSize.x = Screen.width;
            screenSize.y = Screen.height;

            UpdateScreenSize();
        }
   
        //If player tries to drag food out of bounds its position gets locked on that axis
        if (isDragging)
        {
            //Mouse at left side
            if (Input.mousePosition.x <= 0)
            {
                if (Input.mousePosition.y <= 0 || Input.mousePosition.y >= Screen.height - boundaryOffsetHeight)
                {
                    //If the mouse is in the corner don't allow movement
                }
                else
                {
                    Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    mousePosition.x = -objectX;
                    transform.position = mousePosition;

                }
            }//Mouse at right side
            else if (Input.mousePosition.x >= Screen.width - boundaryOffsetWidth)
            {
                if (Input.mousePosition.y <= 0 || Input.mousePosition.y >= Screen.height - boundaryOffsetHeight)
                {

                }
                else
                {
                    Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    mousePosition.x = objectX;
                    transform.position = mousePosition;

                }
            }//Mouse at the top 
            else if (Input.mousePosition.y <= 0)
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.y = -objectY;
                transform.position = mousePosition;

            }//Mouse at the bottom
            else if (Input.mousePosition.y >= Screen.height - boundaryOffsetHeight)
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.y = objectY;
                transform.position = mousePosition;

            }
            else //Allow free movement
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                transform.position = mousePosition;

                GameObject currentActiveFood = activeFood.GetClickedFood(gameObject);
                activeFood.ValidFood(currentActiveFood);
            }

            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                activeFood.GetClickedFood(gameObject);
                activeFood.ValidFood(gameObject);

                //If user tries to drag food out of bounds food stops moving else allow free movement
                if (touch.position.x <= 0 || touch.position.x >= Screen.width - boundaryOffsetWidth ||
                    touch.position.y <= 0 || touch.position.y >= Screen.height - boundaryOffsetHeight)
                {
                  
                }
                else
                {
                    Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position) + offset;
                    transform.position = touchPosition;    
                }
          
            }
        }
        else //If player chooses another food the previous food goes back to its original position
        {
            if (moveFood && gameObject != ActiveFood.activeFood.currentActiveFood)
            {
                initialPosition = RandomAnimalAndFood.randomAnimalAndFood.FoodPositionDictionary[gameObject];

                float t = Time.deltaTime * moveSpeed;

                transform.position = Vector3.Lerp(transform.position, initialPosition, t);

            }
        }
    }
    //Updates screen boundaries so that 
    void UpdateScreenSize()
    {
        boundaryOffsetHeight = Screen.height * 0.05f;
        boundaryOffsetWidth = Screen.width * 0.025f;
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        Vector3 screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(screenWidth, screenHeight, 0));
        objectX = screenBounds.x - objectWidth;
        objectY = screenBounds.y - objectHeight;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        moveFood = false;

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        moveFood = true;
    }
}