using UnityEngine;
using System.Collections;

public class FirePlace : Entity
{

    GameObject torch;

    public void Start()
    {
        base.Start();
        Name = "fireplaceName";
        description = "fireplaceDescription";
        cannotUseText = "needPaper";
        neededObjects.Add(ObjectHolder.Instance().papers.GetComponent<Entity>());
        torch = transform.parent.FindChild("Torch").gameObject;
        torch.SetActive(activated);
    }

    public override bool Use()
    {
        if (base.Use())
        {
            Inventory.Remove(neededObjects[0]);
            torch.SetActive(true);
            GetComponent<AudioSource>().Play();
            activated = true;
            usable = false;
            return true;
        }
        else
        {
            uiHolder.WriteText(localizer.Get(cannotUseText));
            return false;
        }
    }
}
