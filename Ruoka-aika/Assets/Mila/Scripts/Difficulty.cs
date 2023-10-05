using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Difficulty : MonoBehaviour
{
    //This script only stores what difficulty has been chosen 
    public bool easy;
    public bool normal = true;
    public bool hard;

    public bool audioMuted;

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

