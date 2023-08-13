using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAnimalAndFood : MonoBehaviour
{
    [SerializeField] List<GameObject> animals = new List<GameObject>();
    [SerializeField] List<GameObject> chosenAnimals = new List<GameObject>();
    [SerializeField] List<GameObject> animalsThatWerentChosen = new List<GameObject>();
    [SerializeField] List<GameObject> foods = new List<GameObject>();
    [SerializeField] List<GameObject> allFoods = new List<GameObject>();
    [SerializeField] List<GameObject> chosenFoods = new List<GameObject>();

    [SerializeField] Transform lineStart;
    [SerializeField] Transform lineEnd;
    [SerializeField] Transform lineStartFood;
    [SerializeField] Transform lineEndFood;

    public float timerToChangeFood = 10;
    public float timerToChangeAnimal = 20;

    GameObject randomFood;
    GameObject correctRandomAnimal;
    GameObject animalThatGetsSwapped;

    private bool timeForAChangeOfAnimal;

    //Bools to determine how many foods will be chosen
    bool foodsToChoose1;
    bool foodsToChoose2 = true;
    bool foodsToChoose3;
    bool foodsToChoose4;
    bool foodsToChoose5;

    public int numberOfFoodsToChoose;
    int numAnimalsToChoose = 4;


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
            timeForAChangeOfAnimal = true;
            AnimalToGetSwapped();
            AddFoods();
            timerToChangeAnimal = 60;
        }

        //These change how many foods spawn during runtime (Will be replaced later to check for points instead)
        if (Input.GetKey(KeyCode.Q))
        {
            foodsToChoose1 = true;
            foodsToChoose2 = false;
            foodsToChoose3 = false;
            foodsToChoose4 = false;
            foodsToChoose5 = false;
        }
        if (Input.GetKey(KeyCode.W))
        {
            foodsToChoose1 = false;
            foodsToChoose2 = true;
            foodsToChoose3 = false;
            foodsToChoose4 = false;
            foodsToChoose5 = false;
        }
        if (Input.GetKey(KeyCode.E))
        {
            foodsToChoose1 = false;
            foodsToChoose2 = false;
            foodsToChoose3 = true;
            foodsToChoose4 = false;
            foodsToChoose5 = false;
        }
        if (Input.GetKey(KeyCode.R))
        {
            foodsToChoose1 = false;
            foodsToChoose2 = false;
            foodsToChoose3 = false;
            foodsToChoose4 = true;
            foodsToChoose5 = false;
        }
        if (Input.GetKey(KeyCode.T))
        {
            foodsToChoose1 = false;
            foodsToChoose2 = false;
            foodsToChoose3 = false;
            foodsToChoose4 = false;
            foodsToChoose5 = true;
        }
        CheckForCurrentLevel();
    }
    private IEnumerator ChangeFoodAndAnimal()
    {
        //A slight delay to ensure that the user can't see the food change
        float transitionDuration = 0.5f;

        //Wait for the transition duration
        yield return new WaitForSeconds(transitionDuration);

        RandomFood(numberOfFoodsToChoose);

    }

    private void Start()
    {
        randomAnimalAndFood = this;
        ChooseRandomAnimals();
    }

    public void ChooseRandomAnimals()
    {
        //This determines how many animals are chosen       
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
        RandomFood(1);
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
        if (timeForAChangeOfAnimal)
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
                    foods.Add(food);
                }
            }

        }
        //Sets the foods that were chosen active and others inactive 
        foreach (GameObject food in allFoods)
        {
            food.SetActive(false);

        }
        foreach (GameObject food in chosenFoods)
        {
            food.SetActive(true);
        }
    }
    public void RandomFood(int numberOfFoodsToChoose)
    {
        //Clear the list of chosen foods so it can be filled again
        chosenFoods.Clear();

        //This does the calculations based on how many foods are being chosen 
        numberOfFoodsToChoose = Mathf.Clamp(numberOfFoodsToChoose, 1, 5);

        //Deactivate all the foods so that there arent any unnecessary ones active
        foreach (GameObject notChosen in allFoods)
        {
            notChosen.SetActive(false);
        }

        //Copy of the foods list to preserve that one
        List<GameObject> availableFoods = new List<GameObject>(foods);

        //Chooses foods based on the given amount
        for (int i = 0; i < numberOfFoodsToChoose; i++)
        {
            //Chooses a food from availableFoods and makes sure it doesnt get chosen again (It still sometimes does needs to be looked at)
            int chosenFoodIndex = Random.Range(0, availableFoods.Count);
            GameObject chosenFood = availableFoods[chosenFoodIndex];
            chosenFoods.Add(chosenFood);
            availableFoods.RemoveAt(chosenFoodIndex);

            //Goes over all the chosen foods once more and gives it a position
            for (int t = 0; t < chosenFoods.Count; t++)
            {
                FoodPosition(chosenFoods[t]);
                chosenFoods[t].SetActive(true);
            }
        }
    }

    //This method calculates the foods position on a given line so that its always random
    public void FoodPosition(GameObject food)
    {
        //Makes sure there actually are foods in that list
        if (chosenFoods.Count > 0)
        {
            //Finds the position for that food from chosenFoodsList
            int foodIndex = chosenFoods.IndexOf(food);

            if (chosenFoods.Count > 1)
            {
                //Calculates the position on a given line with this calculation
                float t = (float)foodIndex / (chosenFoods.Count - 1);
                Vector3 startPosition = lineStartFood.position;
                Vector3 endPosition = lineEndFood.position;
                Vector3 newPosition = Vector3.Lerp(startPosition, endPosition, t);

                //Sets the food in its position
                food.transform.position = newPosition;
            }
            else
            {
                //If there is only 1 food it gets set to the start of the line
                food.transform.position = lineStartFood.position;
            }
        }
    }

    //This method changes the amount of animals that are present based on difficulty
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

    //This method changes the amount of foods that are going to spawn
    void CheckForCurrentLevel()
    {
        if (foodsToChoose1)
        {
            numberOfFoodsToChoose = 1;
        }
        if (foodsToChoose2)
        {
            numberOfFoodsToChoose = 2;
        }
        else if (foodsToChoose3)
        {
            numberOfFoodsToChoose = 3;
        }
        else if (foodsToChoose4)
        {
            numberOfFoodsToChoose = 4;
        }
        else if (foodsToChoose5)
        {
            numberOfFoodsToChoose = 5;
        }
    }
}
