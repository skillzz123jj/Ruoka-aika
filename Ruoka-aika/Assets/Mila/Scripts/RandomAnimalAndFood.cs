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
    [SerializeField] public List<GameObject> chosenFoods = new List<GameObject>();
    [SerializeField] List<GameObject> bowls = new List<GameObject>();
    List<string> foodsWithoutAssociations = new List<string>
    {
        "Rengas", "Jäätelö", "Sitruuna", "Karkki", "Chili"
    };

    [SerializeField] Transform lineStart;
    [SerializeField] Transform lineEnd;
    [SerializeField] Transform lineStartFood;
    [SerializeField] Transform lineEndFood;

    public float timerToChangeFood = 10;
    public float timerToChangeAnimal = 20;

    GameObject randomFood;
    GameObject correctRandomAnimal;


    public bool canChangeAnimal;

    //Bools to determine how many foods will be chosen
    bool foodsToChoose1 = true;
    bool foodsToChoose2;
    bool foodsToChoose3;
    bool foodsToChoose4;
    bool foodsToChoose5;
    bool foodsToChoose6;
    bool foodsToChoose7;

    public int foodsLeft;
    public int numberOfFoodsToChoose;
    int numAnimalsToChoose = 4;

    //Allows other scripts to access this one
    public static RandomAnimalAndFood randomAnimalAndFood;

    //This dictionary stores the temporary animals and their foods 
    public Dictionary<string, string> TempDictionary = new Dictionary<string, string>();


    //Dictionary to map animal names to their corresponding food items
    public Dictionary<string, List<string>> AnimalsFoodsDictionary = new Dictionary<string, List<string>>()
    {
        { "Koira", new List<string> { "Pihvi", "Paisti", "Luu" } },
        { "Pupu", new List<string> { "Porkkana", "Kaali" } },
        { "Lehmä", new List<string> { "Kurkku", "Leipä" } },
        { "Lammas", new List<string> { "Retiisi", "Pizza" } },
        { "Possu", new List<string> { "Porkkana", "Pähkinät" } },
        { "Strutsi", new List<string> { "Pähkinät", "Kurkku" } },
        { "Kissa", new List<string> { "Kala", "Pihvi" } },
        { "Kana", new List<string> { "Jyvät", "Oliivi" } },
        { "Alpakka", new List<string> { "Kurkku", "Oliivi" } },
        { "Pesukarhu", new List<string> { "Nakki", "Appelsiini" } },
         { "Hevonen", new List<string> { "Jyvät", "Retiisi" } }
    };

    private void Update()
    {
        timerToChangeFood -= Time.deltaTime;

        if (timerToChangeFood <= 0)
        {
            if (foodsLeft > 0)
            {
                Score.scoreScript.ScoreDown();

            }
            RandomFood(numberOfFoodsToChoose);

            RandomCorrectAnimal();
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
            ChangeRandomAnimal();
            AddFoods();
            timerToChangeAnimal = 60;
        }

        AmountOfScore();
        CheckForCurrentLevel();
    }

    private void Start()
    {
        randomAnimalAndFood = this;
        ChooseRandomAnimals();
    }

    //This method chooses the initial animals and their positions
    public void ChooseRandomAnimals()
    {
        // This determines how many animals are chosen       
        CheckForDifficulty();

        // Adds the amount of animals that are in the animals list (Makes the list dynamic instead of locking it to any number)
        List<int> amountOfAnimals = new List<int>();


        for (int i = 0; i < animals.Count; i++)
        {
            amountOfAnimals.Add(i);
        }

        // These are the starting and ending points and the animals are set randomly and evenly on that line
        Vector3 startPosition = lineStart.position;
        Vector3 endPosition = lineEnd.position;

        for (int i = 0; i < numAnimalsToChoose; i++)
        {
            // Randomly chooses a number and stores it in randomAnimal
            int randomAnimal = Random.Range(0, amountOfAnimals.Count);
            int randomBowl = Random.Range(0, bowls.Count);

            // Corresponds the random number to the animals list and adds it to chosenAnimals
            chosenAnimals.Add(animals[amountOfAnimals[randomAnimal]]);

            // Set the position of the chosen animal
            float t = i / (float)(numAnimalsToChoose - 1);
            Vector3 newPosition = Vector3.Lerp(startPosition, endPosition, t);
            chosenAnimals[i].transform.position = newPosition;

            // Set the position of the chosen bowl to match the animal's position
            bowls[randomBowl].transform.position = newPosition;


            // Remove the randomly generated randomAnimal to prevent it from being chosen again
            amountOfAnimals.RemoveAt(randomAnimal);
            bowls.RemoveAt(randomBowl);
        }

        // Set all the selected animals as active in the scene
        foreach (GameObject animal in chosenAnimals)
        {
            animal.SetActive(true);
        }

        // This gets all the animals and checks which of them aren't in chosenAnimals and deactivates them 
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

    public void RandomCorrectAnimal()
    {
        int foodIndex = 0;
        //Clears the dictionary that checks what animals are allowed to eat the foods
        TempDictionary.Clear();

        //For each food it checks if the animal can eat it and adds it to possible animals
        foreach (GameObject obj in chosenFoods)
        {
            //This makes sure that the foods that arent meant to be eaten, dont get an animals assigned to them
            if (foodsWithoutAssociations.Contains(obj.name))
            {
                Debug.Log($"Eläimet ei saa syödä {obj}");
                continue;
            }
            string randomFoodName = obj.name;

            List<GameObject> possibleAnimals = new List<GameObject>();

            foreach (var entry in AnimalsFoodsDictionary)
            {
                if (entry.Value.Contains(randomFoodName))
                {
                    GameObject animal = GameObject.Find(entry.Key);
                    if (animal != null)
                    {
                        possibleAnimals.Add(animal);
                    }
                }
            }

            if (possibleAnimals.Count > 0)
            {
                //Assign the correct animal for this food based on its index in the list
                int correctAnimalIndex = foodIndex % possibleAnimals.Count;
                correctRandomAnimal = possibleAnimals[correctAnimalIndex];
            }
            else
            {
                //If there are no possible animals, choose a random one from chosenAnimals list
                int randomAnimalIndex = Random.Range(0, chosenAnimals.Count);
                correctRandomAnimal = chosenAnimals[randomAnimalIndex];
            }

            Debug.Log($"Syötä {obj.name} {correctRandomAnimal.name}");

            //Adds it to the temporary dictionary
            TempDictionary.Add(correctRandomAnimal.name, obj.name);
            foodIndex++;
        }
    }

 
    //This method chooses a random animal from the ones that havent been chosen 
    void ChangeRandomAnimal()
    {
        //Chooses the animal that gets swapped out
        int randomSwappedAnimalIndex = Random.Range(0, chosenAnimals.Count);
        GameObject animalThatGetsSwapped = chosenAnimals[randomSwappedAnimalIndex];

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


    public void AddFoods()
    {
        //Create a HashSet to track added food names (HashSet is like a list but it makes sure no duplicates are in it)
        HashSet<string> addedFoodNames = new HashSet<string>();

        //Adds the chosen animals' corresponding foods in the foods list
        foreach (GameObject animal in chosenAnimals)
        {
            // Check if the animal name exists in the dictionary
            if (AnimalsFoodsDictionary.ContainsKey(animal.name))
            {
                // Fetches the foods from the dictionary for each animal
                List<string> foodItems = AnimalsFoodsDictionary[animal.name];
                foreach (string foodName in foodItems)
                {
                    // Only add the food if it hasn't been added before
                    if (!addedFoodNames.Contains(foodName))
                    {
                        // Find the corresponding food object and add it to the foods list
                        GameObject food = GameObject.Find(foodName);
                        foods.Add(food);

                        // Add the food name to the HashSet
                        addedFoodNames.Add(foodName);
                    }
                }
            }
        }
        //When there are 3 or more foods set to be chosen it adds the foods that arent going to be fed to anyone in to the mix
        if (numberOfFoodsToChoose > 2)
        {
            foreach (string foodName in foodsWithoutAssociations)
            {
                GameObject specificFood = GameObject.Find(foodName);
                if (specificFood != null)
                {
                    foods.Add(specificFood);
                    addedFoodNames.Add(foodName);
                }
            }

        }

        // Sets the foods that were chosen active and others inactive
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

        foodsLeft = numberOfFoodsToChoose;
        //This is just makes sure the amount isnt less than 1 or more then 7
        numberOfFoodsToChoose = Mathf.Clamp(numberOfFoodsToChoose, 1, 7);

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

            //Goes over all the chosen foods once more and gives them positions
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
        else if (foodsToChoose6)
        {
            numberOfFoodsToChoose = 6;
        }
        else if (foodsToChoose7)
        {
            numberOfFoodsToChoose = 7;
        }
    }
    void AmountOfFoods()
    {
        //These change how many foods spawn during runtime (Will be replaced later to check for points instead)
        if (Input.GetKey(KeyCode.Q))
        {
            foodsToChoose1 = true;
            foodsToChoose2 = false;
            foodsToChoose3 = false;
            foodsToChoose4 = false;
            foodsToChoose5 = false;
            foodsToChoose6 = false;
            foodsToChoose7 = false;
        }
        if (Input.GetKey(KeyCode.W))
        {
            foodsToChoose1 = false;
            foodsToChoose2 = true;
            foodsToChoose3 = false;
            foodsToChoose4 = false;
            foodsToChoose5 = false;
            foodsToChoose6 = false;
            foodsToChoose7 = false;
        }
        if (Input.GetKey(KeyCode.E))
        {
            foodsToChoose1 = false;
            foodsToChoose2 = false;
            foodsToChoose3 = true;
            foodsToChoose4 = false;
            foodsToChoose5 = false;
            foodsToChoose6 = false;
            foodsToChoose7 = false;
        }
        if (Input.GetKey(KeyCode.R))
        {
            foodsToChoose1 = false;
            foodsToChoose2 = false;
            foodsToChoose3 = false;
            foodsToChoose4 = true;
            foodsToChoose5 = false;
            foodsToChoose6 = false;
            foodsToChoose7 = false;
        }
        if (Input.GetKey(KeyCode.T))
        {
            foodsToChoose1 = false;
            foodsToChoose2 = false;
            foodsToChoose3 = false;
            foodsToChoose4 = false;
            foodsToChoose5 = true;
            foodsToChoose6 = false;
            foodsToChoose7 = false;
        }
        if (Input.GetKey(KeyCode.Y))
        {
            foodsToChoose1 = false;
            foodsToChoose2 = false;
            foodsToChoose3 = false;
            foodsToChoose4 = false;
            foodsToChoose5 = false;
            foodsToChoose6 = true;
            foodsToChoose7 = false;

        }
        if (Input.GetKey(KeyCode.U))
        {
            foodsToChoose1 = false;
            foodsToChoose2 = false;
            foodsToChoose3 = false;
            foodsToChoose4 = false;
            foodsToChoose5 = false;
            foodsToChoose6 = false;
            foodsToChoose7 = true;
        }

    }
    void AmountOfScore()
    {
        //These change how many foods spawn during runtime 
        if (Score.scoreScript.score <= 7)
        {
            foodsToChoose1 = true;
            foodsToChoose2 = false;
            foodsToChoose3 = false;
            foodsToChoose4 = false;
            foodsToChoose5 = false;
            foodsToChoose6 = false;
            foodsToChoose7 = false;
        }
        else if (Score.scoreScript.score <= 20)
        {
            foodsToChoose1 = false;
            foodsToChoose2 = true;
            foodsToChoose3 = false;
            foodsToChoose4 = false;
            foodsToChoose5 = false;
            foodsToChoose6 = false;
            foodsToChoose7 = false;
        }
        else if (Score.scoreScript.score <= 30)
        {
            foodsToChoose1 = false;
            foodsToChoose2 = false;
            foodsToChoose3 = true;
            foodsToChoose4 = false;
            foodsToChoose5 = false;
            foodsToChoose6 = false;
            foodsToChoose7 = false;
        }
        else if (Score.scoreScript.score <= 50)
        {
            foodsToChoose1 = false;
            foodsToChoose2 = false;
            foodsToChoose3 = false;
            foodsToChoose4 = true;
            foodsToChoose5 = false;
            foodsToChoose6 = false;
            foodsToChoose7 = false;
        }
        else if (Score.scoreScript.score <= 65)
        {
            foodsToChoose1 = false;
            foodsToChoose2 = false;
            foodsToChoose3 = false;
            foodsToChoose4 = false;
            foodsToChoose5 = true;
            foodsToChoose6 = false;
            foodsToChoose7 = false;
        }
        else if (Score.scoreScript.score <= 75)
        {
            foodsToChoose1 = false;
            foodsToChoose2 = false;
            foodsToChoose3 = false;
            foodsToChoose4 = false;
            foodsToChoose5 = false;
            foodsToChoose6 = true;
            foodsToChoose7 = false;

        }
        else if (Score.scoreScript.score <= 100)
        {
            foodsToChoose1 = false;
            foodsToChoose2 = false;
            foodsToChoose3 = false;
            foodsToChoose4 = false;
            foodsToChoose5 = false;
            foodsToChoose6 = false;
            foodsToChoose7 = true;
        }

    }
}