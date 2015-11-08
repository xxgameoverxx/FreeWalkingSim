using UnityEngine;
using System.Collections;

public class SpellBook : Entity
{

    public BoxCollider toBeActivated;

    void Start()
    {
        base.Start();
        Name = "weirdBookName";
        description = "weirdBookDescription";
    }

    public override bool Use()
    {
        if (base.Use())
        {
            transform.parent.GetComponent<Animation>().Play("Open_Flat");
            toBeActivated.enabled = true;
            activated = true;
            usable = false;
            GameManager.Instance().SpellBookUsed();
            return true;
        }
        else
            return false;
    }
}
