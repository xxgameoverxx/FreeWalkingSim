using UnityEngine;
using System.Collections;

public class LookUp : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(270, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }
}
