using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField] GameObject disableFoods;
    [SerializeField] GameObject subtitles;

    public Button[] buttons;
    public Button[] uiButtons;
    public Button[] instructionButtons;
    private int currentIndex = -1;

    [SerializeField] MenuManager menuManager;

    void Update()
    {
        Invoke("GameEnded", 1f);
        Difficulty.difficulty.gameRunning = false;

        if (Difficulty.difficulty.instructions)
        {
            uiButtons = instructionButtons;
        }
        else
        {
            uiButtons = buttons;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            int nextIndex = currentIndex;

            do
            {
                nextIndex = (nextIndex + 1) % uiButtons.Length;
                if (menuManager.skip)
                {
                    // Skip the button, increment the index again
                    nextIndex = (nextIndex + 1) % uiButtons.Length;
                    menuManager.skip = false;
                }

            }
            while (!uiButtons[nextIndex].interactable);

            currentIndex = nextIndex;
            uiButtons[currentIndex].Select();
        }
    }
    void GameEnded()
    {
        Destroy(disableFoods);
        Destroy(subtitles);
    }
    public void CorrectIndex()
    {
        currentIndex = 3;

    }
    public void RestartGame(int scene)
    {
        if (Input.GetKey(KeyCode.Space))
        {
            return;
        }
        Time.timeScale = 1f;
        SceneManager.LoadScene(scene);

    }
    public void BackToMenu(int scene)
    {
        if (Input.GetKey(KeyCode.Space))
        {
            return;
        }
        SceneManager.LoadScene(scene);
    }
}
