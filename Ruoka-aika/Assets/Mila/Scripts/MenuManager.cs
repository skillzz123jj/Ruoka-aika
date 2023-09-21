using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] Sprite mute;
    [SerializeField] GameObject audio;
    [SerializeField] GameObject muteAudio;
    [SerializeField] GameObject exitGameText;
    [SerializeField] GameObject muteAudioText;
    [SerializeField] GameObject audioText;
    [SerializeField] GameObject restartText;
    [SerializeField] GameObject instructionText;

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
       audio.SetActive(false);
       muteAudio.SetActive(true);
        muteAudioText.SetActive(false);

    }
    public void Audio()
    {
        audio.SetActive(true);
        muteAudio.SetActive(false);
        audioText.SetActive(false);

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
