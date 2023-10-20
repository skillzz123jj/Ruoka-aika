using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    [SerializeField] TMP_Text scoreTEXT;
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] GameObject gameWonScreen;
    [SerializeField] GameObject randomAnimalAndFood;
    [SerializeField] GameObject activeFood;
    [SerializeField] GameObject error1;
    [SerializeField] GameObject error2;
    [SerializeField] GameObject error3;
    public int score;
    public int errors = 0;

    public static Score scoreScript;

    void Start()
    {
        scoreScript = this;

    }

    void Update()
    {
        if (errors == 3)
        {
            activeFood.SetActive(false);
            randomAnimalAndFood.SetActive(false);
            Invoke("EndTheGame", 1.5f);
        }
        if (score >= 120)
        {
            activeFood.SetActive(false);
            randomAnimalAndFood.SetActive(false);
            Invoke("GameWon", 1.5f);
        }
    }

    void EndTheGame()
    {
        gameOverScreen.SetActive(true);
        
    }
    void GameWon()
    {
        gameWonScreen.SetActive(true);
    }
    public void ScoreUp()
    {
        score++;
        scoreTEXT.text = score.ToString();
    }
    public void ScoreDown()
    {
        //This checks if the remaining foods are ones that cant be fed and makes sure they dont lower the score
        int increaseAmount = 0;
        foreach (GameObject food in RandomAnimalAndFood.randomAnimalAndFood.chosenFoods)
        {
            if (food.CompareTag("EiSyötävä"))
            {
                increaseAmount++;
            }
        }

        //Takes the amount of foods that were left when the timer ran out and lowers it from total score
        int decreaseAmount = RandomAnimalAndFood.randomAnimalAndFood.foodsLeft;
        decreaseAmount -= increaseAmount;

        if (score >= decreaseAmount)
        {
            score -= decreaseAmount;
        }
        else
        {
            //Keeps the score at 0 even if it does go under 0
            score = 0;
        }

        scoreTEXT.text = score.ToString();
        increaseAmount = 0;
    }
    public void WrongFood()
    {
        errors++;
        if (errors >= 1)
        {
            error1.SetActive(true);
        }
        if (errors >= 2)
        {
            error2.SetActive(true);
        }
        if (errors >= 3)
        {
            error3.SetActive(true);
        
        }
    }
}
