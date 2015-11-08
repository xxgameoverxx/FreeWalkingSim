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

    public void UpdateCursor()
    {
        if (Application.loadedLevelName == "GameScene" && !GameManager.Instance().paused)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            UpdateCursor();
    }
}
