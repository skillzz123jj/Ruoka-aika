using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button easyButton;
    public Button normalButton;

    public Button textInsButton;
    public Button audioInsButton;
    public Button textAndAudioInsButton;

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
                if (menuManager.skip)
                {
                    // Skip the button, increment the index again
                    nextIndex = (nextIndex + 1) % buttons.Length;
                    menuManager.skip = false;
                }

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
    public void AudioInstruction()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            return;
        }

        // Set the audioInstructionsToggle to true
        audioInstructionsToggle.isOn = true;

        // Additionally, you can reset other toggles if needed
        textInstructionsToggle.isOn = false;

        textInsButton.image.sprite = textDefaultSprite;
        audioInsButton.image.sprite = audioHighlightSprite;


        //easyButton.image.sprite = easyDefaultSprite;
    }
    public void TextInstruction()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            return;
        }


        audioInstructionsToggle.isOn = false;

        // Additionally, you can reset other toggles if needed
        textInstructionsToggle.isOn = true;
        textInsButton.image.sprite = textHighlightSprite;
        audioInsButton.image.sprite = audioDefaultSprite;


    }

    public void AudioAndTextInstruction()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            return;
        }
        OnAudioInstructionsToggleChanged(true);
        Difficulty.difficulty.easy = false;
        Difficulty.difficulty.normal = true;
        audioInsButton.image.sprite = audioDefaultSprite;
        textInsButton.image.sprite = textDefaultSprite;

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

