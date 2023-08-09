using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Difficulty : MonoBehaviour
{
    public bool easy;
    public bool normal = true;
    public bool hard;

    public static Difficulty difficulty;
    
    void Start()
    {
        //Doesnt destroy on the next scene if there arent any duplicates
        if (difficulty == null)
        {
            DontDestroyOnLoad(gameObject);
            difficulty = this;
        }
        else
        {

            Destroy(gameObject);
        }
    }

  
    public void StartGame(int scene)
    {
        SceneManager.LoadScene(scene);
    }


    //Checks for the chosen difficulty option
    public void Easy()
    {
        easy = true;
        normal = false;
        hard = false;
    }
    public void Normal()
    {
        easy = false;
        normal = true;
        hard = false;
    }
    public void Hard()
    {
        easy = false;
        normal = false;
        hard = true;
    }
}

