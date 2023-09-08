using UnityEngine;

public class InputManager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null && hit.collider.CompareTag("Food"))
            {
                FoodController food = hit.collider.gameObject.GetComponent<FoodController>();

                if (food != null)
                {
                    food.PickUp();
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null && hit.collider.CompareTag("Food"))
            {
                FoodController food = hit.collider.gameObject.GetComponent<FoodController>();

                if (food != null)
                {
                    food.Release();
                }
            }
        }
    }
}
