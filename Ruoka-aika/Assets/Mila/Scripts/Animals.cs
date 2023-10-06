using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animals : MonoBehaviour
{
    [SerializeField] Animator animExpression;
    [SerializeField] Animator animTail;
    [SerializeField] AudioSource chewingSound;
    [SerializeField] AudioSource errorSound;

    GameObject foodThatCollided;
    GameObject badFood;
    GameObject error;
    string foodThatCollidedName;

    float shrinkSpeed = 0.7f;
    public bool good;
    public bool bad;
    bool isShrinking;

    Queue<GameObject> foodQueue = new Queue<GameObject>();

    //Checks for collisions and if the animal is allowed to eat that food
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Food") || collision.CompareTag("EiSyötävä"))
        {
            //Finds the foods name that collided with the animal
            foodThatCollided = collision.gameObject;
            foodThatCollidedName = foodThatCollided.name;

            /*So here it first checks if the animal is even in that dictionary if not well wrong food anyway,
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
                if (chewingSound != null)
                {
                    chewingSound.Play();
                }
               // ActiveFood.activeFood.ResetBackground(foodThatCollided);
                GoodFood();
                good = false;

            }
            else if (bad)
            {
                //ActiveFood.activeFood.ResetBackground(foodThatCollided);
                errorSound.Play();
                BadFood();
                bad = false;

            }
        }
    }

    //Spawns new foods when the old ones have been fed  
    public void NewFoods()
    {
        RandomAnimalAndFood.randomAnimalAndFood.RandomFood(RandomAnimalAndFood.randomAnimalAndFood.numberOfFoodsToChoose);
        RandomAnimalAndFood.randomAnimalAndFood.RandomCorrectAnimal();
        RandomAnimalAndFood.randomAnimalAndFood.foodsLeft = RandomAnimalAndFood.randomAnimalAndFood.numberOfFoodsToChoose;
        RandomAnimalAndFood.randomAnimalAndFood.timerToChangeFood = 10;
    }

    void GoodFood()
    {
        //If the animal is allowed to eat the food and the score goes up
        Debug.Log($"{gameObject.name} saa syödä {foodThatCollidedName}");
        RandomAnimalAndFood.randomAnimalAndFood.chosenFoods.Remove(foodThatCollided);
        if (animExpression != null)
        {
            animExpression.SetTrigger("Iloinen");
            animTail.SetTrigger("Häntä");

        }
        if (isShrinking)
        {
            //If numerous foods were fed to one animal the foods wait in a queue till its their turn to shrink
            foodQueue.Enqueue(foodThatCollided);
        }
        else
        {   
            StartCoroutine(ShrinkFood(foodThatCollided));
        }

        Score.scoreScript.ScoreUp();
    }
  
    private IEnumerator ShrinkFood(GameObject goodFood)
    {
        isShrinking = true;
        var script = goodFood.GetComponent<DragAndDrop>();
        script.enabled = false; 
        
        Vector3 initialScale = goodFood.transform.localScale;
        RandomAnimalAndFood.randomAnimalAndFood.timerToChangeFood = 10;
        RandomAnimalAndFood.randomAnimalAndFood.foodsLeft--;

        //Pauses the code while the food shrinks
        while (goodFood.transform.localScale.x > 0.0f)
        {
            goodFood.transform.localScale -= Vector3.one * shrinkSpeed * Time.deltaTime;

            yield return null;
        }
    

        //Resets the food for future selection
        script.enabled = true;
        goodFood.transform.position = new Vector2(0, -20);
        goodFood.SetActive(false);
        goodFood.transform.localScale = initialScale;
        isShrinking = false;

        HandleChanges();

        //Keeps calling this function till the queue is empty
        if (foodQueue.Count > 0)
        {
            var nextFood = foodQueue.Dequeue();
            StartCoroutine(ShrinkFood(nextFood));
        }
    }

    void BadFood()
    {
        //If the food was fed to the wrong animal player loses a life
        Debug.Log($"{gameObject.name} ei saa syödä {foodThatCollidedName}");
        badFood = foodThatCollided;
        RandomAnimalAndFood.randomAnimalAndFood.chosenFoods.Remove(badFood);
        var script = badFood.GetComponent<DragAndDrop>();
        script.enabled = false;
        if (animExpression != null)
        {
            animExpression.SetTrigger("Surullinen");
        }
        if (foodThatCollided.CompareTag("EiSyötävä"))
        {
            RandomAnimalAndFood.randomAnimalAndFood.chosenFoods.Remove(badFood);
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
        error = Instantiate(ActiveFood.activeFood.wrongFoodSprite, foodThatCollided.transform.position, Quaternion.identity);
        Invoke("DestroySprite", 1.5f);
    }

    //This one gets rid of the sprite and the food after a slight delay
    void DestroySprite()
    {
        badFood.SetActive(false);
        Destroy(error);
        var script = badFood.GetComponent<DragAndDrop>();
        script.enabled = true;
    }

    //If there are no more foods this one handles that
    void HandleChanges()
    {
        if (RandomAnimalAndFood.randomAnimalAndFood.foodsLeft == 0 && !RandomAnimalAndFood.randomAnimalAndFood.changedFoodsRecently)
        {
            RandomAnimalAndFood.randomAnimalAndFood.changedFoodsRecently = true;
            //This makes sure that animals only get swapped when there arent any foods 
            if (RandomAnimalAndFood.randomAnimalAndFood.nowIsAGoodTime)
            {
                RandomAnimalAndFood.randomAnimalAndFood.nowIsAGoodTime = false;
                RandomAnimalAndFood.randomAnimalAndFood.CanChangeAnimal();
            }
            else
            {
                Invoke("NewFoods", 3F);
                ActiveFood.activeFood.SwitchToNextFood();
            }
        }
    }
}





