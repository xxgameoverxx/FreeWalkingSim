using UnityEngine;
using System.Collections;


public class FirePlace : Entity
{

    GameObject torch;
    bool firstTime = true;

    public void Start()
    {
        base.Start();
        neededObjects.Add(ObjectHolder.Instance().papers.GetComponent<Entity>());
        torch = transform.parent.FindChild("Torch").gameObject;
        torch.SetActive(activated);
    }

    public override bool Use()
    {
        if (firstTime)
        {
            firstTime = false;
            GameManager.Instance().FirePlaceUsed();
        }
        if (base.Use())
        {
            Inventory.Remove(neededObjects[0]);
            torch.SetActive(true);
            GetComponent<AudioSource>().Play();
            activated = true;
            usable = false;
            return true;
        }
        else if(!activated)
        {
            uiHolder.WriteText(cannotUseText);
            return false;
        }
        return true;
    }
}
