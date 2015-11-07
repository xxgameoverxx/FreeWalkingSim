using UnityEngine;
using System.Collections;

public class Candle : Entity
{

    GameObject candleLight;

    void Start()
    {
        base.Start();
        Name = "candleName";
        description = "candleDescription";
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
