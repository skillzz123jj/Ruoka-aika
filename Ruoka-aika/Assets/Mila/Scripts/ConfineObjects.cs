using UnityEngine;

public class ConfineObjects : MonoBehaviour
{
    private Vector3 screenBounds;
    private float _objectWidth;
    private float _objectHeight;
    private SpriteRenderer spriteRenderer;

    [SerializeField] ActiveFood activeFood;

    void Start()
    {
        //Finds the objects backgrounds boundaries to check position
        spriteRenderer = transform.GetComponentInChildren<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            _objectWidth = spriteRenderer.bounds.size.x / 2;
            _objectHeight = spriteRenderer.bounds.size.y / 2;
        }
    }

    void Update()
    {
        //Clamps the food position to make sure it doesnt go off screen
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        Vector3 viewPos = transform.position;
        viewPos.x = Mathf.Clamp(viewPos.x, screenBounds.x * -1 + _objectWidth, screenBounds.x - _objectWidth);
        viewPos.y = Mathf.Clamp(viewPos.y, screenBounds.y * -1 + _objectHeight, screenBounds.y - _objectHeight);
        transform.position = viewPos;

    }
}

