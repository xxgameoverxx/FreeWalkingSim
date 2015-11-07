using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public static class Inventory
{
    public static List<Entity> items = new List<Entity>();
    private static List<GameObject> panelVisuals = new List<GameObject>();

    public static void Add(Entity e)
    {
        items.Add(e);
        UpdatePanel();
    }

    public static void Remove(Entity e)
    {
        items.Remove(e);
        UpdatePanel();
    }

    private static void UpdatePanel()
    {
        foreach (GameObject g in panelVisuals)
        {
            GameObject.Destroy(g);
        }
        panelVisuals.Clear();
        foreach (Entity i in items)
        {
            GameObject g = GameObject.Instantiate(ObjectHolder.Instance().inventoryImage);
            g.transform.FindChild("Image").GetComponent<Image>().sprite = i.inventoryImage;
            g.transform.SetParent(ObjectHolder.Instance().inventoryPanel.transform);
            panelVisuals.Add(g);
        }
    }
}
