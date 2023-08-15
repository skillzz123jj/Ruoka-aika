using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    [SerializeField] TMP_Text scoreTEXT;
    public int score;

    public static Score scoreScript;

    void Start()
    {
        scoreScript = this;

    }

    //Raises the score when the food has been fed
    public void ScoreUp()
    {
        score++;
        scoreTEXT.text = score.ToString();
    }
    //Lowers the score for each valid food that wasnt fed (will be implemented later)
    public void ScoreDown()
    {
        score--;
        scoreTEXT.text = score.ToString();
    }
}
