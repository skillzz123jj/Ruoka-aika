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

    public Sprite audioHighlightSprite;
    public Sprite textHighlightSprite;
    public Sprite textAndAudioHighlightSprite;

    public Sprite audioDefaultSprite;
    public Sprite textDefaultSprite;
    public Sprite textAndAudioDefaultSprite;

    public Sprite easyHighlightSprite;
    public Sprite normalHighlightSprite;

    public Sprite easyDefaultSprite;
    public Sprite normalDefaultSprite;

    public Toggle audioInstructionsToggle;
    public Toggle textInstructionsToggle;

    public Selectable[] buttons;
    int currentIndex = 0;

    [SerializeField] MenuManager menuManager;


    private void Start()
    {
        //Initialize the toggles based on player preferences
        audioInstructionsToggle.isOn = PlayerPrefs.GetInt("AudioInstructionsEnabled", 1) == 1;
        textInstructionsToggle.isOn = PlayerPrefs.GetInt("TextInstructionsEnabled", 1) == 1;

        //Add listeners to handle toggle changes
        audioInstructionsToggle.onValueChanged.AddListener(OnAudioInstructionsToggleChanged);
        textInstructionsToggle.onValueChanged.AddListener(OnTextInstructionsToggleChanged);

        if (Difficulty.difficulty.easy)
        {
            Easy();
        }
        else
        {
            Normal();
        }

        if (Difficulty.difficulty.textOn)
        {
            TextInstruction();
        }
        else if (Difficulty.difficulty.audioOn)
        {
            AudioInstruction();
        }
        else
        {
            AudioAndTextInstruction();
        }
    }

    public void StartGame(int scene)
    {
        Difficulty.difficulty.gameRunning = true;
        if (Input.GetKey(KeyCode.Space))
        {
            return;
        }
        SceneManager.LoadScene(scene);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Difficulty.difficulty.gameRunning = false;

            int nextIndex = currentIndex;

            do
            {
                nextIndex = (nextIndex + 1) % buttons.Length;
                if (menuManager.skip)
                {

                    nextIndex = (nextIndex + 1) % buttons.Length;
                    menuManager.skip = false;
                }

            }
            while (!buttons[nextIndex].interactable);

            currentIndex = nextIndex;
            buttons[currentIndex].Select();
        }
    }

    public void Easy()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            return;
        }
        Difficulty.difficulty.easy = true;
        Difficulty.difficulty.normal = false;

        normalChosenButton.SetActive(false);
        easyChosenButton.SetActive(true);
        normalChosenButton.GetComponent<Button>().interactable = false;
        easyChosenButton.GetComponent<Button>().interactable = true;
        easyButton.interactable = false;
        normalButton.interactable = true;

        if (Input.GetKey(KeyCode.Return))
        {
            currentIndex++;
            buttons[currentIndex].Select();
            easyChosenButton.GetComponent<Button>().Select();
        }
    }
    public void Normal()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            return;
        }
        Difficulty.difficulty.easy = false;
        Difficulty.difficulty.normal = true;

        normalChosenButton.SetActive(true);
        easyChosenButton.SetActive(false);
        normalChosenButton.GetComponent<Button>().interactable = true;
        easyChosenButton.GetComponent<Button>().interactable = false;
        easyButton.interactable = true;
        normalButton.interactable = false;

        if (Input.GetKey(KeyCode.Return))
        {
            currentIndex++;
            buttons[currentIndex].Select();
            normalChosenButton.GetComponent<Button>().Select();
        }
    }
    public void AudioInstruction()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            return;
        }


        Difficulty.difficulty.audioOn = true;
        Difficulty.difficulty.textOn = false;
        Difficulty.difficulty.audioAndTextOn = false;
        Difficulty.difficulty.textInstructions = false;

        //Changes the settings in playerPrefs
        audioInstructionsToggle.isOn = true;
        textInstructionsToggle.isOn = false;


        textInsButtonChosen.SetActive(false);
        audioInsButtonChosen.SetActive(true);
        textAndAudioInsButtonChosen.SetActive(false);
        if (Input.GetKey(KeyCode.Return))
        {
            currentIndex++;
            buttons[currentIndex].Select();
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
        if (Input.GetKey(KeyCode.Space))
        {
            return;
        }

        Difficulty.difficulty.audioOn = false;
        Difficulty.difficulty.textOn = true;
        Difficulty.difficulty.audioAndTextOn = false;
        Difficulty.difficulty.textInstructions = true;

        audioInstructionsToggle.isOn = false;
        textInstructionsToggle.isOn = true;

        textInsButtonChosen.SetActive(true);
        audioInsButtonChosen.SetActive(false);
        textAndAudioInsButtonChosen.SetActive(false);

        if (Input.GetKey(KeyCode.Return))
        {
            currentIndex++;
            buttons[currentIndex].Select();
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
        if (Input.GetKey(KeyCode.Space))
        {
            return;
        }
        Difficulty.difficulty.audioOn = false;
        Difficulty.difficulty.textOn = false;
        Difficulty.difficulty.audioAndTextOn = true;
        Difficulty.difficulty.textInstructions = true;

        audioInstructionsToggle.isOn = true;
        textInstructionsToggle.isOn = true;

        textInsButtonChosen.SetActive(false);
        audioInsButtonChosen.SetActive(false);
        textAndAudioInsButtonChosen.SetActive(true);

        if (Input.GetKey(KeyCode.Return))
        {
            currentIndex++;
            buttons[currentIndex].Select();
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

