using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RandomAnimalAndFood : MonoBehaviour
{

    public List<GameObject> animals = new List<GameObject>();
    List<GameObject> chosenAnimals = new List<GameObject>();
    public Transform lineStart;
    public Transform lineEnd;

    public Transform lineStartFood;
    public Transform lineEndFood;

    [SerializeField] List<GameObject> foods = new List<GameObject>();
    [SerializeField] List<GameObject> allFoods = new List<GameObject>();

    GameObject randomFood;
    GameObject randomFoodPrevious;
    GameObject correctRandomAnimal;

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

    }
    private void Start()
    {
        randomAnimalAndFood = this;
        ChooseRandomAnimals();
    }

    public void ChooseRandomAnimals()
    {
        int numAnimalsToChoose = 4;

        //Adds the amount of animals that are in the list
        List<int> amountOfAnimals = new List<int>();

        for (int i = 0; i < animals.Count; i++)
        {
            amountOfAnimals.Add(i);
        }

        // This chooses 4 random indexes from the list by running the loop 4 times 
        for (int i = 0; i < numAnimalsToChoose; i++)
        {
            //Randomly chooses the animal and stores it in randomAnimal
            int randomAnimal = Random.Range(0, amountOfAnimals.Count);
            //Corresponds the random number to the allAnimals list and adds it to chosenAnimals
            chosenAnimals.Add(animals[amountOfAnimals[randomAnimal]]);
            //Removes the randomly generated randomAnimal in order to prevent it to be chosen again
            amountOfAnimals.RemoveAt(randomAnimal);
        }

        //These are the starting and ending points the animals are randomly set evenly on that line
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


        //This gets all the animals and checks which of these arent in chosenAnimals and deactivates them 
        foreach (GameObject obj in animals)
        {
            if (!chosenAnimals.Contains(obj))
            {
                obj.SetActive(false);

            }
        }

        AddFoods();
        RandomFood();
        RandomCorrectAnimal();
    }

    public void RandomCorrectAnimal()
    {
        // Get a list of animals that can eat the randomly chosen food.
        List<GameObject> possibleAnimals = new List<GameObject>();
        string randomFoodName = randomFood.name;

        //This finds which animal/animals are allowed to eat the chosen food
        foreach (var entry in animalToFoodMap)
        {
            if (entry.Value.Contains(randomFoodName))
            {
                possibleAnimals.Add(GameObject.Find(entry.Key));
            }
        }

        // If there are no animals that can eat the selected food, choose any random animal from the chosenAnimals list.
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
        Debug.Log("Syötä ruoka " + correctRandomAnimal.name);
    }

    public void AddFoods()
    {
        //Adds the chosen animals foods in the foods list
        foreach (GameObject obj in chosenAnimals)
        {
            //Check if the animal name exists in the dictionary
            if (animalToFoodMap.ContainsKey(obj.name))
            {
                List<string> foodItems = animalToFoodMap[obj.name];
                foreach (string foodName in foodItems)
                {
                    //Find the corresponding food object and add it to the foods list
                    GameObject food = GameObject.Find(foodName);
                    food.SetActive(true);
                    foods.Add(food);
                }
            }

        }
        //Sets the foods that werent chosen inactive
        foreach (GameObject food in allFoods)
        {
            food.SetActive(false);
        }
    }

    public void RandomFood()
    {
        //Chooses a random number and correlates it to the foods in foodsList
        int chosenFood = Random.Range(0, foods.Count);
        randomFood = foods[chosenFood];

        Debug.Log(randomFood.name);
        //It sets that one active and the rest as inactive
        //  randomFood.SetActive(true);

        if (randomFoodPrevious == randomFood)
        {
            randomFood.SetActive(false);
            FoodPosition(randomFood);

        }

        foreach (GameObject notChosen in foods)
        {
            if (notChosen != randomFood)
            {
                notChosen.SetActive(false);
            }
        }
        FoodPosition(randomFood);
        randomFoodPrevious = randomFood;
    }

    public void FoodPosition(GameObject food)
    {
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
}