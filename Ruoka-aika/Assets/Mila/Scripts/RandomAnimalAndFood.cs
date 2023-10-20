using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;

public class RandomAnimalAndFood : MonoBehaviour
{
    [SerializeField] List<GameObject> animals = new List<GameObject>();
    [SerializeField] public List<GameObject> chosenAnimals = new List<GameObject>();
    [SerializeField] List<GameObject> animalsThatWerentChosen = new List<GameObject>();
    [SerializeField] List<GameObject> foods = new List<GameObject>();
    [SerializeField] public List<GameObject> allFoods = new List<GameObject>();
    [SerializeField] public List<GameObject> chosenFoods = new List<GameObject>();
    [SerializeField] List<GameObject> bowls = new List<GameObject>();

    public AudioSource audioSource;

    public List<Vector2> foodPositions = new List<Vector2>();
    public List<Vector2> copyOfFoodPositions = new List<Vector2>();

    public Vector2 startPosition;
    public Vector2 spaceBetweenFoods;

    List<string> foodsWithoutAssociations = new List<string>
    {
        "Rengas", "Jäätelö", "Sitruuna", "Karkki", "Chili", "Pizza"
    };

    [SerializeField] Transform lineStart;
    [SerializeField] Transform lineEnd;

    public float timerToChangeFood = 10;
    public float timerToChangeAnimal = 20;

    GameObject correctRandomAnimal;

    [SerializeField] Animator smokeAnim;

    public GameObject smoke;
    public bool canChangeAnimal;
    bool justChangedAnimals;
    public bool changedFoodsRecently;
    public bool nowIsAGoodTime;

    public int foodsLeft = 1;
    public int numberOfFoodsToChoose;
    public int numberOfAllowedBadFoods;
    int numAnimalsToChoose = 4;

    [SerializeField] TMP_Text instructionTEXT;

    //Allows other scripts to access this one
    public static RandomAnimalAndFood randomAnimalAndFood;

    //This dictionary stores the temporary animals and their foods 
    public Dictionary<string, List<string>> TempDictionary = new Dictionary<string, List<string>>();

    public Dictionary<GameObject, Vector2> FoodPositionDictionary = new Dictionary<GameObject, Vector2>();

    //Dictionary to map animal names to their corresponding food items
    public Dictionary<string, List<string>> AnimalsFoodsDictionary = new Dictionary<string, List<string>>()
    {
        { "Koira", new List<string> { "Pihvi", "Paisti", "Luu", "Koiranruoka", "Broileri" } },
        { "Pupu", new List<string> { "Porkkana", "Kaali", "Lehdet" } },
        { "Lehmä", new List<string> { "Kurkku", "Lehdet", "Vehnä" } },
        { "Lammas", new List<string> { "Retiisi", "Lehdet", "Vehnä" } },
        { "Possu", new List<string> { "Porkkana", "Kurkku", "Sienet", "Retiisi" } },
        { "Strutsi", new List<string> { "Pähkinät", "Kurkku", "Mato", "Etana" } },
        { "Kissa", new List<string> { "Kala", "Pihvi", "Kissanruoka", "Kinkku" } },
        { "Kana", new List<string> { "Jyvät", "Oliivi", "Leipä" } },
        { "Alpakka", new List<string> { "Vehnä", "Lehdet" } },
        { "Pesukarhu", new List<string> { "Nakki", "Appelsiini", "Lehdet", "Leipä", "Kala", "Kurkku", "Vehnä", "Oliivi", "Kaali",
           "Pähkinät", "Porkkana", "Paisti", "Pihvi", "Etana", "Jyvät", "Mato", "Retiisi", "Luu", "Broileri", "Kinkku","Sienet"} },
         { "Hevonen", new List<string> { "Vehnä", "Retiisi" } }
    };

    private void Update()
    {
        if (Input.GetKey(KeyCode.L))
        {
            foreach (GameObject hmm in chosenFoods)
            {
                Debug.Log(hmm);
            }
        }
       
        timerToChangeFood -= Time.deltaTime;

        if (timerToChangeFood <= 0 && !ActiveFood.activeFood.isMoving)
        {
            if (foodsLeft > 0)
            {
                Score.scoreScript.ScoreDown();

            }
            if (nowIsAGoodTime)
            {
              
                StartCoroutine(CanChangeAnimal(2F));
          
            }else
            {
                ActiveFood.activeFood.wasChosen = false;
                ActiveFood.activeFood.currentActiveFood = null;
                RandomFood(numberOfFoodsToChoose, numberOfAllowedBadFoods);
                RandomCorrectAnimal();
                //  timerToChangeFood = 10;
                ActiveFood.activeFood.wasChosen = false;
                ActiveFood.activeFood.highLight.SetActive(false);
                ActiveFood.activeFood.currentActiveFood = null;


            }




            if (Difficulty.difficulty.easy)
            {
                timerToChangeFood = 100;
            }
            else if (Difficulty.difficulty.normal)
            {
                timerToChangeFood = 10;
            }
        }

        timerToChangeAnimal -= Time.deltaTime;

        if (timerToChangeAnimal <= 0)
        {
            nowIsAGoodTime = true;
        }

        CheckForCurrentLevel();
      
    }

