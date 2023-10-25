using UnityEngine;
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

    public Toggle audioInstructionsToggle;
    public Toggle textInstructionsToggle;

    public Button[] buttons;
    private int currentIndex = 0;

    public static MainMenu mainMenu;

    private void Start()
    {
        //Initialize the toggles based on player preferences
        audioInstructionsToggle.isOn = PlayerPrefs.GetInt("AudioInstructionsEnabled", 1) == 1;
        textInstructionsToggle.isOn = PlayerPrefs.GetInt("TextInstructionsEnabled", 1) == 1;

        //Add listeners to handle toggle changes
        audioInstructionsToggle.onValueChanged.AddListener(OnAudioInstructionsToggleChanged);
        textInstructionsToggle.onValueChanged.AddListener(OnTextInstructionsToggleChanged);
    }

    //These are for buttons
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
            }
            while (!buttons[nextIndex].interactable);

            currentIndex = nextIndex;
            buttons[currentIndex].Select();
        }

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
        if (Input.GetKey(KeyCode.Space))
        {
            return;
        }
        Difficulty.difficulty.easy = true;
        Difficulty.difficulty.normal = false;
        normalButton.image.sprite = normalDefaultSprite;

    }
    public void Normal()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            return;
        }
        Difficulty.difficulty.easy = false;
        Difficulty.difficulty.normal = true;
        easyButton.image.sprite = easyDefaultSprite;

    }

    private void OnAudioInstructionsToggleChanged(bool isOn)
    {
        //Save the preference to PlayerPrefs
        PlayerPrefs.SetInt("AudioInstructionsEnabled", isOn ? 1 : 0);
    }

    private void OnTextInstructionsToggleChanged(bool isOn)
    {
        //Save the preference to PlayerPrefs
        PlayerPrefs.SetInt("TextInstructionsEnabled", isOn ? 1 : 0);
    }
}

