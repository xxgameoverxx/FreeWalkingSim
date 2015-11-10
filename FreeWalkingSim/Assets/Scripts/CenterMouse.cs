using UnityEngine;
using System.Collections;

public class CenterMouse : MonoBehaviour
{
    #region Singleton
    private static CenterMouse centerMouse;

    public static CenterMouse Instance()
    {
        if (!centerMouse)
        {
            centerMouse = FindObjectOfType(typeof(CenterMouse)) as CenterMouse;
            if (!centerMouse)
            {
                GameObject newCenterMouse = new GameObject("CenterMouse");
                newCenterMouse.AddComponent<CenterMouse>();
                centerMouse = newCenterMouse.GetComponent<CenterMouse>();
            }
        }

        return centerMouse;
    }
    #endregion

    public void Start()
    {
        UpdateCursor();
    }

    void OnLevelLoaded()
    {
        UpdateCursor();
    }

    void OnApplicationFocused()
    {
        UpdateCursor();
    }

    public void UpdateCursor()
    {
        Cursor.SetCursor(Resources.Load<Texture2D>("Sprites/amouse"), Vector2.zero, CursorMode.Auto);
        if (Application.loadedLevelName == "GameScene" && !GameManager.Instance().paused)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            UpdateCursor();
    }
}
