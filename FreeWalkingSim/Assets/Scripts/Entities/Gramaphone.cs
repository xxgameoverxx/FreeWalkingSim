using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gramaphone : Entity
{

    public List<AudioClip> playlist = new List<AudioClip>();
    public AudioClip track;
    public AudioClip scratch;
    private AudioSource source;
    private AudioSource vinyl;
    private int trackNo = 0;

    // Use this for initialization
    void Start()
    {
        base.Start();
        source = GetComponent<AudioSource>();
        Transform vnl = transform.FindChild("vinyl");
        if (vnl != null)
            vinyl = vnl.GetComponent<AudioSource>();
    }

    private IEnumerator Playing()
    {
        while (trackNo != playlist.Count)
        {
            if (!source.isPlaying)
            {
                Debug.Log("Playing track no " + trackNo);
                source.clip = playlist[trackNo];
                trackNo++;
            }
            yield return null;
        }
        Debug.Log("Playlist ended");
        trackNo = 0;
        activated = false;
        source.Stop();
    }

    public override bool Use()
    {
        if(activated && base.Use())
        {
            //Debug.Log("stop");
            activated = false;
            source.Stop();
            if(scratch != null)
            {
                source.PlayOneShot(scratch);
            }
            if (vinyl != null)
                vinyl.Stop();
            //source.Stop();
            //trackNo = 0;
            return true;
        }
        else if(!activated && base.Use())
        {
            //Debug.Log("play");
            activated = true;
            if (scratch != null)
            {
                source.PlayOneShot(scratch);
            }
            if (vinyl != null)
                vinyl.Play();
            source.clip = track;
            source.Play();
            //StartCoroutine(Playing());
            return true;
        }
        return false;
    }
}
