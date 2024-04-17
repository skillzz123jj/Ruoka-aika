using UnityEngine;

public class ConfineObjects : MonoBehaviour
{
    private Vector3 screenBounds;
    private float _objectWidth;
    private float _objectHeight;

    [SerializeField] ActiveFood activeFood;

    void Start()
    {
        
        _objectWidth = transform.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        _objectHeight = transform.GetComponent<SpriteRenderer>().bounds.size.y / 2;
    }

    void Update()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        Vector3 viewPos = transform.position;
        viewPos.x = Mathf.Clamp(viewPos.x, screenBounds.x * -1 + _objectWidth, screenBounds.x - _objectWidth);
        viewPos.y = Mathf.Clamp(viewPos.y, screenBounds.y * -1 + _objectHeight, screenBounds.y - _objectHeight);
        transform.position = viewPos;
       
   }

}