using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

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

            if (allowedFoods.Contains(foodThatCollidedName))
            {
                Debug.Log(gameObject.name + " can eat " + foodThatCollidedName);
                //Chooses a new food
                RandomAnimalAndFood.randomAnimalAndFood.RandomFood(RandomAnimalAndFood.randomAnimalAndFood.numberOfFoodsToChoose);
                RandomAnimalAndFood.randomAnimalAndFood.RandomCorrectAnimal();
                RandomAnimalAndFood.randomAnimalAndFood.timerToChangeFood = 10;

            }
            else
            {
                Debug.Log(gameObject.name + " cannot eat " + foodThatCollidedName);
                RandomAnimalAndFood.randomAnimalAndFood.RandomFood(RandomAnimalAndFood.randomAnimalAndFood.numberOfFoodsToChoose);
                RandomAnimalAndFood.randomAnimalAndFood.RandomCorrectAnimal();
                RandomAnimalAndFood.randomAnimalAndFood.timerToChangeFood = 10;

            }
        }
    }

}



