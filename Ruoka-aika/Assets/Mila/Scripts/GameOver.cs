using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField] GameObject disableFoods;
    [SerializeField] GameObject subtitles;

    public Button[] buttons;
    private int currentIndex = 0;

    [SerializeField] MenuManager menuManager;

    void Update()
    {         
        Invoke("GameEnded", 1f);
        Difficulty.difficulty.gameRunning = false;

        if (Input.GetKeyDown(KeyCode.Space))
        {
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
    }
    void GameEnded()
    {
        Destroy(disableFoods);
        Destroy(subtitles);
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
