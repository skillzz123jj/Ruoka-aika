using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeasonalChanges : MonoBehaviour
{
    public Sprite springSprite;
    public Sprite summerSprite;
    public Sprite fallSprite;
    public Sprite winterSprite;

    [SerializeField] Image spriteRenderer;

    public static SeasonalChanges seasonalChanges;

    private void Start()
    {

        UpdateSeason();
    }

    private void UpdateSeason()
    {
        //This checks the devices current month and day
        int currentMonth = System.DateTime.Now.Month;
        int currentDay = System.DateTime.Now.Day;

        string currentSeason = DetermineSeason(currentMonth, currentDay);
        UpdateSprite(currentSeason);
    }

    private string DetermineSeason(int month, int day)
    {
        //This sets how long each season lasts
        if ((month == 3 && day >= 20) || (month > 3 && month < 6) || (month == 5 && day <= 31))
        {
            return "Spring";
        }
        else if ((month == 6 && day >= 1) || (month > 6 && month < 9) || (month == 11 && day <= 25))
        {
            return "Summer";
        }
        //9 -22
        else if ((month == 11 && day >= 25) || (month > 9 && month < 12) || (month == 11 && day <= 30))
        {
            return "Fall";
        }
        else
        {//Winter is just the time that is left
            return "Winter";
        }
      
    }

    private void UpdateSprite(string currentSeason)
    {
        //This gets the season data and changes the background based on that
        switch (currentSeason)
        {
            case "Spring":
                spriteRenderer.sprite = springSprite;
                break;
            case "Summer":
                spriteRenderer.sprite = summerSprite;
                break;
            case "Fall":
                spriteRenderer.sprite = fallSprite;
                break;
            case "Winter":
                spriteRenderer.sprite = winterSprite;
                break;
            default:
                //This sets spring as the default background if the device is unable to check for any dates
                spriteRenderer.sprite = springSprite;
                break;
        }
    }
}


