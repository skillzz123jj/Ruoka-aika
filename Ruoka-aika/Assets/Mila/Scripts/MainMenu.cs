using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button easyButton;
    public Button normalButton;

    public GameObject easyChosenButton;
    public GameObject normalChosenButton;

    public Button textInsButton;
    public Button audioInsButton;
    public Button textAndAudioInsButton;

    public GameObject textInsButtonChosen;
    public GameObject audioInsButtonChosen;
    public GameObject textAndAudioInsButtonChosen;

    public Toggle audioInstructionsToggle;
    public Toggle textInstructionsToggle;

    [SerializeField] List<Button> buttons = new List<Button>();
    [SerializeField] List<Button> uiButtons = new List<Button>();
    [SerializeField] List<Button> instructionButtons = new List<Button>();
    
    [SerializeField] MenuManager menuManager;

    private void Start()
    {
        GameData.gameData.currentIndex = -1;
        //Initialize the toggles based on player preferences
        audioInstructionsToggle.isOn = PlayerPrefs.GetInt("AudioInstructionsEnabled", 1) == 1;
        textInstructionsToggle.isOn = PlayerPrefs.GetInt("TextInstructionsEnabled", 1) == 1;

        //Add listeners to handle toggle changes
        audioInstructionsToggle.onValueChanged.AddListener(OnAudioInstructionsToggleChanged);
        textInstructionsToggle.onValueChanged.AddListener(OnTextInstructionsToggleChanged);

        if (GameData.gameData.easy)
        {
            Easy();
        }
        else
        {
            Normal();
        }

        if (GameData.gameData.textOn)
        {
            TextInstruction();
        }
        else if (GameData.gameData.audioOn)
        {
            AudioInstruction();
        }
  
    }

    public void StartGame(int scene)
    {
        GameData.gameData.gameRunning = true;
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Tab))
        {
            return;
        }
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(scene);
    }

    private void Update()
    {
        if (GameData.gameData.instructions)
        {
            uiButtons = instructionButtons;
        }
        else
        {
            uiButtons = buttons;
        }
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Tab))
        {
            GameData.gameData.gameRunning = false;

            int nextIndex = GameData.gameData.currentIndex;

            do
            {
                nextIndex = (nextIndex + 1) % uiButtons.Count;
                if (menuManager.skip)
                {

                    nextIndex = (nextIndex + 1) % uiButtons.Count;
                    menuManager.skip = false;
                }

            }
            while (!buttons[nextIndex].interactable);

            GameData.gameData.currentIndex = nextIndex;
            uiButtons[GameData.gameData.currentIndex].Select();
        }
    }

    public void Easy()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Tab))
        {
            return;
        }
        GameData.gameData.easy = true;
        GameData.gameData.normal = false;

        normalChosenButton.SetActive(false);
        easyChosenButton.SetActive(true);
        normalChosenButton.GetComponent<Button>().interactable = false;
        easyChosenButton.GetComponent<Button>().interactable = true;
        easyButton.interactable = false;
        normalButton.interactable = true;

        if (Input.GetKey(KeyCode.Return))
        {
            GameData.gameData.currentIndex++;
            buttons[GameData.gameData.currentIndex].Select();
            easyChosenButton.GetComponent<Button>().Select();
        }
    }
    public void Normal()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Tab))
        {
            return;
        }
        GameData.gameData.easy = false;
        GameData.gameData.normal = true;

        normalChosenButton.SetActive(true);
        easyChosenButton.SetActive(false);
        normalChosenButton.GetComponent<Button>().interactable = true;
        easyChosenButton.GetComponent<Button>().interactable = false;
        easyButton.interactable = true;
        normalButton.interactable = false;

        if (Input.GetKey(KeyCode.Return))
        {
            GameData.gameData.currentIndex++;
            buttons[GameData.gameData.currentIndex].Select();
            normalChosenButton.GetComponent<Button>().Select();
        }
    }
    public void AudioInstruction()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Tab))
        {
            return;
        }


        GameData.gameData.audioOn = true;
        GameData.gameData.textOn = false;
        GameData.gameData.audioAndTextOn = false;
        GameData.gameData.textInstructions = false;

        //Changes the settings in playerPrefs
        audioInstructionsToggle.isOn = true;
        textInstructionsToggle.isOn = false;


        textInsButtonChosen.SetActive(false);
        audioInsButtonChosen.SetActive(true);
        textAndAudioInsButtonChosen.SetActive(false);
        if (Input.GetKey(KeyCode.Return))
        {
            GameData.gameData.currentIndex++;
            buttons[GameData.gameData.currentIndex].Select();
            audioInsButtonChosen.GetComponent<Button>().Select();
        }

        textInsButtonChosen.GetComponent<Button>().interactable = false;
        audioInsButtonChosen.GetComponent<Button>().interactable = true;
        textAndAudioInsButtonChosen.GetComponent<Button>().interactable = false;

        textInsButton.interactable = true;
        audioInsButton.interactable = false;
        textAndAudioInsButton.interactable = true;
    }
    public void TextInstruction()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Tab))
        {
            return;
        }

        GameData.gameData.audioOn = false;
        GameData.gameData.textOn = true;
        GameData.gameData.audioAndTextOn = false;
        GameData.gameData.textInstructions = true;

        audioInstructionsToggle.isOn = false;
        textInstructionsToggle.isOn = true;

        textInsButtonChosen.SetActive(true);
        audioInsButtonChosen.SetActive(false);
        textAndAudioInsButtonChosen.SetActive(false);

        if (Input.GetKey(KeyCode.Return))
        {
            GameData.gameData.currentIndex++;
            buttons[GameData.gameData.currentIndex].Select();
            textInsButtonChosen.GetComponent<Button>().Select();
        }

        textInsButtonChosen.GetComponent<Button>().interactable = true;
        audioInsButtonChosen.GetComponent<Button>().interactable = false;
        textAndAudioInsButtonChosen.GetComponent<Button>().interactable = false;

        textInsButton.interactable = false;
        audioInsButton.interactable = true;
        textAndAudioInsButton.interactable = true;

    }

    public void AudioAndTextInstruction()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Tab))
        {
            return;
        }
        GameData.gameData.audioOn = false;
        GameData.gameData.textOn = false;
        GameData.gameData.audioAndTextOn = true;
        GameData.gameData.textInstructions = true;

        audioInstructionsToggle.isOn = true;
        textInstructionsToggle.isOn = true;

        textInsButtonChosen.SetActive(false);
        audioInsButtonChosen.SetActive(false);
        textAndAudioInsButtonChosen.SetActive(true);

        if (Input.GetKey(KeyCode.Return))
        {
            GameData.gameData.currentIndex++;
            buttons[GameData.gameData.currentIndex].Select();
            textAndAudioInsButtonChosen.GetComponent<Button>().Select();
        }

        textInsButtonChosen.GetComponent<Button>().interactable = false;
        audioInsButtonChosen.GetComponent<Button>().interactable = false;
        textAndAudioInsButtonChosen.GetComponent<Button>().interactable = true;

        textInsButton.interactable = true;
        audioInsButton.interactable = true;
        textAndAudioInsButton.interactable = false;

    }
    public void OnAudioInstructionsToggleChanged(bool isOn)
    {
        //Save the preference to PlayerPrefs
        PlayerPrefs.SetInt("AudioInstructionsEnabled", isOn ? 1 : 0);
    }

    public void OnTextInstructionsToggleChanged(bool isOn)
    {
        //Save the preference to PlayerPrefs
        PlayerPrefs.SetInt("TextInstructionsEnabled", isOn ? 1 : 0);
    }
}

