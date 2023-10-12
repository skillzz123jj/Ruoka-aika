using UnityEngine;
using System.Collections.Generic;

public class SelectionManager : MonoBehaviour
{
    public GameObject foodBackground;
    public GameObject animalBackground;

    private bool isSelectingFood = true;
    private GameObject selectedObject;

    private Dictionary<GameObject, Vector2> foodPositionDictionary;
    private Dictionary<GameObject, Vector2> animalPositionDictionary;

    private void Start()
    {
        RandomAnimalAndFood randomAnimalAndFood = FindObjectOfType<RandomAnimalAndFood>();

        if (randomAnimalAndFood != null)
        {
            foodPositionDictionary = randomAnimalAndFood.FoodPositionDictionary;
            
        }
        else
        {
            Debug.LogError("RandomAnimalAndFood script not found in the scene.");
        }

        foodBackground.SetActive(true);
        animalBackground.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isSelectingFood = !isSelectingFood;
            
            // Update the background objects.
            foodBackground.SetActive(isSelectingFood);
            animalBackground.SetActive(!isSelectingFood);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Handle object selection logic here.
            selectedObject = SelectClosestObject();
        }
    }

    private GameObject SelectClosestObject()
    {
        GameObject[] objects = GameObject.FindObjectsOfType<GameObject>();
        float closestDistance = Mathf.Infinity;
        GameObject closestObject = null;

        foreach (GameObject obj in objects)
        {
            ActiveFood foodComponent = obj.GetComponent<ActiveFood>();
            Animals animalComponent = obj.GetComponent<Animals>();

            if ((isSelectingFood && foodComponent != null) || (!isSelectingFood && animalComponent != null))
            {
                float distance = Vector2.Distance(obj.transform.position, transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestObject = obj;
                }
            }
        }

        return closestObject;
    }
}




