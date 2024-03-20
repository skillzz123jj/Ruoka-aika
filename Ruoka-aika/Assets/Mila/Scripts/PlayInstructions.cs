using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayInstructions : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] TMP_Text buttonText;
    [SerializeField] GameObject startAudioButton;
    [SerializeField] GameObject stopAudioButton;

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
        Debug.Log(audioSource.clip.length);
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