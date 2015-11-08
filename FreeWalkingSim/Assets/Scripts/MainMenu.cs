﻿using UnityEngine;
using System.Collections;
using System;

public class MainMenu : MonoBehaviour
{
    public void OnNewGameClicked()
    {
        Application.LoadLevel("GameScene");
    }

    public void OnCreditsClicked()
    {
        Credits();
    }

    public void OnQuitClicked()
    {
        Application.Quit();
    }

    public void OnTurkishClicked()
    {
        Localizer.Instance().ChangeLanguage("Tr");
    }

    public void OnEnglishClicked()
    {
        Localizer.Instance().ChangeLanguage("En");
    }

    public void Torreng()
    {
        Application.OpenURL("http://torrenglabs.com/");
    }

    public void EBergi()
    {
        Application.OpenURL("http://e-bergi.com/");
    }

    private void Credits()
    {
        string credits = Localizer.Instance().Get("credits", "-", false);
        string usedAssets = Localizer.Instance().Get("usedAssets", "-", false);
        string usedAssets2 = Localizer.Instance().Get("usedAssets2", "-", false);
        string usedAssets3 = Localizer.Instance().Get("usedAssets3", "-", false);
        string thanks = Localizer.Instance().Get("thanks", "-", false);
        string developer = Localizer.Instance().Get("anton", "-", false);
        

        if (credits == "ERROR")
            credits = "lol";
        if (usedAssets == "ERROR")
            usedAssets = "lol";
        if (usedAssets2 == "ERROR")
            usedAssets2 = "lol";
        if (usedAssets3 == "ERROR")
            usedAssets3 = "lol";
        if (thanks == "ERROR")
            thanks = "lol";
        if (developer == "ERROR")
            developer = "lol";

        credits = RemoveWhitespace(credits);
        ModalPanel.Instance().Register(new PopUpMessage(credits));
        usedAssets = RemoveWhitespace(usedAssets);
        ModalPanel.Instance().Register(new PopUpMessage(usedAssets));
        usedAssets2 = RemoveWhitespace(usedAssets2);
        ModalPanel.Instance().Register(new PopUpMessage(usedAssets2));
        usedAssets3 = RemoveWhitespace(usedAssets3);
        ModalPanel.Instance().Register(new PopUpMessage(usedAssets3));
        thanks = RemoveWhitespace(thanks);
        ModalPanel.Instance().Register(new PopUpMessage(thanks));
        developer = RemoveWhitespace(developer);
        ModalPanel.Instance().Register(new PopUpMessage(developer));
    }

    public string RemoveWhitespace(string str)
    {
        string line = str.Replace("\t", " ");
        while (line.IndexOf("  ") > 0)
        {
            line = line.Replace("  ", " ");
        }
        return line;
        //return string.Join(" ", str.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
    }
}