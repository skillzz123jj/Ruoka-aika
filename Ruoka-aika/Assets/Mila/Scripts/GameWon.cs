using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameWon : MonoBehaviour
{
    [SerializeField] TMP_Text displayScoreTEXT;
    [SerializeField] GameObject disableFoods;
    [SerializeField] GameObject subtitles;

    public Button[] buttons;
    private int currentIndex = 0;

    void Update()
    {       
        int score = Score.scoreScript.score;
        displayScoreTEXT.text = $"Sait ruokittua kaikki {score} eläintä";    
        Invoke("GameEnded", 1f);
        Difficulty.difficulty.gameRunning = false;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentIndex = (currentIndex + 1) % buttons.Length;
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

