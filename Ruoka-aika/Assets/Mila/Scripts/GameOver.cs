using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] TMP_Text displayScoreTEXT;

    void Update()
    {
        displayScoreTEXT.text = Score.scoreScript.score.ToString();
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
