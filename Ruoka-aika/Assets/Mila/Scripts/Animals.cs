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
    int amountOfGoodFoods;
    int amountOfBadFoods;

    public bool good;
    public bool bad;
    public bool isShrinking;

    [SerializeField] RandomAnimalAndFood randomAnimalAndFood;
    [SerializeField] ActiveFood activeFood;
    [SerializeField] Score score;

    public static Animals animals; 

    Queue<GameObject> foodQueue = new Queue<GameObject>();

    //Checks for collisions and if the animal is allowed to eat that food
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Food") || collision.CompareTag("EiSyötävä"))
        {
            //Finds the foods name that collided with the animal
            foodThatCollided = collision.gameObject;
            foodThatCollidedName = foodThatCollided.name;

            if (!activeFood.isMoving)
            {
                activeFood.highlight.SetActive(true);
                activeFood.highlight.transform.position = gameObject.transform.position;

            }
         
            /*So here it first checks if the animal is even in that dictionary if not well wrong food anyway,
            if however the animal is in that dictionary it adds the animals foods in a list and then checks if 
            if the animal can eat the collided food or not*/
            if (randomAnimalAndFood.TempDictionary.ContainsKey(gameObject.name))
            {
                List<string> allowedFoods = randomAnimalAndFood.TempDictionary[gameObject.name];

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
        if (activeFood.foodWasFed)
        {
            if (good)
            { 
                if (chewingSound != null)
                {
                    chewingSound.Play();
                }
                activeFood.wasChosen = false;
                activeFood.ResetBackground(foodThatCollided);
                GoodFood();
                good = false;
                activeFood.animator.SetTrigger("Valinta");
                activeFood.currentFoodIndex = -1;
                activeFood.SwitchToNextFood();

            }
            else if (bad)
            {
                activeFood.wasChosen = false;
                activeFood.ResetBackground(foodThatCollided);
                errorSound.Play();
                BadFood();
                bad = false;
                activeFood.animator.SetTrigger("Valinta");
                activeFood.currentFoodIndex = -1;
                activeFood.SwitchToNextFood();

            }
        }
    }

    //Spawns new foods when the old ones have been fed  
    public void NewFoods()
    {    
        if (randomAnimalAndFood.enabled)
        {
            randomAnimalAndFood.RandomFood(randomAnimalAndFood.numberOfFoodsToChoose, randomAnimalAndFood.numberOfAllowedBadFoods);
            randomAnimalAndFood.RandomCorrectAnimal();
            randomAnimalAndFood.foodsLeft = randomAnimalAndFood.numberOfFoodsToChoose;
            randomAnimalAndFood.TimerManager();

        }
           
    }

    void GoodFood()
    {
        //If the animal is allowed to eat the food and the score goes up
        randomAnimalAndFood.chosenFoods.Remove(foodThatCollided);
        animTail.SetTrigger("Häntä");
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
    
        score.ScoreUp();
    }
  
    private IEnumerator ShrinkFood(GameObject goodFood)
    {
        isShrinking = true;
        var script = goodFood.GetComponent<DragAndDrop>();
        script.enabled = false; 
        
        Vector3 initialScale = goodFood.transform.localScale;
        randomAnimalAndFood.timerToChangeFood += 5;
        randomAnimalAndFood.foodsLeft--;

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
        badFood = foodThatCollided;
        randomAnimalAndFood.chosenFoods.Remove(badFood);
        var script = badFood.GetComponent<DragAndDrop>();
        script.enabled = false;
        if (animExpression != null)
        {
            animExpression.SetTrigger("Surullinen");
        }
        if (foodThatCollided.CompareTag("EiSyötävä"))
        {
            randomAnimalAndFood.chosenFoods.Remove(badFood);
        }

        randomAnimalAndFood.foodsLeft--;
        randomAnimalAndFood.TimerManager(); 
        score.WrongFood();
        WrongFoodSprite();
        HandleChanges();
   
    }

    //This method adds a sprite on the food if it was fed to the incorrect animal
    void WrongFoodSprite()
    {
        error = Instantiate(activeFood.wrongFoodSprite, foodThatCollided.transform.position, Quaternion.identity);
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
        activeFood.highlight.SetActive(false);

        //Checks if there are only bad foods in the scene and lowers the timer to swap foods
        foreach (GameObject food in randomAnimalAndFood.chosenFoods)
            {
                if (food.CompareTag("Food"))
                {
               
                    amountOfGoodFoods++;
                }
                else
                {
                    amountOfBadFoods++;
                   
                }
            }
            if (amountOfGoodFoods <= 0 && amountOfBadFoods > 0)
            {
                randomAnimalAndFood.timerToChangeFood = 2f;
            }

            amountOfBadFoods = 0;
            amountOfGoodFoods = 0;
        

        if (randomAnimalAndFood.foodsLeft == 0 && !randomAnimalAndFood.changedFoodsRecently)
        {
            randomAnimalAndFood.changedFoodsRecently = true;
            //This makes sure that animals only get swapped when there arent any foods 
            if (randomAnimalAndFood.nowIsAGoodTime)
            {
                randomAnimalAndFood.nowIsAGoodTime = false;
                //randomAnimalAndFood.timerToChangeFood = 2;
                StartCoroutine(randomAnimalAndFood.CanChangeAnimal(2f));
            }
            else
            {
                Invoke("NewFoods", 3F);               
            }
        }
    }
}





