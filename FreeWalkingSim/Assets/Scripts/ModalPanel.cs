﻿using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class PopUpMessage
{
    public string message;
    public string afterMessage;
    public Action gotTheMessage = null;
    public bool fadeIn = false;

    private ModalPanel modalPanel;

    // Yes/No/Cancel: A string, a Yes event, a No event and Cancel event
    public PopUpMessage(string _message, string _afterMessage = "", Action _gotTheMessage = null, bool _fadeIn = false)
    {
        message = _message;
        afterMessage = _afterMessage;
        gotTheMessage = _gotTheMessage;
        fadeIn = _fadeIn;

        modalPanel = ModalPanel.Instance();
    }

    public IEnumerator Unregister()
    {
        yield return new WaitForEndOfFrame();
        modalPanel.Unregister(this);
    }
}

public class ModalPanel : MonoBehaviour
{
    public Text question;
    public GameObject modalPanelObject;
    private List<PopUpMessage> messageList = new List<PopUpMessage>();
    private Localizer localizer;
    private UIHolder uiHolder;

    private static ModalPanel modalPanel;

    public static ModalPanel Instance()
    {
        if (!modalPanel)
        {
            modalPanel = FindObjectOfType(typeof(ModalPanel)) as ModalPanel;
            if (!modalPanel)
                Debug.LogError("There needs to be one active ModalPanel script on a GameObject in your scene.");
        }

        return modalPanel;
    }

    void Init()
    {
        uiHolder = UIHolder.Instance();
    }

    void OnLevelLoaded()
    {
        if (Application.loadedLevelName == "GameScene")
            Init();
    }

    void Start()
    {
        if (Application.loadedLevelName == "GameScene")
            Init();
        localizer = Localizer.Instance();
        ClosePanel();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && messageList.Count > 0)
            StartCoroutine(messageList[0].Unregister());

        if (messageList.Count > 0)
        {
            Popup(messageList[0]);
            if (GameManager.Instance().inGame)
                ObjectHolder.Instance().player.enabled = false;
        }
        else
            ClosePanel();
    }

    private void ClearAllPanels()
    {
        messageList.Clear();
    }

    public void Register(PopUpMessage message)
    {
        messageList.Add(message);
    }

    public void Unregister(PopUpMessage message)
    {
        if (GameManager.Instance().inGame)
        {
            uiHolder.WriteText(message.afterMessage);
            ObjectHolder.Instance().player.enabled = true;
        }
        if (message.gotTheMessage != null)
            message.gotTheMessage.Invoke();

        messageList.Remove(message);
    }

    private IEnumerator FadeIn()
    {
        float duration = 2f; //0.5 secs
        float currentTime = 0f;
        while (currentTime < duration)
        {
            float alpha = Mathf.Lerp(0f, 1f, currentTime / duration);
            this.question.color = new Color(this.question.color.r, this.question.color.g, this.question.color.b, alpha);
            currentTime += Time.deltaTime;
            yield return null;
        }
        yield break;
    }

    private void Popup(PopUpMessage m)
    {
        modalPanelObject.SetActive(true);
        this.question.color = new Color(0, 0, 0, 0);
        this.question.text = m.message;
        if (m.fadeIn)
        {
            StartCoroutine(FadeIn());
        }
        else
            this.question.color = new Color(0, 0, 0, 255);

    }

    void ClosePanel()
    {
        modalPanelObject.SetActive(false);
    }
}
