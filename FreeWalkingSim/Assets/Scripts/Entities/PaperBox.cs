using UnityEngine;
using System.Collections;

public class PaperBox : Entity
{
    void Start()
    {
        base.Start();
        Name = "paperBoxName";
        description = "paperBoxDescription";
        usedText = "paperTaken";
        objectToGet = ObjectHolder.Instance().papers.GetComponent<Entity>();
    }

    public override bool Use()
    {
        if(base.Use())
        {
            usable = false;
            return true;
        }
        else
        {
            return false;
        }
    }
}
