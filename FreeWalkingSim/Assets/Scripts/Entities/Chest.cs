using UnityEngine;
using System.Collections;

public class Chest : Entity
{

    public BoxCollider toBeRemoved;

    // Use this for initialization
    void Start()
    {
        base.Start();
    }

    public override bool Use()
    {
        if (base.Use())
        {
            toBeRemoved.enabled = false;
            transform.parent.GetComponent<Animation>().Play("ChestAnim");
            activated = true;
            return true;
        }
        else
        {
            return false;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
