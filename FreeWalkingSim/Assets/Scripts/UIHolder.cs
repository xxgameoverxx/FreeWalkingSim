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

    void Start()
    {
        console = GameObject.FindGameObjectWithTag("Console").GetComponent<Text>();
    }

    void Update()
    {
        console.color = new Color(255, 255, 255, Mathf.Lerp(console.color.a, 0, Time.deltaTime));
    }

    public void WriteText(string newText)
    {
            console.color = new Color(255, 255, 255, 255);
            console.text = newText;
    }
}
