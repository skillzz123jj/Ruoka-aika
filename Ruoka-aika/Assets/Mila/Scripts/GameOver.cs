using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] TMP_Text displayScoreTEXT;
    [SerializeField] GameObject disableFoods;

    void Update()
    {
        displayScoreTEXT.text = Score.scoreScript.score.ToString();
        Invoke("GameEnded", 1.5f);
    }
    void GameEnded()
    {
        disableFoods.SetActive(false);
    }
    public void RestartGame(int scene)
    {

        SceneManager.LoadScene(scene);

    }
    public void BackToMenu(int scene)
    {

        SceneManager.LoadScene(scene);

    }
}
