using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.ImageEffects;

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
    public bool paused = false;

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
        CenterMouse.Instance();
        modalPanel = ModalPanel.Instance();
        objectHolder = ObjectHolder.Instance();

        objectHolder.tutorialPanel.SetActive(false);

        Inventory.items.Clear();
        foreach (Entity e in objectsToAddAtStart)
        {
            Inventory.Add(e);
        }

        modalPanel.Register(new PopUpMessage("storySoFar", "storySoFarComment", showTutorialAction));

        OnPauseMenu();
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

    public void OnPauseMenu()
    {
        if (paused)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
        objectHolder.pauseMenu.SetActive(paused);
        objectHolder.player.enabled = !paused;
        CenterMouse.Instance().UpdateCursor();
    }

    public void OnNoiseToggled(bool val)
    {
        Camera.main.GetComponent<NoiseAndScratches>().enabled = val;
    }

    public void OnBlackAndWhiteToggled(bool val)
    {
        Camera.main.GetComponent<Grayscale>().enabled = val;
    }

    public void OnTurkishClicked()
    {
        Localizer.Instance().ChangeLanguage("Tr");
    }

    public void OnEnglishClicked()
    {
        Localizer.Instance().ChangeLanguage("En");
    }

    public void OnMenuClicked()
    {
        Application.LoadLevel("MainMenu");
    }

    public void OnQuitClicked()
    {
        Application.Quit();
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
            paused = !paused;
            OnPauseMenu();
        }
    }

}
