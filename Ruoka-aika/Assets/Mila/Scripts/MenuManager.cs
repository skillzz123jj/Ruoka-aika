using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class MenuManager : MonoBehaviour
{
    [SerializeField] Button muteAudioAsDefault;
    [SerializeField] Button audioAsDefault;
    [SerializeField] GameObject audioButton;
    [SerializeField] GameObject muteAudioButton;
    [SerializeField] GameObject exitGameText;
    [SerializeField] GameObject muteAudioText;
    [SerializeField] GameObject audioText;
    [SerializeField] GameObject restartText;
    [SerializeField] GameObject instructionText;
    [SerializeField] GameObject instructions;
    [SerializeField] GameObject subtitlesBox;

    public bool skip;

    [SerializeField] MainMenu mainMenu;


    //All of these handle the UI buttons on the top right corner
    private void Start()
    {
        if (subtitlesBox != null)
        {
            if (Difficulty.difficulty.textInstructions)
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

        if (Difficulty.difficulty.gameRunning && Input.GetKey(KeyCode.Return))
        {
            return;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            return;
        }
   
        instructions.SetActive(!instructions.activeSelf);
    }
    public void reloadGame(int scene)
    {
        if (Difficulty.difficulty.gameRunning && Input.GetKey(KeyCode.Return))
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
        if (Difficulty.difficulty.gameRunning && Input.GetKey(KeyCode.Return))
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
        if (Difficulty.difficulty.gameRunning && Input.GetKey(KeyCode.Return))
        {
            return;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            return;
        }
        muteAudioAsDefault.interactable = true;
        audioAsDefault.interactable = false;
        Difficulty.difficulty.audioMuted = true;
      
        audioButton.SetActive(false);
        muteAudioButton.SetActive(true);
        muteAudioText.SetActive(false);
        EventSystem.current.SetSelectedGameObject(muteAudioAsDefault.gameObject);


    }

    public void Audio()
    {
        if (Difficulty.difficulty.gameRunning && Input.GetKey(KeyCode.Return))
        {
            return;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            return;
        }
        audioAsDefault.interactable = true;
        muteAudioAsDefault.interactable = false;
        Difficulty.difficulty.audioMuted = false;
        skip = true;
        audioButton.SetActive(true);
        muteAudioButton.SetActive(false);
        audioText.SetActive(false);
        EventSystem.current.SetSelectedGameObject(audioAsDefault.gameObject);
       

    }
    private void Update()
    {

        if (Difficulty.difficulty.audioMuted)
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