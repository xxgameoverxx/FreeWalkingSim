using UnityEngine;
using System.Collections;
using System.IO;

public class Candle : Entity
{

    GameObject candleLight;

    void Start()
    {
        Name = "candleName";
        description = "candleDescription";
        base.Start();
        candleLight = transform.FindChild("CandleLight").gameObject;
        candleLight.SetActive(activated);
    }

    public override bool Use()
    {
        candleLight.SetActive(true);
        activated = true;
        return true;
    }

}
