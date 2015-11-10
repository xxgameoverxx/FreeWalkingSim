using UnityEngine;
using System;
using System.Collections;

public class EncodedBook : Entity
{

    Action useNow;
    bool used = false;
    public AudioClip matchSound;
    public Entity note;

    void Start()
    {
        base.Start();
        useNow += UseNow;
    }

    public override bool CheckItems()
    {
        if (used)
            return true;

        foreach (Entity e in neededObjects)
        {
            Entity obj = Inventory.items.Find(en => en.Name == e.Name);
            if (obj == null)
            {
                uiHolder.WriteText(localizer.Get(e.Name) + " " + localizer.Get("objectNeeded"));
                return false;
            }
            else
            {
                obj.quantity--;
                if (obj.quantity == 0)
                    Inventory.Remove(obj);
            }
        }

        if (activationItem != string.Empty)
        {
            Entity e = Inventory.items.Find(t => t.Name == activationItem);
            if (e == null)
            {
                uiHolder.WriteText(localizer.Get(cannotUseText));
                return false;
            }
            else
            {
                Inventory.Remove(e);
            }
        }
        return true;
    }
    private void UseNow()
    {
        if (!CheckItems())
            uiHolder.WriteText(localizer.Get("encrypted"));
        else
            PostUse();

        if (!used && activated)
        {
            used = true;
            useSound.PlayOneShot(matchSound);
        }
    }

    private void PostUse()
    {

        if (Inventory.items.Contains(this))
            Inventory.Remove(this);
        if (objectToGet != null)
        {
            Inventory.Add(objectToGet);
            objectToGet.gameObject.SetActive(false);
        }
        activated = true;
        if (usedPopUpMessage != string.Empty)
        {
            ModalPanel.Instance().Register(new PopUpMessage(localizer.Get(usedPopUpMessage), localizer.Get(usedText), null, !used));
        }
        else
            uiHolder.WriteText(localizer.Get(usedText));
        if (useSound != null)
        {
            if (playOnce != null)
                useSound.PlayOneShot(playOnce);
            if (useSound.clip != null)
                useSound.PlayDelayed(delaySound);
        }
    }

    public override bool Use()
    {
        int i = 0;
        foreach (Entity e in activatedEntity)
        {
            if (!e.activated)
            {
                uiHolder.WriteText(localizer.Get(notActivatedMessage[i]));
                return false;
            }
            i++;
        }
        if (!used)
        {
            ModalPanel.Instance().Register(new PopUpMessage("", "", useNow));
            note.usable = true;
        }
        else
            useNow.Invoke();

        return true;
    }
}
