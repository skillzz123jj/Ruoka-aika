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

   [SerializeField] ActiveFood activeFood;

    public static DragAndDrop dragAndDrop;
    private Vector2 mousePosition;

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
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
   

        if (isDragging)
        {
            if (Input.mousePosition.x <= 0 || Input.mousePosition.x >= Screen.width - boundaryOffsetWidth)
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.x = transform.position.x;
                transform.position = mousePosition;
                
            }
            else if (Input.mousePosition.y <= 0 || Input.mousePosition.y >= Screen.height - boundaryOffsetHeight)
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.y = transform.position.y;
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
                //Gives the food an offset when player is using a mobile device
                Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position) + offset;


                transform.position = touchPosition;//new Vector3(touchPosition.x - 1f, touchPosition.y + 1f, touchPosition.z);

                activeFood.GetClickedFood(gameObject);
                activeFood.ValidFood(gameObject);

            }
            //else
            //{
            //    Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //    transform.position = mousePosition;

            //    GameObject currentActiveFood = activeFood.GetClickedFood(gameObject);
            //    activeFood.ValidFood(currentActiveFood);
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
