using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

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

    //All of these handle the UI buttons on the top right corner
    public void DisplayInstructions()
    {
        instructions.SetActive(!instructions.activeSelf);
    }
    public void reloadGame(int scene)
    {
        SceneManager.LoadScene(scene);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void MuteAudio()
    {
        Difficulty.difficulty.audioMuted = true;
        audioButton.SetActive(false);
        muteAudioButton.SetActive(true);
        muteAudioText.SetActive(false);
        EventSystem.current.SetSelectedGameObject(muteAudioAsDefault.gameObject);

    }
    public void Audio()
    {
        Difficulty.difficulty.audioMuted = false;
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

        }
        else
        {
            AudioListener.volume = 1f;
            audioButton.SetActive(true);
            muteAudioButton.SetActive(false);
            audioText.SetActive(false);

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