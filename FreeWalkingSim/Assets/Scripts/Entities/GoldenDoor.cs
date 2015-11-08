using UnityEngine;
using System.Collections;

public class GoldenDoor : Entity
{
    public override bool Use()
    {
        if(base.Use())
        {
            GameManager.Instance().GoldenDoorOpened();
            return true;
        }
        return false;
    }
}
