using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayInstructions : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] GameObject startAudioButton;
    [SerializeField] GameObject stopAudioButton;

    [SerializeField] MenuManager menuManager;

    public void Play()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            return;
        }

        StartCoroutine(PlayAudio());

    }
    public void ActivateButtons()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            return;
        }

        startAudioButton.SetActive(true);
        stopAudioButton.SetActive(false);
        Button audioOff = stopAudioButton.GetComponent<Button>();
        audioOff.interactable = false;
        Button button = startAudioButton.GetComponent<Button>();
        button.interactable = true;
    }
    public void StopAudio()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            return;
        }

        startAudioButton.SetActive(true);
        stopAudioButton.SetActive(false);
        Button audioOff = stopAudioButton.GetComponent<Button>();
        audioOff.interactable = false;
        Button button = startAudioButton.GetComponent<Button>();
        button.Select();
        button.interactable = true;
        audioSource.Stop();
    }
    public IEnumerator PlayAudio()
    {

        audioSource.Play();
        startAudioButton.SetActive(false);
        stopAudioButton.SetActive(true);
        Button button = startAudioButton.GetComponent<Button>();
        button.interactable = false;
        Button audioOff = stopAudioButton.GetComponent<Button>();
        audioOff.Select();
        audioOff.interactable = true;
        menuManager.skip = true;
        yield return new WaitForSeconds(audioSource.clip.length);
        startAudioButton.SetActive(true);
        stopAudioButton.SetActive(false);
        button = startAudioButton.GetComponent<Button>();
        button.Select();
        button.interactable = true;
        audioOff = stopAudioButton.GetComponent<Button>();
        audioOff.interactable = false;

    }
}