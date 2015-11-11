using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIHolder : MonoBehaviour
{

    #region Singleton
    private static UIHolder uiHolder;

    public static UIHolder Instance()
    {
        if (!uiHolder)
        {
            uiHolder = FindObjectOfType(typeof(UIHolder)) as UIHolder;
            if (!uiHolder)
            {
                GameObject uiHolderObject = new GameObject("UIHolder");
                uiHolderObject.AddComponent<UIHolder>();
                uiHolder = uiHolderObject.GetComponent<UIHolder>();
            }
        }

        return uiHolder;
    }
    #endregion

    public Text console;
    private Localizer localizer;

    void Start()
    {
        console = GameObject.FindGameObjectWithTag("Console").GetComponent<Text>();
        localizer = Localizer.Instance();
    }

    void Update()
    {
        console.color = new Color(255, 255, 255, Mathf.Lerp(console.color.a, 0, Time.deltaTime));
    }

    public void WriteText(string newText, bool translate = true, bool talk = true)
    {
        string txt = newText;
        AudioClip voice = null;
        if (translate)
            txt = localizer.GetText(newText);
        if (talk)
            voice = localizer.GetSound(newText);
        console.color = new Color(255, 255, 255, 255);
        console.text = txt;
        if (voice != null)
            Voice.Say(voice);
    }
}
