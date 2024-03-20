using UnityEngine;

public class GameData : MonoBehaviour
{
    //Handles game data across scenes
    public bool easy = true;
    public bool normal;

    public bool gameRunning = true;
    public bool textInstructions = false;
    public bool audioMuted;

    public bool textOn = false;
    public bool audioOn = true;
    public bool audioAndTextOn;

    public bool spring;
    public bool winter;
    public bool fall;

    public bool finnish;
    public bool instructions;
    public int currentIndex;

    public static GameData gameData;

    void Start()
    {
        //Gets destroyed if there are numerous in the scene 
        if (gameData == null)
        {
            DontDestroyOnLoad(gameObject);
            gameData = this;
        }
        else
        {

            Destroy(gameObject);
        }
    }
}

