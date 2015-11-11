using UnityEngine;
using System.Xml.Linq;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

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

    public string currentLanguage = "En";
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

    public string GetText(string text, string language = "-", bool error = true)
    {
        if (language == "-")
            language = currentLanguage;

        if (!localTextDict.ContainsKey(text))
        {
            if (error)
                Debug.LogError("No text with such name: " + text);
            return "ERROR";
        }

        return localTextDict[text].GetText(language);
    }

    public AudioClip GetSound(string text, string language = "-", bool error = false)
    {
        if (language == "-")
            language = currentLanguage;

        if (!localTextDict.ContainsKey(text))
        {
            if (error)
                Debug.LogError("No text with such name: " + text);
            return null;
        }

        return localTextDict[text].GetSound(language, error);
    }

    public void ChangeLanguage(string language)
    {
        currentLanguage = language;
        langaugeChangedAction.Invoke();
    }

    void ReadLaguageFile()
    {
        XElement root = XElement.Load(Application.dataPath + "/Resources/Localization.xml");

        foreach (XElement x in root.Elements())
        {
            if (localTextDict.ContainsKey(x.Attribute("name").Value))
                Debug.LogError(x.Attribute("name").Value + " is a dublicate!");
            else
            {
                LocalText newText = new LocalText(x);
                StartCoroutine(LoadMusic(x, newText));
                localTextDict.Add(x.Attribute("name").Value, newText);
            }
        }
    }

    IEnumerator LoadMusic(XElement node, LocalText text)
    {
        AudioClip clip = null;
        foreach (XElement element in node.Elements())
        {
            string clipName = element.Name.ToString() + "_" + node.Attribute("name").Value;
            string path = Application.dataPath + "/Resources/Sound/Voice/" + clipName + ".wav";
            if (File.Exists(path))
            {
                WWW www = new WWW("file://" + path);
                while (!www.isDone)
                    yield return null;
                clip = www.GetAudioClip(false, true, AudioType.WAV);
                clip.name = clipName;
            }
            text.sound.Add(element.Name.ToString(), clip);
        }
    }
}

public class LocalText
{
    public LocalText(XElement node)
    {
        Name = node.Attribute("name").Value;
        text = new Dictionary<string, string>();
        sound = new Dictionary<string, AudioClip>();

        foreach (XElement element in node.Elements())
        {
            text.Add(element.Name.ToString(), element.Value);
            if (element.Name.ToString() == "En")
                originalText = element.Value;
        }
    }

    public string Name;
    string originalText;
    Dictionary<string, string> text;
    public Dictionary<string, AudioClip> sound;

    public string GetText(string language = "En")
    {
        string localText = text["En"];

        if (text[language] == null)
            Debug.LogError("No " + language + " translation of " + originalText);
        else
            localText = text[language];

        return localText;
    }

    public AudioClip GetSound(string language = "En", bool error = false)
    {
        AudioClip localSound = sound["En"];

        if (sound[language] == null && error)
            Debug.LogError("No " + language + " sound translation of " + originalText);
        else
            localSound = sound[language];

        return localSound;
    }
}