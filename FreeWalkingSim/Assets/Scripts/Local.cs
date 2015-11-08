using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class Local : MonoBehaviour
{

    string textName;
    Text txt;
    Localizer localizer;

    void Start()
    {
        localizer = Localizer.Instance();
        txt = GetComponent<Text>();
        textName = txt.text;
        OnLaguageChange();
        localizer.langaugeChangedAction += OnLaguageChange;
    }

    public void OnLaguageChange()
    {
        if (this == null)
        {
            localizer.langaugeChangedAction -= OnLaguageChange;
            return;
        }
        if (txt == null)
        {
            Debug.LogError("There is no text component on " + this);
            return;
        }
        txt.text = localizer.Get(textName, localizer.currentLanguage);
    }
}