    public IEnumerator CanChangeAnimal(float delay)
    {
        timerToChangeFood = 12;
        justChangedAnimals = true;
        yield return new WaitForSeconds(delay);
        ChangeRandomAnimal();
        AddFoods();
        yield return new WaitForSeconds(1f);
        RandomFood(numberOfFoodsToChoose, numberOfAllowedBadFoods);
        RandomCorrectAnimal();
        timerToChangeAnimal = 75;
        nowIsAGoodTime = false;
    }
    private void Start()
    {
        randomAnimalAndFood = this;
        CheckForCurrentLevel();
        ChooseRandomAnimals();
    }

    //This method chooses the initial animals and their positions
    public void ChooseRandomAnimals()
    {
        Difficulty.difficulty.gameRunning = true;
        //This determines how many animals are chosen       
        CheckForDifficulty();

        //Adds the amount of animals that are in the animals list (Makes the list dynamic instead of locking it to any number)
        List<int> amountOfAnimals = new List<int>();


        for (int i = 0; i < animals.Count; i++)
        {
            amountOfAnimals.Add(i);
        }

        //These are the starting and ending points and the animals are set randomly and evenly on that line
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
        RandomFood(numberOfFoodsToChoose, numberOfAllowedBadFoods);
        RandomCorrectAnimal();
    }
    
