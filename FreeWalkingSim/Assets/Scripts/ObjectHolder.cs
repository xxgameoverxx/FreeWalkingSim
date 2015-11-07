using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectHolder : MonoBehaviour
{
    #region Singleton
    private static ObjectHolder objectHolder;

    public static ObjectHolder Instance()
    {
        if (!objectHolder)
        {
            objectHolder = FindObjectOfType(typeof(ObjectHolder)) as ObjectHolder;
            if (!objectHolder)
            {
                GameObject uiHolderObject = new GameObject("ObjectHolder");
                uiHolderObject.AddComponent<ObjectHolder>();
                objectHolder = uiHolderObject.GetComponent<ObjectHolder>();
            }
        }

        return objectHolder;
    }
    #endregion


    public GameObject tutorialPanel;
    public GameObject papers;
    public GameObject matches;
    public GameObject inventoryImage;
    public GameObject inventoryPanel;
    public GameManager manager;

    void Awake()
    {
        inventoryPanel = GameObject.FindGameObjectWithTag("InventoryPanel");
        tutorialPanel = GameObject.FindGameObjectWithTag("TutorialPanel");

        papers = Resources.Load<GameObject>("Prefabs/UnimportantPaper");
        inventoryImage = Resources.Load<GameObject>("Prefabs/UI/Base");
        manager = GameManager.Instance();
    }
}
