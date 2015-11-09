using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Characters.FirstPerson;

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
    public GameObject pauseMenu;
    public GameObject loadingScreen;
    public FirstPersonController player;
    public Toggle blackWhiteToggle;
    public Toggle noiseToggle;
    public GameManager manager;

    void Start()
    {

        papers = Resources.Load<GameObject>("Prefabs/UnimportantPaper");
        inventoryImage = Resources.Load<GameObject>("Prefabs/UI/Base");
        loadingScreen = Resources.Load<GameObject>("Prefabs/UI/LoadingCanvas");
        manager = GameManager.Instance();

        inventoryPanel = GameObject.FindGameObjectWithTag("InventoryPanel");
        tutorialPanel = GameObject.FindGameObjectWithTag("TutorialPanel");
        pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
        player = GameObject.FindObjectOfType<FirstPersonController>();

        if (GameObject.FindGameObjectWithTag("BlackWhiteToggle") != null)
        {
            blackWhiteToggle = GameObject.FindGameObjectWithTag("BlackWhiteToggle").GetComponent<Toggle>();
            noiseToggle = GameObject.FindGameObjectWithTag("NoiseToggle").GetComponent<Toggle>();
            noiseToggle.onValueChanged.AddListener((val) => manager.OnNoiseToggled(val));
            blackWhiteToggle.onValueChanged.AddListener((val) => manager.OnBlackAndWhiteToggled(val));
        }
    }
}
