﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAnimalAndFood : MonoBehaviour
{
    public List<GameObject> animals = new List<GameObject>();
    public List<GameObject> chosenAnimals = new List<GameObject>();
    public List<GameObject> animalsThatWerentChosen = new List<GameObject>();
    [SerializeField] List<GameObject> foods = new List<GameObject>();
    [SerializeField] List<GameObject> allFoods = new List<GameObject>();

    public Transform lineStart;
    public Transform lineEnd;
    public Transform lineStartFood;
    public Transform lineEndFood;

    public float timerToChangeFood = 10;
    public float timerToChangeAnimal = 20;
    int numAnimalsToChoose = 4;

    GameObject randomFood;
    GameObject randomFoodPrevious;
    GameObject correctRandomAnimal;
    GameObject animalThatGetsSwapped;

    private bool isFoodChanging = false;
    private bool timeForAChange;

    //Allows other scripts to access this one
    public static RandomAnimalAndFood randomAnimalAndFood;

    //Dictionary to map animal names to their corresponding food items
    public Dictionary<string, List<string>> animalToFoodMap = new Dictionary<string, List<string>>()
    {
        { "Koira", new List<string> { "Pihvi", "Paisti" } },
        { "Pupu", new List<string> { "Porkkana", "Kaali" } },
        { "Lehmä", new List<string> { "Vehnä/Kaura", "Leipä" } },
        { "Lammas", new List<string> { "Vehnä/Kaura", "Pizza" } },
        { "Possu", new List<string> { "Porkkana", "Pähkinät" } },
        { "Strutsi", new List<string> { "Pähkinät", "Kurkku" } },
        { "Kissa", new List<string> { "Kala", "Pihvi" } },
        { "Kana", new List<string> { "Lehdet/Nurtsi", "Oliivi" } }
    };

    private void Update()
    {
        timerToChangeFood -= Time.deltaTime;

        //Timer depends which difficulty was chosen
        if (timerToChangeFood <= 0)
        {
            StartCoroutine(ChangeFoodAndAnimal());
            if (Difficulty.difficulty.easy)
            {
                timerToChangeFood = 100;
            }
            else if (Difficulty.difficulty.normal)
            {
                timerToChangeFood = 10;
            }
            else if (Difficulty.difficulty.hard)
            {
                timerToChangeFood = 5;
            }
        }

        timerToChangeAnimal -= Time.deltaTime;

        if (timerToChangeAnimal <= 0)
        {
            timeForAChange = true;
            AnimalToGetSwapped();
            AddFoods();
            timerToChangeAnimal = 20;
        }
    }

    private IEnumerator ChangeFoodAndAnimal()
    {
        //Exits the coroutine if if the bool is true
        if (isFoodChanging)
        {
            yield break;
        }

        isFoodChanging = true;

        //A slight delay to ensure that user cant see the food change
        float transitionDuration = 0.5f;

        float elapsedTime = 0;

        //This allows for a smooth transition without pausing the game
        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        RandomFood();
        RandomCorrectAnimal();

        isFoodChanging = false;

    }
    private void Start()
    {
        randomAnimalAndFood = this;
        ChooseRandomAnimals();
    }

    public void ChooseRandomAnimals()
    {
        //The amount of animals depends on difficulty
        CheckForDifficulty();
        
        //Adds the amount of animals that are in the animals list (Makes the list dynamic instead of locking it to any number)
        List<int> amountOfAnimals = new List<int>();

        for (int i = 0; i < animals.Count; i++)
        {
            amountOfAnimals.Add(i);
        }

        //This chooses 4 random animals from the list by running the loop 4 times 
        for (int i = 0; i < numAnimalsToChoose; i++)
        {
            //Randomly chooses a number and stores it in randomAnimal
            int randomAnimal = Random.Range(0, amountOfAnimals.Count);
            //Corresponds the random number to the animals list and adds it to chosenAnimals (We do these temporary lists so the actual lists dont get changed)
            chosenAnimals.Add(animals[amountOfAnimals[randomAnimal]]);
            //Removes the randomly generated randomAnimal in order to prevent it to be chosen again
            amountOfAnimals.RemoveAt(randomAnimal);
        }

        //These are the starting and ending points and the animals are set randomly and evenly on that line
        Vector3 startPosition = lineStart.position;
        Vector3 endPosition = lineEnd.position;

        //Runs this loop 4 times to get a position for each animal
        for (int i = 0; i < chosenAnimals.Count; i++)
        {
            //Calculates the positions as a float and stores it in t
            float t = i / (float)(chosenAnimals.Count - 1);
            //Counts the position based on the given line and the calculation above
            Vector3 newPosition = Vector3.Lerp(startPosition, endPosition, t);
            //Sets the animal in its position
            chosenAnimals[i].transform.position = newPosition;
            //Sets the selected animal as active in the scene
            chosenAnimals[i].SetActive(true);
        }

        //This gets all the animals and checks which of them arent in chosenAnimals and deactivates them 
        foreach (GameObject obj in animals)
        {
            if (!chosenAnimals.Contains(obj))
            {
                obj.SetActive(false);
                animalsThatWerentChosen.Add(obj);

            }
        }

        AddFoods();
        RandomFood();
        RandomCorrectAnimal();
    }

    //This method chooses one animal to be the `correct` one so if two animals eat the same food only one of them can have it
    public void RandomCorrectAnimal()
    {
        //Get a list of animals that can eat the randomly chosen food.
        List<GameObject> possibleAnimals = new List<GameObject>();
        string randomFoodName = randomFood.name;

        //This finds which animal/animals are allowed to eat the chosen food
        foreach (var entry in animalToFoodMap)
        {
            if (entry.Value.Contains(randomFoodName))
            {
                //Check if the GameObject exists in the scene before adding it to possibleAnimals
                GameObject animal = GameObject.Find(entry.Key);
                if (animal != null)
                {
                    possibleAnimals.Add(animal);
                }
            }
        }

        //If there are no animals that can eat the selected food choose any random animal from the chosenAnimals list
        if (possibleAnimals.Count == 0)
        {
            int randomAnimalIndex = Random.Range(0, chosenAnimals.Count);
            correctRandomAnimal = chosenAnimals[randomAnimalIndex];
        }
        else
        {
            // Randomly choose one of the possible animals that can eat the selected food.
            int correctAnimalIndex = Random.Range(0, possibleAnimals.Count);
            correctRandomAnimal = possibleAnimals[correctAnimalIndex];

        }

        Debug.Log("Sy�t� ruoka " + correctRandomAnimal.name);
    }

    //This method chooses the animal that gets changed
    void AnimalToGetSwapped()
    {

        if (timeForAChange)
        {
            do
            {
                //Chooses a random animal from chosen animals to get rid of
                int randomAnimalIndex = Random.Range(0, chosenAnimals.Count);
                animalThatGetsSwapped = chosenAnimals[randomAnimalIndex];

                //Keeps running this loop till the animal that is about to get swapped is no longer the one that is the current animal to be fed
            } while (animalThatGetsSwapped == correctRandomAnimal);
        }
        ActuallyChangeTheAnimal();

        Debug.Log("Animal to get swapped " + animalThatGetsSwapped);

    }
    //This method chooses a random animal from the ones that havent been chosen 
    void ActuallyChangeTheAnimal()
    {
        //Chooses a random animal from the ones that arent currently in the scene
        int randomAnimalIndex = Random.Range(0, animalsThatWerentChosen.Count);
        GameObject animalThatGetsPutIn = animalsThatWerentChosen[randomAnimalIndex];

        //Swaps the new animal and the old one in their lists
        int indexAnimalThatGetsSwapped = chosenAnimals.IndexOf(animalThatGetsSwapped);
        int indexAnimalThatGetsPutIn = animalsThatWerentChosen.IndexOf(animalThatGetsPutIn);

        chosenAnimals[indexAnimalThatGetsSwapped] = animalThatGetsPutIn;
        animalsThatWerentChosen[indexAnimalThatGetsPutIn] = animalThatGetsSwapped;

        //Sets the new animal as active in the scene and deactivates the other
        animalThatGetsSwapped.SetActive(false);
        animalThatGetsPutIn.SetActive(true);

        //This saves the position of the animal that gets swapped so that the new animal takes its place
        Vector3 oldPosition = animalThatGetsSwapped.transform.position;

        animalThatGetsSwapped.transform.position = animalThatGetsPutIn.transform.position;
        animalThatGetsPutIn.transform.position = oldPosition;

        //We clear the food list so it can be filled with new animals foods
        foods.Clear();
        foreach (GameObject food in allFoods)
        {
            //We set them all active so that they can be put in the foods list
            food.SetActive(true);
        }
    }

    //This method checks which animals were chosen and gets their corresponding foods and adds them on one list
    public void AddFoods()
    {

        //Adds the chosen animals corresponding foods in the foods list
        foreach (GameObject animal in chosenAnimals)
        {
            //Check if the animal name exists in the dictionary
            if (animalToFoodMap.ContainsKey(animal.name))
            {
                //Fetches the foods from the dictionary for each animal
                List<string> foodItems = animalToFoodMap[animal.name];
                foreach (string foodName in foodItems)
                {
                    //Find the corresponding food object and add it to the foods list
                    GameObject food = GameObject.Find(foodName);
                    // food.SetActive(true);
                    foods.Add(food);
                }
            }

        }
        //Sets the foods that werent chosen inactive
        foreach (GameObject food in allFoods)
        {
            food.SetActive(false);

        }
        foreach (GameObject food in foods)
        {
            if (food == randomFood)
            {
                food.SetActive(true);
            }
        }
    }

    //This method takes the chosen foods and then chooses a random food from those
    public void RandomFood()
    {
        //Chooses a random number and correlates it to the foods in foodsList
        int chosenFood = Random.Range(0, foods.Count);
        randomFood = foods[chosenFood];

        Debug.Log(randomFood.name);

        //If the new food is the same as the old food it reactivates it so it goes back to its initial position
        if (randomFoodPrevious == randomFood)
        {
            randomFood.SetActive(false);
            FoodPosition(randomFood);
        }

        //Deactivates all the foods that werent chosen 
        foreach (GameObject notChosen in foods)
        {
            if (notChosen != randomFood)
            {
                notChosen.SetActive(false);
            }
        }
        //Sets the foods position
        FoodPosition(randomFood);
        //This makes sure that its able to check if the food has been chosen already
        randomFoodPrevious = randomFood;
    }

    //This method calculates the foods position on a given line so that its always random
    public void FoodPosition(GameObject food)
    {
        //These are the starting and ending points and the food/foods are randomly set evenly on that line
        Vector3 startPosition = lineStartFood.position;
        Vector3 endPosition = lineEndFood.position;

        //Calculates the positions as a float based on the index of the food in the list
        int foodIndex = foods.IndexOf(food);
        float t = foodIndex / (float)(foods.Count - 1);

        //Counts the position based on the given line and the calculation above
        Vector3 newPosition = Vector3.Lerp(startPosition, endPosition, t);

        // Sets the food in its position
        food.transform.position = newPosition;
        // Sets the selected food as active in the scene
        food.SetActive(true);
    }

    //This method checks what difficulty was chosen 
    void CheckForDifficulty()
    {
        if (Difficulty.difficulty.easy)
        {
            numAnimalsToChoose = 3;
        }
        else if (Difficulty.difficulty.normal)
        {
            numAnimalsToChoose = 4;
        }
        else if (Difficulty.difficulty.hard)
        {
            numAnimalsToChoose = 5;
        }
        else { numAnimalsToChoose = 4; }
    }
}


