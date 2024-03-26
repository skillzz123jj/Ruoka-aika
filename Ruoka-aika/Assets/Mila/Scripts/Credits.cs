using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class Credits : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    [SerializeField] GameObject credits;
    [SerializeField] Button closeCredits;
    string credit = "Tekijät";

    void Start()
    {
        StartCoroutine(enumerator());
    }

    void Update()
    {
        credit = GameData.gameData.finnish ? "Tekijät" : "Skapare";
        if (credits.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Tab))
            {
                closeCredits.onClick.Invoke();
                text.text = $"{credit}";
            }
            text.text = $"<b>{credit}</b>";
        }

    }
    public void CloseCredits()
    {
        text.text = $"{credit}";
    }
    public void HoverCredits()
    {

        text.text = $"<b>{credit}</b>";
    }

    public void ExitCredits()
    {
        credit = GameData.gameData.finnish ? "Tekijät" : "Skapare";
        text.text = $"{credit}";
    }

    IEnumerator enumerator()
    {
        yield return new WaitForEndOfFrame();

        text.text = $"{credit}";
    }
    public void Instructions()
    {
        GameData.gameData.instructions = false;

    }
}

