using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animals : MonoBehaviour
{
    //Checks for collisions and if the animal is allowed to eat that food
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Finds the foods name that collided with the animal
        GameObject foodThatCollided = collision.gameObject;
        string foodThatCollidedName = foodThatCollided.name;

        //Checks if the animal dictionary contains the food that the animal collided with
        if (RandomAnimalAndFood.randomAnimalAndFood.animalToFoodMap.ContainsKey(gameObject.name))
        {
            //Checks if the animal is allowed to eat the food
            List<string> allowedFoods = RandomAnimalAndFood.randomAnimalAndFood.animalToFoodMap[gameObject.name];

            //Determines wether the food was in that animals dictionary or not 
            if (allowedFoods.Contains(foodThatCollidedName))
            {
                Debug.Log(gameObject.name + " saa syödä " + foodThatCollidedName);
                //Chooses a new food
                RandomAnimalAndFood.randomAnimalAndFood.RandomFood();
                //Chooses a new animal
                RandomAnimalAndFood.randomAnimalAndFood.RandomCorrectAnimal();

            }
            else
            {
                Debug.Log(gameObject.name + " ei saa syödä " + foodThatCollidedName);
                RandomAnimalAndFood.randomAnimalAndFood.RandomFood();
                RandomAnimalAndFood.randomAnimalAndFood.RandomCorrectAnimal();

            }
        }
    }
}
