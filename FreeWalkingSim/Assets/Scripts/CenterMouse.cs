using UnityEngine;
using System.Collections;

public class CenterMouse : MonoBehaviour
{

    void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
