using Unity.VisualScripting;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private Vector3 offset;

    private bool isDragging;
    public bool move = true;

    public Vector2 initialPosition;
    public float moveSpeed = 3.0f;
    float boundaryOffsetHeight = Screen.height * 0.05f;
    float boundaryOffsetWidth = Screen.width * 0.025f;
    Vector3 mousePos;

    private Vector3 screenBounds;
    private float objectWidth;
    private float objectHeight;
    private SpriteRenderer spriteRenderer;

    float objectX;
    float objectY;


[SerializeField] ActiveFood activeFood;

    public static DragAndDrop dragAndDrop;
    private Vector2 mousePosition;

    private void Start()
    {
        spriteRenderer = transform.GetComponentInChildren<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            objectWidth = spriteRenderer.bounds.size.x / 2;
            objectHeight = spriteRenderer.bounds.size.y / 2;
        }
        if (Application.platform == RuntimePlatform.WebGLPlayer && Application.isMobilePlatform)
        {
            gameObject.transform.localScale = new Vector2(1.15f, 1.15f);

        }

     

    }
    public void OnMouseDown()
    {
        isDragging = true;
        move = true;
        activeFood.isDragging = true;

        if (Input.touchCount > 0)
        {
            offset = transform.position - Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
        }

    }

    public void OnMouseUp()
    {
        isDragging = false;
        activeFood.isDragging = false;
    }

    void Update()
    {
         boundaryOffsetHeight = Screen.height * 0.05f;
         boundaryOffsetWidth = Screen.width * 0.025f;
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        Vector3 screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(screenWidth, screenHeight, 0));
        objectX = screenBounds.x - objectWidth;
        objectY = screenBounds.y - objectHeight;



        if (isDragging)
        {
            if (Input.mousePosition.x <= 0)
            {

                if (Input.mousePosition.y <= 0 || Input.mousePosition.y >= Screen.height - boundaryOffsetHeight)
                {

                }
                else
                {
                    Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    mousePosition.x = -objectX;
                    transform.position = mousePosition;

                }
            }
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
            }
            else if (Input.mousePosition.y <= 0)
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.y = -objectY;
                transform.position = mousePosition;

            }
            else if (Input.mousePosition.y >= Screen.height - boundaryOffsetHeight)
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.y = objectY;
                transform.position = mousePosition;

            }
            else
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                transform.position = mousePosition;

                GameObject currentActiveFood = activeFood.GetClickedFood(gameObject);
                activeFood.ValidFood(currentActiveFood);
            }



            if (Input.touchCount > 0)
            {
                // Get touch input
                Touch touch = Input.GetTouch(0);
                activeFood.GetClickedFood(gameObject);
                activeFood.ValidFood(gameObject);

                // Check if the touch is within screen boundaries
                if (touch.position.x <= 0 || touch.position.x >= Screen.width - boundaryOffsetWidth ||
                    touch.position.y <= 0 || touch.position.y >= Screen.height - boundaryOffsetHeight)
                {
                    // Do nothing if touch is beyond screen boundaries
                }
                else
                {
                    // Move the object based on touch input
                    Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position) + offset;
                    transform.position = touchPosition;

                    // Optionally, perform other actions here
                }
                //else
                //{
                //    Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                //    transform.position = mousePosition;

                //    GameObject currentActiveFood = activeFood.GetClickedFood(gameObject);
                //    activeFood.ValidFood(currentActiveFood);
                //}
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
