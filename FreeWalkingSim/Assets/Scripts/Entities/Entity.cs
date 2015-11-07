using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Characters.FirstPerson;

public class Entity : MonoBehaviour
{
    private FirstPersonController player;
    private Material mat;

    public Localizer localizer;
    public List<Entity> activatedEntity = new List<Entity>();
    public List<string> notActivatedMessage = new List<string>();
    public List<Entity> neededObjects = new List<Entity>();

    public Sprite inventoryImage;
    public GameObject objectToSpawn;
    public Entity objectToGet;
    public UIHolder uiHolder;
    public bool activated = false;
    public bool usable = true;
    public string activationItem;
    public string cannotUseText;
    public string usedText;
    public string description = "";
    public string Name = "";
    public float distanceToClick = 2;
    public float distanceToSee = 2;
    public int quantity = 1;

    public void Start()
    {
        player = GameObject.FindObjectOfType<FirstPersonController>();
        uiHolder = UIHolder.Instance();
        if (GetComponent<Renderer>() != null)
            mat = GetComponent<Renderer>().material;
        localizer = Localizer.Instance();
    }

    public void OnMouseOver()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < distanceToSee && mat != null)
            mat.SetColor("_EmissionColor", new Color(0.1f, 0.1f, 0.1f));

        if (Vector3.Distance(player.transform.position, transform.position) < distanceToClick)
        {
            if (Input.GetMouseButtonUp(1))
                Use();
            if (Input.GetMouseButtonUp(0))
                uiHolder.WriteText(localizer.Get(description));
        }
    }

    public void OnMouseExit()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < distanceToSee && mat != null)
            mat.SetColor("_EmissionColor", new Color(0, 0, 0));

    }

    public virtual bool CheckItems()
    {
        int i = 0;
        foreach (Entity e in activatedEntity)
        {
            if(!e.activated)
            {
                uiHolder.WriteText(localizer.Get(notActivatedMessage[i]));
                return false;
            }
            i++;
        }
        foreach (Entity e in neededObjects)
        {
            Entity obj = Inventory.items.Find(en => en.Name == e.Name);
            if(obj == null)
            {
                uiHolder.WriteText(e.Name + localizer.Get("objectNeeded"));
                return false;
            }
            else
            {
                obj.quantity--;
                if (obj.quantity == 0)
                    Inventory.Remove(obj);
            }
        }
        return true;
    }

    public virtual bool Use()
    {
        if (!usable || !CheckItems())
            return false;
        if (objectToGet != null)
            Inventory.Add(objectToGet);
        if (Inventory.items.Contains(this))
            Inventory.Remove(this);
        uiHolder.WriteText(localizer.Get(usedText));
        return true;
    }
}
