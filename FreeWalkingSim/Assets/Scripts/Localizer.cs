using UnityEngine;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System;

public class Localizer : MonoBehaviour
{
    #region Singleton
    private static Localizer localizer;

    public static Localizer Instance()
    {
        if (!localizer)
        {
            localizer = FindObjectOfType(typeof(Localizer)) as Localizer;
            if (!localizer)
            {
                GameObject newLocalizer = new GameObject("Localizer");
                newLocalizer.AddComponent<Localizer>();
                localizer = newLocalizer.GetComponent<Localizer>();
            }
        }

        return localizer;
    }
    #endregion

    public string currentLanguage = "Tr";
    static Dictionary<string, LocalText> localTextDict = new Dictionary<string, LocalText>();

    public Action langaugeChangedAction;

    void Awake()
    {
        localTextDict = new Dictionary<string, LocalText>();
        ReadLaguageFile();
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
            ChangeLanguage("Tr");
        if (Input.GetKeyDown(KeyCode.Y))
            ChangeLanguage("En");
    }

    public string Get(string text, string language = "-", bool error = true)
    {
        if (language == "-")
            language = currentLanguage;

        if (!localTextDict.ContainsKey(text))
        {
            if (error)
                Debug.LogError("No text with such name: " + text);
            return "ERROR";
        }

        return localTextDict[text].Get(language);
    }

    public void ChangeLanguage(string language)
    {
        currentLanguage = language;
        langaugeChangedAction.Invoke();
    }

    void ReadLaguageFile()
    {
        XmlDocument xDoc = new XmlDocument();
        xDoc.Load(Application.dataPath + "/Resources/Localization.xml");

        foreach (XmlNode node in xDoc.ChildNodes)
        {
            if (node.Name == "Root")
            {
                foreach (XmlNode childNode in node.ChildNodes)
                {
                    if (localTextDict.ContainsKey(childNode.Attributes["name"].Value))
                    {
                        Debug.LogError(childNode.Attributes["name"].Value + " is a dublicate!");
                    }
                    localTextDict.Add(childNode.Attributes["name"].Value, new LocalText(childNode));
                }
            }
        }
    }
}

public class LocalText
{
    public LocalText(XmlNode node)
    {
        Name = node.Attributes["name"].Value;
        text = new Dictionary<string, string>();
        foreach (XmlNode childNode in node.ChildNodes)
        {
            text.Add(childNode.Name, childNode.InnerText);
            if (childNode.Name == "En")
                originalText = childNode.InnerText;
        }
    }

    public string Name;
    string originalText;
    Dictionary<string, string> text;

    public string Get(string language = "En")
    {
        string localText = text["En"];

        if (text[language] == null)
            Debug.LogError("No " + language + " translation of " + originalText);
        else
            localText = text[language];

        return localText;
    }
}