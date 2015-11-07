using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Local : MonoBehaviour
{

    string textName;
    Text txt;
    Localizer localizer;

    void Awake()
    {
        localizer = Localizer.Instance();
        txt = GetComponent<Text>();
        textName = txt.text;
        OnLaguageChange();
        localizer.lagaugeChangedAction += OnLaguageChange;
    }

    public void OnLaguageChange()
    {
        if (txt == null)
        {
            Debug.LogError("There is no text component on " + this);
            return;
        }
        txt.text = localizer.Get(textName, localizer.currentLanguage);
    }
}
