using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Characters.FirstPerson;

public class Entity : MonoBehaviour
{
    private FirstPersonController player;
    private Material mat;

    protected Localizer localizer;
    protected UIHolder uiHolder;
    public List<Entity> activatedEntity = new List<Entity>(); //entities which needs to be activated = true in order to use this entity
    public List<string> notActivatedMessage = new List<string>(); //messages to write if an entity in the list above is not activated (order is important)
    public List<Entity> neededObjects = new List<Entity>(); //objects which are needed to be in the inventory to use this entity

    public string Name = "";
    public string description = "";
    public string usedText;
    public string usedPopUpMessage;
    public string cannotUseText;
    public string activationItem;
    public bool activated = false;
    public bool usable = true;
    public float distanceToSee = 2;
    public float distanceToClick = 2;
    public GameObject objectToSpawn;
    public Entity objectToGet;
    public Sprite inventoryImage;
    protected AudioSource useSound;
    public AudioClip playOnce;
    public float delaySound = 0;
    public int quantity = 1;
    public Color shineColor = new Color(0.1f, 0.1f, 0.1f);


    public void Start()
    {
        player = GameObject.FindObjectOfType<FirstPersonController>();
        useSound = GetComponent<AudioSource>();
        uiHolder = UIHolder.Instance();
        if (GetComponent<Renderer>() != null)
            mat = GetComponent<Renderer>().material;
        localizer = Localizer.Instance();
    }

    public void OnMouseOver()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < distanceToSee && mat != null)
        {
            mat.SetColor("_EmissionColor", shineColor);
        }

        if (Vector3.Distance(player.transform.position, transform.position) < distanceToClick)
        {
            if (Input.GetMouseButtonUp(1))
                Use();
            if (Input.GetMouseButtonDown(0))
                uiHolder.WriteText(localizer.Get(description));
        }
    }

    public void OnMouseExit()
    {
        mat.SetColor("_EmissionColor", new Color(0, 0, 0));
    }

    public virtual bool CheckItems()
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

    public virtual bool Use()
    {
        if (!usable || !CheckItems())
            return false;
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
            ModalPanel.Instance().Register(new PopUpMessage(localizer.Get(usedPopUpMessage), localizer.Get(usedText)));
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
        return true;
    }
}
