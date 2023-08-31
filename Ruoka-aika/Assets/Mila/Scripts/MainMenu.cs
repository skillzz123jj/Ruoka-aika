using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //These are for buttons
    public void StartGame(int scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void Easy()
    {
        Difficulty.difficulty.easy = true;
        Difficulty.difficulty.normal = false;
        Difficulty.difficulty.hard = false;
    }
    public void Normal()
    {
        Difficulty.difficulty.easy = false;
        Difficulty.difficulty.normal = true;
        Difficulty.difficulty.hard = false;

    }
    public void Hard()
    {
        Difficulty.difficulty.easy = false;
        Difficulty.difficulty.normal = false;
        Difficulty.difficulty.hard = true;
    }

}

