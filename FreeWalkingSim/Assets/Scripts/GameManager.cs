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

    public GameObject hoveredGO;
    public enum HoverState { HOVER, NONE };
    public HoverState hover_state = HoverState.NONE;

    private float tutCountDown = 10;
    public bool inGame = false;
    public bool paused = false;

    public List<Entity> objectsToAddAtStart = new List<Entity>();
    private ModalPanel modalPanel;
    private ObjectHolder objectHolder;

    private Action showTutorialAction;
    private Action endGameAction;

    public GameObject goldenKey;

    void Start()
    {
        Screen.fullScreen = true;
        Screen.SetResolution(1920, 1080, true);
        CenterMouse.Instance().UpdateCursor();
        showTutorialAction += ShowTutorial;
        endGameAction += EndGame;
        if (Application.loadedLevelName == "GameScene")
            Init();
    }

    void OnSceneLoaded()
    {
        CenterMouse.Instance().UpdateCursor();
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

        modalPanel.Register(new PopUpMessage(Localizer.Instance().Get("storySoFar"), Localizer.Instance().Get("storySoFarComment"), showTutorialAction));

        OnPauseMenu();
    }

    public void ShowTutorial()
    {
        objectHolder.tutorialPanel.SetActive(true);
    }

    public void FirePlaceUsed()
    {
        GameObject.FindObjectOfType<PaperBox>().usable = true;
    }

    public void SpellBookUsed()
    {
        UIHolder.Instance().WriteText(Localizer.Instance().Get("keySpawn"));
        goldenKey.SetActive(true);
        foreach (Gramaphone g in FindObjectsOfType<Gramaphone>())
        {
            if (g.Name == "musicBoxName")
                g.activated = true;
        }
    }

    public void GoldenDoorOpened()
    {
        modalPanel.Register(new PopUpMessage(Localizer.Instance().Get("outro"), "", endGameAction));
    }

    private void EndGame()
    {
        Application.LoadLevel("MainMenu");
    }

    public void OnPauseMenu()
    {
        if (paused)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;

        if (objectHolder.tutorialPanel.activeSelf)
            objectHolder.tutorialPanel.SetActive(false);

        objectHolder.pauseMenu.SetActive(paused);
        objectHolder.player.enabled = !paused;
        CenterMouse.Instance().UpdateCursor();
    }

    public void OnNoiseToggled(bool val)
    {
        foreach (Camera c in FindObjectsOfType<Camera>())
        {
            c.GetComponent<NoiseAndScratches>().enabled = val;
        }
    }

    public void OnBlackAndWhiteToggled(bool val)
    {
        foreach (Camera c in FindObjectsOfType<Camera>())
        {
            c.GetComponent<Grayscale>().enabled = val;
        }
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
        Instantiate(objectHolder.loadingScreen);
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
        if (Input.GetKeyDown(KeyCode.Escape) && inGame)
        {
            paused = !paused;
            OnPauseMenu();
        }

        #region CenterMouse

        RaycastHit hitInfo = new RaycastHit();
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        if (Physics.Raycast(ray, out hitInfo))
        {
            if (hover_state == HoverState.NONE)
            {
                hoveredGO = hitInfo.collider.gameObject;
            }
            hover_state = HoverState.HOVER;
        }

        if(hitInfo.collider != null && (hitInfo.collider.gameObject != hoveredGO || hitInfo.distance > 3))
        {
            if (hover_state == HoverState.HOVER)
            {
                hoveredGO.SendMessage("MouseExit", SendMessageOptions.DontRequireReceiver);
            }
            hover_state = HoverState.NONE;
        }

        if (hover_state == HoverState.HOVER)
        {
            hitInfo.collider.SendMessage("MouseOver", SendMessageOptions.DontRequireReceiver); //Mouse is hovering
        }
        #endregion
    }

}
