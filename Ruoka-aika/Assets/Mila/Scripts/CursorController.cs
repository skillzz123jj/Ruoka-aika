using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.Playables;

public class CursorController : MonoBehaviour
{
    public Texture2D defaultCursor;
    public Texture2D hoverCursor;

    public static CursorController cursor;

    void Start()
    {
        // Set the default cursor
        ChangeCursor(defaultCursor);

        if (cursor == null)
        {
            DontDestroyOnLoad(gameObject);
            cursor = this;
        }
        else
        {

            Destroy(gameObject);
        }
    }

    public void ChangeCursor(Texture2D cursorType)
    {
        Cursor.SetCursor(cursorType, Vector2.zero, CursorMode.Auto);
    }

    void Update()
    {
        if (IsPointerOverSpecificUIElement())
        {
            ChangeCursor(hoverCursor);
        }
        else
        {
            ChangeCursor(defaultCursor);

        }
    }

    bool IsPointerOverSpecificUIElement()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (RaycastResult result in results)
        {
            GameObject hitObject = result.gameObject;
            if (hitObject.CompareTag("Button") || hitObject.transform.parent != null && hitObject.transform.parent.CompareTag("Button"))
            {
                return true;
            }
        }

        return false;
    }
}
