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
        if (RandomAnimalAndFood.randomAnimalAndFood.foodsForAnimalsMap.ContainsKey(gameObject.name))
        {
            //Checks if the animal is allowed to eat the food
            string allowedFoods = RandomAnimalAndFood.randomAnimalAndFood.foodsForAnimalsMap[gameObject.name];

            if (allowedFoods.Contains(foodThatCollidedName))
            {
                Debug.Log(gameObject.name + " saa syödä " + foodThatCollidedName);
                //Chooses a new food
                Score.scoreScript.ScoreUp();
                RandomAnimalAndFood.randomAnimalAndFood.RandomFood(RandomAnimalAndFood.randomAnimalAndFood.numberOfFoodsToChoose);
                RandomAnimalAndFood.randomAnimalAndFood.RandomCorrectAnimal();
                RandomAnimalAndFood.randomAnimalAndFood.timerToChangeFood = 10;
            }
        }
        else
        {
           
            Debug.Log(gameObject.name + " ei saa syödä " + foodThatCollidedName);
            RandomAnimalAndFood.randomAnimalAndFood.RandomFood(RandomAnimalAndFood.randomAnimalAndFood.numberOfFoodsToChoose);
            RandomAnimalAndFood.randomAnimalAndFood.RandomCorrectAnimal();
            RandomAnimalAndFood.randomAnimalAndFood.timerToChangeFood = 10;
        }
    }
}



