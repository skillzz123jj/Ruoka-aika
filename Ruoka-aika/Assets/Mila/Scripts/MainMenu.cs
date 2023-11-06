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

    bool textOn;
    bool audioOn;
    bool audioAndTextOn = true;

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
        HandleSprites();
    }

    void HandleSprites()
    {
        if (audioOn)
        {
            audioInsButton.image.sprite = audioHighlightSprite;
      
        }
        if (textOn) 
        {
            textInsButton.image.sprite = textHighlightSprite;
         
        }
        if (audioAndTextOn)
        {
            textAndAudioInsButton.image.sprite = textAndAudioHighlightSprite;
       
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

        audioOn = true;
        textOn = false;
        audioAndTextOn = false;

        // Set the audioInstructionsToggle to true
        audioInstructionsToggle.isOn = true;

        // Additionally, you can reset other toggles if needed
        textInstructionsToggle.isOn = false;

        textInsButton.image.sprite = textDefaultSprite;
        textInsButton.image.sprite = textDefaultSprite;
        textAndAudioInsButton.image.sprite = textAndAudioDefaultSprite;



        //easyButton.image.sprite = easyDefaultSprite;
    }
    public void TextInstruction()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            return;
        }

        audioOn = false;
        textOn = true;
        audioAndTextOn = false;

        audioInstructionsToggle.isOn = false;

        // Additionally, you can reset other toggles if needed
        textInstructionsToggle.isOn = true;
  
        audioInsButton.image.sprite = audioDefaultSprite;
        audioInsButton.image.sprite = audioDefaultSprite;
        textAndAudioInsButton.image.sprite = textAndAudioDefaultSprite;


    }

    public void AudioAndTextInstruction()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            return;
        }
        audioOn = false;
        textOn = false;
        audioAndTextOn = true;

        audioInstructionsToggle.isOn = true;
        textInstructionsToggle.isOn = true;

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

