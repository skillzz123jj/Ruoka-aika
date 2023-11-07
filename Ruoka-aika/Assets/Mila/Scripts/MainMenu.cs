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
        if (Difficulty.difficulty.audioOn)
        {
            audioInsButton.image.sprite = audioHighlightSprite;
      
        }
        if (Difficulty.difficulty.textOn) 
        {
            textInsButton.image.sprite = textHighlightSprite;
         
        }
        if (Difficulty.difficulty.audioAndTextOn)
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

        Difficulty.difficulty.audioOn = true;
        Difficulty.difficulty.textOn = false;
        Difficulty.difficulty.audioAndTextOn = false;
        Difficulty.difficulty.textInstructions = false;

        //Changes the settings in playerPrefs
        audioInstructionsToggle.isOn = true;
        textInstructionsToggle.isOn = false;

        //Changes sprites
        textInsButton.image.sprite = textDefaultSprite;
        textInsButton.image.sprite = textDefaultSprite;
        textAndAudioInsButton.image.sprite = textAndAudioDefaultSprite;

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

         audioInsButton.image.sprite = audioDefaultSprite;
         textAndAudioInsButton.image.sprite = textAndAudioDefaultSprite;
              
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

