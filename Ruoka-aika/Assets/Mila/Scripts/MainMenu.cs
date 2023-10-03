using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button easyButton;
    public Button normalButton;

    public Sprite easyHighlightSprite;
    public Sprite normalHighlightSprite;

    public Sprite easyDefaultSprite;
    public Sprite normalDefaultSprite;

    //These are for buttons
    public void StartGame(int scene)
    {
        SceneManager.LoadScene(scene);
    }
    
    private void Update()
    {
        if (Difficulty.difficulty.easy)
        {
            easyButton.image.sprite = easyHighlightSprite;
            
        }
        if (Difficulty.difficulty.normal)
        {
            normalButton.image.sprite = normalHighlightSprite;
           
        }
    }

    public void Easy()
    {
        Difficulty.difficulty.easy = true;
        Difficulty.difficulty.normal = false;
        Difficulty.difficulty.hard = false;
        normalButton.image.sprite = normalDefaultSprite;

    }
    public void Normal()
    {
        Difficulty.difficulty.easy = false;
        Difficulty.difficulty.normal = true;
        Difficulty.difficulty.hard = false;
        easyButton.image.sprite = easyDefaultSprite;

    }
}

