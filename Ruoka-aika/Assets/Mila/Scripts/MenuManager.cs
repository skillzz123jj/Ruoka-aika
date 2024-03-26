using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    [SerializeField] Button muteAudioAsDefault;
    [SerializeField] Button audioAsDefault;
    [SerializeField] Button instructionButton;
    [SerializeField] Button closeInstructionsButton;
    [SerializeField] GameObject audioButton;
    [SerializeField] GameObject muteAudioButton;
    [SerializeField] GameObject exitGameText;
    [SerializeField] GameObject muteAudioText;
    [SerializeField] GameObject audioText;
    [SerializeField] GameObject restartText;
    [SerializeField] GameObject instructionText;
    [SerializeField] GameObject instructions;
    [SerializeField] GameObject subtitlesBox;
    [SerializeField] GameObject blur;

    public bool skip;
    int previousIndex;

    [SerializeField] RandomAnimalAndFood randomAnimalAndFood;

    public static MenuManager menuManager;
  
    //All of these handle the UI buttons on the top right corner
    private void Start()
    {
        if (subtitlesBox != null)
        {
            if (GameData.gameData.textInstructions)
            {
                subtitlesBox.SetActive(true);
            }
            else
            {
                subtitlesBox.SetActive(false);

            }
        }

    }
    public void DisplayInstructions()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Tab))
        {
            return;
        }
      
        previousIndex = GameData.gameData.currentIndex;
        GameData.gameData.instructions = true;
        InstructionTextGoAway();
        instructions.SetActive(true);
        if (Input.GetKey(KeyCode.Return))
        {

            closeInstructionsButton.Select();
        }
        GameData.gameData.currentIndex = 0;
        blur.GetComponent<Image>().enabled = true;

    }
    public void CloseInstructions()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Tab))
        {
            return;
        }

        GameData.gameData.instructions = false;
        if (Input.GetKey(KeyCode.Return))
        {
            instructionButton.Select();

        }
      
        GameData.gameData.currentIndex = previousIndex;
        blur.GetComponent<Image>().enabled = false;
        instructions.SetActive(false);

    }
    public void reloadGame(int scene)
    {
        if (GameData.gameData.gameRunning && Input.GetKey(KeyCode.Return))
        {
            return;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            return;
        }
        SceneManager.LoadScene(scene);
        
    }
    public void QuitGame()
    {
        if (GameData.gameData.gameRunning && Input.GetKey(KeyCode.Return))
        {
            return;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            return;
        }

        Application.Quit();
    }
    public void MuteAudio()
    {
        if (GameData.gameData.gameRunning && Input.GetKey(KeyCode.Return))
        {
            return;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            return;
        }
        muteAudioAsDefault.interactable = true;
        audioAsDefault.interactable = false;
        GameData.gameData.audioMuted = true;
      
        audioButton.SetActive(false);
        muteAudioButton.SetActive(true);
        muteAudioText.SetActive(false);
        EventSystem.current.SetSelectedGameObject(muteAudioAsDefault.gameObject);
    }
    public void Audio()
    {
        if (GameData.gameData.gameRunning && Input.GetKey(KeyCode.Return))
        {
            return;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            return;
        }
        audioAsDefault.interactable = true;
        muteAudioAsDefault.interactable = false;
        GameData.gameData.audioMuted = false;
        skip = true;
        audioButton.SetActive(true);
        muteAudioButton.SetActive(false);
        audioText.SetActive(false);
        EventSystem.current.SetSelectedGameObject(audioAsDefault.gameObject);
      
    }
    private void Update()
    {
        if (GameData.gameData.audioMuted)
        {
            AudioListener.volume = 0f;
            audioButton.SetActive(false);
            muteAudioButton.SetActive(true);
            muteAudioText.SetActive(false);
            muteAudioAsDefault.interactable = true;
            audioAsDefault.interactable = false;

        }
        else
        {
            AudioListener.volume = 1f;
            audioButton.SetActive(true);
            muteAudioButton.SetActive(false);
            audioText.SetActive(false);
            muteAudioAsDefault.interactable = false;
            audioAsDefault.interactable = true;

        }
    }
    public void ExitGameText()
    {
        exitGameText.SetActive(true);
    }
    public void ExitGameTextGoAway()
    {
        exitGameText.SetActive(false);
    }
    public void RestartGameText()
    {
        restartText.SetActive(true);
    }
    public void RestartGameTextGoAway()
    {
        restartText.SetActive(false);
    }
    public void InstructionText()
    {
        instructionText.SetActive(true);
    }
    public void InstructionTextGoAway()
    {
        instructionText.SetActive(false);
    }
    public void AudioText()
    {
        audioText.SetActive(true);
    }
    public void AudioTextGoAway()
    {
        audioText.SetActive(false);
    }
    public void MuteAudioText()
    {       
        muteAudioText.SetActive(true);
    }
    public void MuteAudioTextGoAway()
    {  
        muteAudioText.SetActive(false);
    }
   
}