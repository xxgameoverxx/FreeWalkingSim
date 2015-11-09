using UnityEngine;
using System.Collections;

public class LoadingCanvas : MonoBehaviour {

	void OnLevelLoaded()
    {
        Destroy(this.gameObject);
    }
}
