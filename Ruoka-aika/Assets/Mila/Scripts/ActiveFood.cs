using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveFood : MonoBehaviour
{
    public bool foodWasFed;

    public static ActiveFood activeFood;

    void Start()
    {
        activeFood = this;
    }

    void Update()
    {
        //This makes sure that the food can be fed 
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonUp(0))
        {
            foodWasFed = true;
            Invoke("resetBool", 0.2f);
        }
    }

    void resetBool()
    {
        foodWasFed = false;
    }
}
