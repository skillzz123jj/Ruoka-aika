using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    [SerializeField] TMP_Text scoreTEXT;
    [SerializeField] TMP_Text livesTEXT;
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] GameObject randomAnimalAndFood;
    public int score;
    public int lives = 3;

    public static Score scoreScript;

    void Start()
    {
        scoreScript = this;

    }

    void Update()
    {
        if (lives == 0)
        {
            randomAnimalAndFood.SetActive(false);
            gameOverScreen.SetActive(true);

        }
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
        lives--;
        livesTEXT.text = lives.ToString();

    }
}
