using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField] GameObject disableFoods;
    [SerializeField] GameObject subtitles;
    [SerializeField] GameObject highlight;

    public Button[] buttons;
    public Button[] uiButtons;
    public Button[] instructionButtons;

    [SerializeField] MenuManager menuManager;

    private void OnEnable()
    {
        GameData.gameData.currentIndex = -1;
        highlight.SetActive(false);

    }
    void Update()
    {
        Invoke("GameEnded", 1f);
        GameData.gameData.gameRunning = false;

        if (GameData.gameData.instructions)
        {
            uiButtons = instructionButtons;
        }
        else
        {
            uiButtons = buttons;
        }
        //Allows UI navigation with a keyboard
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Tab))
        {
            int nextIndex = GameData.gameData.currentIndex;

            do
            {
                nextIndex = (nextIndex + 1) % uiButtons.Length;
                if (menuManager.skip)
                {
                    //Skip the button, increment the index again
                    nextIndex = (nextIndex + 1) % uiButtons.Length;
                    menuManager.skip = false;
                }

            }
            while (!uiButtons[nextIndex].interactable);

            GameData.gameData.currentIndex = nextIndex;
            uiButtons[GameData.gameData.currentIndex].Select();
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
