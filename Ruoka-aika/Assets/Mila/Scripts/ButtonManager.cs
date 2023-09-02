using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public Button defaultButton; 

    void Start()
    {
        //Sets the default button when the game ends
        EventSystem.current.SetSelectedGameObject(defaultButton.gameObject);
    }
}