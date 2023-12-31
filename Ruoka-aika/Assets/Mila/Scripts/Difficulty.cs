using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Difficulty : MonoBehaviour
{
    //This script only stores what difficulty has been chosen 
    public bool easy = true;
    public bool normal;

    public bool gameRunning = true;
    public bool textInstructions = true;
    public bool audioMuted;

    public bool textOn = true;
    public bool audioOn = true;
    public bool audioAndTextOn = true;

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
}

