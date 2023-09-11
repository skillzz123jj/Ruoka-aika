using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeasonalChanges : MonoBehaviour
{
    public Sprite springSprite;
    public Sprite summerSprite;
    public Sprite fallSprite;
    public Sprite winterSprite;

    private SpriteRenderer spriteRenderer;

    public static SeasonalChanges seasonalChanges;

    private void Start()
    {
        //Gets the correct sprite as soon as the game starts
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateSeason();

        //Doesnt destroy on the next scene if there arent any duplicates
        if (seasonalChanges == null)
        {
            DontDestroyOnLoad(gameObject);
            seasonalChanges = this;
        }
        else
        {

            Destroy(gameObject);
        }
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
        else if ((month == 6 && day >= 1) || (month > 6 && month < 9) || (month == 9 && day <= 21))
        {
            return "Summer";
        }
        else if ((month == 9 && day >= 22) || (month > 9 && month < 12) || (month == 11 && day <= 30))
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
