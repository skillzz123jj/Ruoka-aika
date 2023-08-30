using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public Button defaultButton; 

    void Start()
    {
        EventSystem.current.SetSelectedGameObject(defaultButton.gameObject);
    }
}