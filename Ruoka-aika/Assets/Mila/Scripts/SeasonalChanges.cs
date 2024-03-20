//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class SeasonalChanges : MonoBehaviour
//{
//    public Sprite springSprite;
//    public Sprite summerSprite;
//    public Sprite fallSprite;
//    public Sprite winterSprite;

//    [SerializeField] Image spriteRenderer;

//    public static SeasonalChanges seasonalChanges;

//    private void Start()
//    {

//        UpdateSeason();
//    }

//    private void UpdateSeason()
//    {
//        //This checks the devices current month and day
//        int currentMonth = System.DateTime.Now.Month;
//        int currentDay = System.DateTime.Now.Day;

//        string currentSeason = DetermineSeason(currentMonth, currentDay);     
//        UpdateSprite(currentSeason);
//    }

//    private string DetermineSeason(int month, int day)
//    {
//        ////This sets how long each season lasts
//        //if ((month == 3 && day >= 20) || (month > 3 && month < 6) || (month == 5 && day <= 31))
//        //{
//        //    return "Spring";
//        //}
//        //else if ((month == 6 && day >= 1) || (month > 6 && month < 9) || (month == 11 && day <= 25))
//        //{
//        //    return "Summer";
//        //}
//        ////9 -22
//        //else if ((month == 11 && day >= 25) || (month > 9 && month < 12) || (month == 11 && day <= 30))
//        //{
//        //    return "Fall";
//        //}
//        //else
//        //{//Winter is just the time that is left
//        //    return "Winter";
//        //}
//        if (month == 12 && day == 1)
//        {
//            return "Winter";
//        }
//        else if (month == 11 && day == 26)
//        {
//            return "Fall";
//        }
//        else
//        {
//            return "Spring";
//        }
//    }

//    private void UpdateSprite(string currentSeason)
//    {
//        //This gets the season data and changes the background based on that
//        switch (currentSeason)
//        {
//            case "Spring":
//                spriteRenderer.sprite = springSprite;
//                break;
//            case "Summer":
//                spriteRenderer.sprite = summerSprite;
//                break;
//            case "Fall":
//                spriteRenderer.sprite = fallSprite;
//                break;
//            case "Winter":
//                spriteRenderer.sprite = winterSprite;
//                break;
//            default:
//                //This sets spring as the default background if the device is unable to check for any dates
//                spriteRenderer.sprite = springSprite;
//                break;
//        }
//    }  
//}

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

    private void Update()
    {
        if (GameData.gameData.winter)
        {
            spriteRenderer.sprite = winterSprite;

        }
        else if (GameData.gameData.fall)
        {
            spriteRenderer.sprite = fallSprite;
        }
        else
        {
            spriteRenderer.sprite = springSprite;
        }
    }

    public void ChangeSeason(string season)
    {
    //    spriteRenderer.sprite = sprite;
        UpdateSprite(season);
    }

    private string DetermineSeason(int month, int day)
    {
        ////This sets how long each season lasts
        //if ((month == 3 && day >= 20) || (month > 3 && month < 6) || (month == 5 && day <= 31))
        //{
        //    return "Spring";
        //}
        //else if ((month == 6 && day >= 1) || (month > 6 && month < 9) || (month == 11 && day <= 25))
        //{
        //    return "Summer";
        //}
        ////9 -22
        //else if ((month == 11 && day >= 25) || (month > 9 && month < 12) || (month == 11 && day <= 30))
        //{
        //    return "Fall";
        //}
        //else
        //{//Winter is just the time that is left
        //    return "Winter";
        //}
        if (month == 12 && day == 1)
        {
            return "Winter";
        }
        else if (month == 11 && day == 26)
        {
            return "Fall";
        }
        else
        {
            return "Spring";
        }
    }

    private void UpdateSprite(string currentSeason)
    {
        //This gets the season data and changes the background based on that
        switch (currentSeason)
        {
            case "Spring":
                GameData.gameData.spring = true;
                GameData.gameData.winter = false;
                GameData.gameData.fall = false;

                break;         
            case "Fall":
                GameData.gameData.spring = false;
                GameData.gameData.winter = false;
                GameData.gameData.fall = true;
                break;
            case "Winter":
                GameData.gameData.spring = false;
                GameData.gameData.winter = true;
                GameData.gameData.fall = false;
                break;
            default:
                //This sets spring as the default background if the device is unable to check for any dates
                spriteRenderer.sprite = springSprite;
                break;
        }
    }
}
