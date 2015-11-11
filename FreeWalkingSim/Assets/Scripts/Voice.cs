using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Voice : MonoBehaviour
{
    private static List<AudioClip> talk = new List<AudioClip>();
    private static AudioSource source;
    private static string playing;

    // Use this for initialization
    void Start()
    {
        source = transform.FindChild("Voice").GetComponent<AudioSource>();
    }

    public static void Say(AudioClip ac)
    {
        if (!talk.Contains(ac) && playing != ac.name)
            talk.Add(ac);
    }

    // Update is called once per frame
    void Update()
    {
        if (talk.Count > 0 && !source.isPlaying)
        {
            playing = talk[0].name;
            source.PlayOneShot(talk[0]);
            talk.Remove(talk[0]);
        }
        else if (talk.Count == 0 && !source.isPlaying)
            playing = "";
    }
}
