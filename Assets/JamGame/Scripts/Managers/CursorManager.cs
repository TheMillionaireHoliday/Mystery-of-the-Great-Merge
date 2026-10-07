using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance;

    public Texture2D defaultCursor;
    public Texture2D hoverCursor;
    public Texture2D cancelCursor;

    public Vector2 hotspot = Vector2.zero;
    public CursorMode cursorMode = CursorMode.Auto;



    private void Awake()
    {
        SetDefaultCursor();

        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    public void SetDefaultCursor()
    {
        Cursor.SetCursor(defaultCursor, hotspot, cursorMode);
    }

    public void SetHoverCursor()
    {
        Cursor.SetCursor(hoverCursor, hotspot, cursorMode);
    }
    public void SetCancelCursor()
    {
        Cursor.SetCursor(cancelCursor, hotspot, cursorMode);
    }
}