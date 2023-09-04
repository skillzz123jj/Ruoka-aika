using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animals : MonoBehaviour
{
    [SerializeField] Animator animExpression;
    [SerializeField] Animator animTail;

    //Checks for collisions and if the animal is allowed to eat that food
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Finds the foods name that collided with the animal
        GameObject foodThatCollided = collision.gameObject;
        string foodThatCollidedName = foodThatCollided.name;

        //Checks if the food dictionary contains the animal that the food collided with
        if (RandomAnimalAndFood.randomAnimalAndFood.foodsForAnimalsMap.ContainsKey(gameObject.name))
        {
            string allowedFoods = RandomAnimalAndFood.randomAnimalAndFood.foodsForAnimalsMap[gameObject.name];

            if (allowedFoods.Contains(foodThatCollidedName))
            {
                //If the animal is allowed to eat the food and the score goes up
                Debug.Log($"{gameObject.name} saa syödä {foodThatCollidedName}");
                if (animExpression != null)
                {
                    animExpression.SetTrigger("Iloinen");
                    animTail.SetTrigger("Häntä");

                }               
                Score.scoreScript.ScoreUp();
                RandomAnimalAndFood.randomAnimalAndFood.timerToChangeFood = 10;
                foodThatCollided.SetActive(false);
                RandomAnimalAndFood.randomAnimalAndFood.foodsLeft--;
               
            }
        }
        else
        {
            //If it was fed to the wrong animal player loses a life
            Debug.Log($"{gameObject.name} ei saa syödä {foodThatCollidedName}");
            if (animExpression != null)
            {
                animExpression.SetTrigger("Surullinen");
            }
          
            RandomAnimalAndFood.randomAnimalAndFood.foodsLeft--;
            RandomAnimalAndFood.randomAnimalAndFood.timerToChangeFood = 10;
            foodThatCollided.SetActive(false);
            Score.scoreScript.WrongFood();        

        }

        if (RandomAnimalAndFood.randomAnimalAndFood.foodsLeft <= 0)
        {        
            StartCoroutine(delayFoods(1.0f));
        }
    }

    //Spawns new foods with a slight delay to slow down the game 
    public IEnumerator delayFoods(float time)
    {
        yield return new WaitForSeconds(time);

        RandomAnimalAndFood.randomAnimalAndFood.RandomFood(RandomAnimalAndFood.randomAnimalAndFood.numberOfFoodsToChoose);
        RandomAnimalAndFood.randomAnimalAndFood.RandomCorrectAnimal();
        RandomAnimalAndFood.randomAnimalAndFood.foodsLeft = RandomAnimalAndFood.randomAnimalAndFood.numberOfFoodsToChoose;

    }
}






