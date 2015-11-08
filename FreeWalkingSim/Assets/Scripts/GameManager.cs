using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{

    #region Singleton
    private static GameManager gameManager;

    public static GameManager Instance()
    {
        if (!gameManager)
        {
            gameManager = FindObjectOfType(typeof(GameManager)) as GameManager;
            if (!gameManager)
            {
                GameObject gameManagerObject = new GameObject("GameManager");
                gameManagerObject.AddComponent<GameManager>();
                gameManager = gameManagerObject.GetComponent<GameManager>();
            }
        }

        return gameManager;
    }
    #endregion

    private float tutCountDown = 10;
    public bool inGame = false;

    public List<Entity> objectsToAddAtStart = new List<Entity>();
    private ModalPanel modalPanel;
    private ObjectHolder objectHolder;

    private Action showTutorialAction;

    public GameObject goldenKey;

    void Start()
    {
        CenterMouse.Instance().Start();
        showTutorialAction += ShowTutorial;
        if (Application.loadedLevelName == "GameScene")
            Init();
    }

    void OnSceneLoaded()
    {
        if (Application.loadedLevelName == "GameScene")
            Init();
        else
            inGame = false;
    }

    public void Init()
    {
        inGame = true;
        modalPanel = ModalPanel.Instance();
        objectHolder = ObjectHolder.Instance();

        objectHolder.tutorialPanel.SetActive(false);

        Inventory.items.Clear();
        foreach (Entity e in objectsToAddAtStart)
        {
            Inventory.Add(e);
        }

        modalPanel.Register(new PopUpMessage("storySoFar", "storySoFarComment", showTutorialAction));
    }

    public void ShowTutorial()
    {
        objectHolder.tutorialPanel.SetActive(true);
    }

    public void SpellBookUsed()
    {
        Debug.Log("Shit happens");
        goldenKey.SetActive(true);
    }

    void Update()
    {
        if (inGame && objectHolder.tutorialPanel.activeSelf)
        {
            if (tutCountDown < 0)
                objectHolder.tutorialPanel.SetActive(false);
            else
                tutCountDown -= Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.LoadLevel("MainMenu");
        }
    }

}
