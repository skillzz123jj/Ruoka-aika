using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LanguageSettings : MonoBehaviour
{

    [SerializeField] TMP_Text text;
    bool isFinnish = true;
    [SerializeField] List<GameObject> finnishObjects = new List<GameObject>();
    [SerializeField] List<GameObject> swedishObjects = new List<GameObject>();

    void Update()
    {
        if (text)
        {
            if (GameData.gameData.finnish)
            {
                text.text = "Ruotsi";

            }
            else
            {
                text.text = "Finska";

            }

        }
        if (GameData.gameData.finnish)
        {
            foreach (GameObject obj in finnishObjects)
            {
                obj.SetActive(true);
            }
            foreach (GameObject obj in swedishObjects)
            {
                obj.SetActive(false);
            }
        }
        else
        {
            foreach (GameObject obj in finnishObjects)
            {
                obj.SetActive(false);
            }
            foreach (GameObject obj in swedishObjects)
            {
                obj.SetActive(true);
            }
        }

    }

    public void ChangeLanguage()
    {
        isFinnish = !isFinnish;
        GameData.gameData.finnish = isFinnish;
        text.text = isFinnish ? "Ruotsi" : "Finska";
    }
}
