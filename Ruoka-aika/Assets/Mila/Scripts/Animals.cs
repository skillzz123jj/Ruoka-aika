using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animals : MonoBehaviour
{
    [SerializeField] Animator animExpression;
    [SerializeField] Animator animTail;

    GameObject foodThatCollided;
    GameObject badFood;
    GameObject goodFood;
    string foodThatCollidedName;

    float shrinkSpeed = 0.7f;
    public bool good;
    public bool bad;

    //Checks for collisions and if the animal is allowed to eat that food
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Food") || collision.CompareTag("EiSyötävä"))
        {
            //Finds the foods name that collided with the animal
            foodThatCollided = collision.gameObject;
            foodThatCollidedName = foodThatCollided.name;

            /*So here it first checks if the animal is even in that dictionary if not well wrong food anyway
            if however the animal is in that dictionary it adds the animals foods in a list and then checks if 
            if the animal can eat the collided food or not*/
            if (RandomAnimalAndFood.randomAnimalAndFood.TempDictionary.ContainsKey(gameObject.name))
            {
                List<string> allowedFoods = RandomAnimalAndFood.randomAnimalAndFood.TempDictionary[gameObject.name];

                //This checks if the animal can eat the food and sets the bools accordingly
                if (allowedFoods.Contains(foodThatCollidedName))
                {               
                    good = true;
                    bad = false;
                }
                else
                {
                    bad = true;
                    good = false;
                }
            }
            else
            {
                bad = true;
                good = false;
            }

        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        //Resets the food that collided if it isnt on any animal
        foodThatCollided = null;
        foodThatCollidedName = null;
        good = false;
        bad = false;
    }

    private void Update()
    {
        if (ActiveFood.activeFood.foodWasFed)
        {
            if (good)
            {              
                GoodFood();
                good = false;

            }
            else if (bad)
            {              
                BadFood();
                bad = false;

            }
        }
    }

    public void NewFoods()
    {
        //Spawns new foods when the old ones have been fed       
        RandomAnimalAndFood.randomAnimalAndFood.RandomFood(RandomAnimalAndFood.randomAnimalAndFood.numberOfFoodsToChoose);
        RandomAnimalAndFood.randomAnimalAndFood.RandomCorrectAnimal();
        RandomAnimalAndFood.randomAnimalAndFood.foodsLeft = RandomAnimalAndFood.randomAnimalAndFood.numberOfFoodsToChoose;
    }

    void GoodFood()
    {
        //If the animal is allowed to eat the food and the score goes up
        Debug.Log($"{gameObject.name} saa syödä {foodThatCollidedName}");
        goodFood = foodThatCollided;
        if (animExpression != null)
        {
            animExpression.SetTrigger("Iloinen");
            animTail.SetTrigger("Häntä");

        }
        StartCoroutine(ShrinkFood());
        Score.scoreScript.ScoreUp();
       // RandomAnimalAndFood.randomAnimalAndFood.timerToChangeFood = 10;
       // foodThatCollided.transform.position = new Vector2(0, -20);
        //foodThatCollided.SetActive(false);
        //RandomAnimalAndFood.randomAnimalAndFood.foodsLeft--;
      //  HandleChanges();
    }
  
    private IEnumerator ShrinkFood()
    {
        var disable = goodFood.GetComponent<DragAndDrop>();
        disable.enabled = false;    
        Vector3 initialScale = goodFood.transform.localScale;
       // GameObject food = goodFood;
        RandomAnimalAndFood.randomAnimalAndFood.timerToChangeFood = 10;
        RandomAnimalAndFood.randomAnimalAndFood.foodsLeft--;
      

        while (goodFood.transform.localScale.x > 0.0f)
        {
            goodFood.transform.localScale -= Vector3.one * shrinkSpeed * Time.deltaTime;

            yield return null;
        }
        HandleChanges();

        disable.enabled = true;

        goodFood.transform.position = new Vector2(0, -20);
        goodFood.SetActive(false);
        goodFood.transform.localScale = initialScale;

    }

    void BadFood()
    {
        //If the food was fed to the wrong animal player loses a life
        Debug.Log($"{gameObject.name} ei saa syödä {foodThatCollidedName}");
        badFood = foodThatCollided;
        var script = badFood.GetComponent<DragAndDrop>();
        script.enabled = false;
        if (animExpression != null)
        {
            animExpression.SetTrigger("Surullinen");
        }
        if (foodThatCollided.CompareTag("EiSyötävä"))
        {
            RandomAnimalAndFood.randomAnimalAndFood.chosenFoods.Remove(foodThatCollided);
        }

        RandomAnimalAndFood.randomAnimalAndFood.foodsLeft--;
        RandomAnimalAndFood.randomAnimalAndFood.timerToChangeFood = 10;  
        Score.scoreScript.WrongFood();
        WrongFoodSprite();
        HandleChanges();
        
    }

    //This method adds a sprite on the food if it was fed to the incorrect animal
    void WrongFoodSprite()
    {       
        ActiveFood.activeFood.wrongFoodSprite.transform.position = foodThatCollided.transform.position;
        ActiveFood.activeFood.wrongFoodSprite.SetActive(true);
        Invoke("ResetSprite", 1.5f);
    }

    //This one gets rid of the sprite and the food after a slight delay
    void ResetSprite()
    {
        ActiveFood.activeFood.wrongFoodSprite.SetActive(false); 
        badFood.SetActive(false);
        ActiveFood.activeFood.wrongFoodSprite.transform.position = new Vector2(0, -20);
        var script = badFood.GetComponent<DragAndDrop>();
        script.enabled = true;

    }

    //If there are no more foods this one gives more foods
    void HandleChanges()
    {
        if (RandomAnimalAndFood.randomAnimalAndFood.foodsLeft == 0)
        {
            //This changes animals 
            if (RandomAnimalAndFood.randomAnimalAndFood.nowIsAGoodTime)
            {
                //A slight delay to make it more smooth
                RandomAnimalAndFood.randomAnimalAndFood.smoke.SetActive(true);
                Invoke("ChangeAnimalWithADelay", 2.0f);
            }
            else
            {
                Invoke("NewFoods", 3F);
            }
        }
    }
    //Changes the animal when the player isnt feeding any
    void ChangeAnimalWithADelay()
    {
        RandomAnimalAndFood.randomAnimalAndFood.timerToChangeFood = 100;
        RandomAnimalAndFood.randomAnimalAndFood.CanChangeAnimal();
        RandomAnimalAndFood.randomAnimalAndFood.nowIsAGoodTime = false;

    }
}





