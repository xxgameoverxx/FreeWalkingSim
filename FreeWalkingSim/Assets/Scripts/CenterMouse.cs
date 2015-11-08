using UnityEngine;
using System.Collections;

public class CenterMouse : MonoBehaviour
{
    #region Singleton
    private static CenterMouse centerMouse;

    public GameObject hoveredGO;
    public enum HoverState { HOVER, NONE };
    public HoverState hover_state = HoverState.NONE;

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

    void UpdateCursor()
    {
        if (Application.loadedLevelName == "GameScene")
        {
            Cursor.lockState = CursorLockMode.Locked;
            //Cursor.visible = false;
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

         
     RaycastHit hitInfo = new RaycastHit();
     Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
             
     if(Physics.Raycast(ray, out hitInfo)){
         if(hover_state == HoverState.NONE){
             hitInfo.collider.SendMessage("OnMouseEnter", SendMessageOptions.DontRequireReceiver);
             hoveredGO = hitInfo.collider.gameObject;
         }
         hover_state = HoverState.HOVER;       
     }
     else{
         if(hover_state == HoverState.HOVER){
             hoveredGO.SendMessage("OnMouseExit", SendMessageOptions.DontRequireReceiver);
         }
         hover_state = HoverState.NONE;
     }
             
     if(hover_state == HoverState.HOVER){
         hitInfo.collider.SendMessage("OnMouseOver", SendMessageOptions.DontRequireReceiver); //Mouse is hovering
         if(Input.GetMouseButtonDown(0)){
             hitInfo.collider.SendMessage("OnMouseDown", SendMessageOptions.DontRequireReceiver); //Mouse down
         }
         if(Input.GetMouseButtonUp(0)){
             hitInfo.collider.SendMessage("OnMouseUp", SendMessageOptions.DontRequireReceiver); //Mouse up
         }
             
 }
    }
}
