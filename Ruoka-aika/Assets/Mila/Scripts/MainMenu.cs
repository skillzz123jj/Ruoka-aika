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
    public Button hardButton;

    public Sprite easyHighlightSprite;
    public Sprite normalHighlightSprite;
    public Sprite hardHighlightSprite;

    public Sprite easyDefaultSprite;
    public Sprite normalDefaultSprite;
    public Sprite hardDefaultSprite;

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
            //normalButton.image.sprite = normalDefaultSprite;
            //hardButton.image.sprite = hardDefaultSprite;

        }
        if (Difficulty.difficulty.normal)
        {
            normalButton.image.sprite = normalHighlightSprite;
            //easyButton.image.sprite = easyDefaultSprite;
            //hardButton.image.sprite = hardDefaultSprite;

        }
        if (Difficulty.difficulty.hard)
        {
            hardButton.image.sprite = hardHighlightSprite;
            //easyButton.image.sprite = easyDefaultSprite;
            //normalButton.image.sprite = normalDefaultSprite;

        }
    }

    public void Easy()
    {
        Difficulty.difficulty.easy = true;
        Difficulty.difficulty.normal = false;
        Difficulty.difficulty.hard = false;
        normalButton.image.sprite = normalDefaultSprite;
        hardButton.image.sprite = hardDefaultSprite;


    }
    public void Normal()
    {
        Difficulty.difficulty.easy = false;
        Difficulty.difficulty.normal = true;
        Difficulty.difficulty.hard = false;
        easyButton.image.sprite = easyDefaultSprite;
        hardButton.image.sprite = hardDefaultSprite;


    }
    public void Hard()
    {
        Difficulty.difficulty.easy = false;
        Difficulty.difficulty.normal = false;       
        Difficulty.difficulty.hard = true;
        easyButton.image.sprite = easyDefaultSprite;
        normalButton.image.sprite = normalDefaultSprite;

    }



}