    //This method assigns the foods for each animal
    public void RandomCorrectAnimal()
    {
        int index = 0;
        int foodIndex = 0;
        //Clears the dictionary that checks what animals are allowed to eat the foods
        TempDictionary.Clear();

        //instructionTEXT.text = "Anna ";
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
            //string animalName = correctRandomAnimal.name;

            //if (correctRandomAnimal.name == "Alpakka")
            //{
            //    animalName = "alpaka";
            //}
            //else if (animalName == "Hevonen")
            //{
            //    animalName = "hevose";
            //}
            //else if (animalName == "Lammas")
            //{
            //    animalName = "lampaa";
            //}

            //if (numberOfFoodsToChoose == 1)
            //{

            //    instructionTEXT.text += $"{obj.name} {animalName}lle".ToLower();
            //}
            //else if (index == chosenFoods.Count - 1)
            //{
            //    instructionTEXT.text += $"{obj.name} {animalName}lle".ToLower();
            //}
            //else
            //{
            //    instructionTEXT.text += $"{obj.name} {animalName}lle ja ".ToLower();
            //}

            Debug.Log($"Syötä {obj.name} {correctRandomAnimal.name}");

            if (TempDictionary.ContainsKey(correctRandomAnimal.name))
            {
                //If the animal is in the dictionary this adds it another food as a value
                TempDictionary[correctRandomAnimal.name].Add(obj.name);
            }
            else
            {
                //If the animal wasnt in the temporary dictionary it adds it alongside a new list for numerous foods
                List<string> newList = new List<string> { obj.name };
                TempDictionary[correctRandomAnimal.name] = newList;
            }
            foodIndex++;
            index++;


            FoodAnimalCombo foodAnimalCombo = new FoodAnimalCombo();
            foodAnimalCombo.foodName = obj.name;
            foodAnimalCombo.animalName = correctRandomAnimal.name;

            Instruction instruction = InstructionManager.instance.GetInstructionForCombo(foodAnimalCombo);

            if (instruction != null)
            {
                // Play the audio instruction
                if (instruction.audioClip != null)
                {
                    if (audioSource != null)
                    {
                        audioSource.clip = instruction.audioClip;
                        audioSource.Play();
                    }
                }

                // Set the text instruction
                if (!string.IsNullOrEmpty(instruction.textInstruction))
                {
                    instructionTEXT.text = instruction.textInstruction;
                }
            }
            else
            {
                Debug.LogError("No instruction found");
            }

        }
    }

   
    //This method chooses a random animal from the ones that havent been chosen 
    void ChangeRandomAnimal()
    {
        string raccoon = "Pesukarhu";

        GameObject raccoonObject = chosenAnimals.Find(obj => obj.name == raccoon);

        GameObject animalThatGetsSwapped;

        if (chosenAnimals.Contains(raccoonObject))
        {
            animalThatGetsSwapped = raccoonObject;
        }
        else
        {
            //Chooses the animal that gets swapped out
            int randomSwappedAnimalIndex = Random.Range(0, chosenAnimals.Count);
            animalThatGetsSwapped = chosenAnimals[randomSwappedAnimalIndex];
        }

        smoke.transform.position = animalThatGetsSwapped.transform.position;
        smoke.SetActive(true);
        smokeAnim.SetTrigger("Savu");

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
        if (justChangedAnimals)
        {
            Invoke("Delay", 2f);
        }
        else
        {
            foreach (GameObject food in chosenFoods)
            {
                food.SetActive(true);
            }
        }    
    }

    //This gives the foods with a slight delay when an animal was just swapped to make it more smooth
    void Delay()
    {
        foreach (GameObject food in chosenFoods)
        {
            food.SetActive(true);
        }
        justChangedAnimals = false;

    }
    public void RandomFood(int numberOfFoodsToChoose, int numberOfAllowedBadFoods)
    {
        //Clear the list of chosen foods so it can be filled again
        chosenFoods.Clear();
        FoodPositionDictionary.Clear();

        foodsLeft = numberOfFoodsToChoose;
        //This is just makes sure the amount isnt less than 1 or more then 7
        numberOfFoodsToChoose = Mathf.Clamp(numberOfFoodsToChoose, 1, 7);

        //Deactivate all the foods so that there arent any unnecessary ones active
        foreach (GameObject notChosen in allFoods)
        {
            notChosen.SetActive(false);
        }

        int badFoods = 0;
        //Copy of the foods list to preserve that one
        List<GameObject> availableFoods = new List<GameObject>(foods);

        //Chooses foods based on the given amount
        for (int i = 0; i < numberOfFoodsToChoose; i++)
        {
            //Chooses foods randomly until it exceeds the allowed bad foods for that round
            if (badFoods < numberOfAllowedBadFoods)
            {
                //Chooses a food from availableFoods and makes sure it doesnt get chosen again 
                int chosenFoodIndex = Random.Range(0, availableFoods.Count);
                GameObject chosenFood = availableFoods[chosenFoodIndex];
                if (chosenFood.CompareTag("EiSyötävä"))
                {
                    badFoods++;
                }
                chosenFoods.Add(chosenFood);
                availableFoods.RemoveAt(chosenFoodIndex);
            }
            else
            {
                //Chooses a food from availableFoods and makes sure it doesnt get chosen again 
                int chosenFoodIndex = Random.Range(0, availableFoods.Count);
                GameObject chosenFood = availableFoods[chosenFoodIndex];
                while (chosenFood.CompareTag("EiSyötävä"))
                {
                    chosenFoodIndex = Random.Range(0, availableFoods.Count);
                    chosenFood = availableFoods[chosenFoodIndex];
                }
                chosenFoods.Add(chosenFood);
                availableFoods.RemoveAt(chosenFoodIndex);
            }

        }
        PositionFoodsRandomly(chosenFoods);
        chosenFoods.Sort((a, b) => {
            return a.transform.position.x.CompareTo(b.transform.position.x);
        });
    }

  
    //This method gives the foods their positions
    public void PositionFoodsRandomly(List<GameObject> foods)
    {

        foreach (GameObject food in foods)
        {
            int index = Random.Range(0, foodPositions.Count);
            Vector2 newPosition = foodPositions[index];

            //Keeps finding a position till it finds one that isnt taken
            while (copyOfFoodPositions.Contains(newPosition))
            {
                index = Random.Range(0, foodPositions.Count);
                newPosition = foodPositions[index];
            }

            copyOfFoodPositions.Add(newPosition);
            food.transform.position = newPosition;
            food.SetActive(true);

            if (!FoodPositionDictionary.ContainsKey(food))
            {
                Vector2 foodPosition = food.transform.position;
                FoodPositionDictionary.Add(food, foodPosition);
            }
        }
        copyOfFoodPositions.Clear();
        changedFoodsRecently = false;
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
        else { numAnimalsToChoose = 4; }
    }


    //This method changes the amount of foods that are going to spawn
    void CheckForCurrentLevel()
    {
        if (Score.scoreScript.score <= 1) //7
        {

            numberOfFoodsToChoose = 1;
            numberOfAllowedBadFoods = 0;
        }
        else if (Score.scoreScript.score <= 2) //20
        {

            numberOfFoodsToChoose = 2;
            numberOfAllowedBadFoods = 0;
        }
        else if (Score.scoreScript.score <= 3) //30
        {

            numberOfFoodsToChoose = 3;
            numberOfAllowedBadFoods = 1;
        }
        else if (Score.scoreScript.score <= 50) //50
        {

            numberOfFoodsToChoose = 4;
            numberOfAllowedBadFoods = 1;
        }
        else if (Score.scoreScript.score <= 65) //65
        {

            numberOfFoodsToChoose = 5;
            numberOfAllowedBadFoods = 2;
        }
        else if (Score.scoreScript.score <= 75) //75
        {

            numberOfFoodsToChoose = 6;
            numberOfAllowedBadFoods = 2;
        }
        else if (Score.scoreScript.score <= 100) //100
        {

            numberOfFoodsToChoose = 7;
            numberOfAllowedBadFoods = 3;
        }
    }
}