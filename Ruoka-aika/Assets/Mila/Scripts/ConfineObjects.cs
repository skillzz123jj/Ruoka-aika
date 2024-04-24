using UnityEngine;

public class ConfineObjects : MonoBehaviour
{
    private Vector3 screenBounds;
    private float objectWidth;
    private float objectHeight;
    private SpriteRenderer spriteRenderer;

    [SerializeField] ActiveFood activeFood;

    void Start()
    {
        //Finds the objects backgrounds boundaries to check position
        spriteRenderer = transform.GetComponentInChildren<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            objectWidth = spriteRenderer.bounds.size.x / 2;
            objectHeight = spriteRenderer.bounds.size.y / 2;
        }
    }

    void Update()
    {
        //Clamps the food position to make sure it doesnt go off screen
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        Vector3 foodPosition = transform.position;
        foodPosition.x = Mathf.Clamp(foodPosition.x, screenBounds.x * -1 + objectWidth, screenBounds.x - objectWidth);
        foodPosition.y = Mathf.Clamp(foodPosition.y, screenBounds.y * -1 + objectHeight, screenBounds.y - objectHeight);
        transform.position = foodPosition;

    }
}

