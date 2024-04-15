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

    public GameObject textInsButtonChosen;
    public GameObject audioInsButtonChosen;

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

        if (!GameData.gameData.clicked)
        {
            AudioInstruction();
            GameData.gameData.clicked = true;
        }

        if (GameData.gameData.easy)
        {
            normalChosenButton.SetActive(false);
            easyChosenButton.SetActive(true);
            normalChosenButton.GetComponent<Button>().interactable = false;
            easyChosenButton.GetComponent<Button>().interactable = true;
            easyButton.interactable = false;
            normalButton.interactable = true;
        }
        else
        {
            normalChosenButton.SetActive(true);
            easyChosenButton.SetActive(false);
            normalChosenButton.GetComponent<Button>().interactable = true;
            easyChosenButton.GetComponent<Button>().interactable = false;
            easyButton.interactable = true;
            normalButton.interactable = false;
        }

        if (GameData.gameData.textOn)
        {
            textInsButtonChosen.SetActive(true);
            audioInsButtonChosen.SetActive(false);
            textInsButtonChosen.GetComponent<Button>().interactable = true;
            audioInsButtonChosen.GetComponent<Button>().interactable = false;

            textInsButton.interactable = false;
            audioInsButton.interactable = true;
        }
        else if (GameData.gameData.audioOn)
        {
          
            textInsButtonChosen.SetActive(false);
            audioInsButtonChosen.SetActive(true);

            textInsButtonChosen.GetComponent<Button>().interactable = false;
            audioInsButtonChosen.GetComponent<Button>().interactable = true;

            textInsButton.interactable = true;
            audioInsButton.interactable = false;
        }
  
    }

    public void StartGame(int scene)
    {
        GameData.gameData.gameRunning = true;
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Tab))
        {
            return;
        }
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
            while (!uiButtons[nextIndex].interactable);

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

        if (Input.GetKey(KeyCode.Return))
        {
            GameData.gameData.currentIndex++;
            buttons[GameData.gameData.currentIndex].Select();
            audioInsButtonChosen.GetComponent<Button>().Select();
        }

        textInsButtonChosen.GetComponent<Button>().interactable = false;
        audioInsButtonChosen.GetComponent<Button>().interactable = true; 

        textInsButton.interactable = true;
        audioInsButton.interactable = false;
  
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

        if (Input.GetKey(KeyCode.Return))
        {
            GameData.gameData.currentIndex++;
            buttons[GameData.gameData.currentIndex].Select();
            textInsButtonChosen.GetComponent<Button>().Select();
        }

        textInsButtonChosen.GetComponent<Button>().interactable = true;
        audioInsButtonChosen.GetComponent<Button>().interactable = false;

        textInsButton.interactable = false;
        audioInsButton.interactable = true;

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

