using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gramaphone : Entity
{

    public List<AudioClip> playlist = new List<AudioClip>();
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

    public override bool Use()
    {
        if (activated && base.Use())
        {
            activated = false;
            source.Stop();
            if (scratch != null)
            {
                source.PlayOneShot(scratch);
            }
            if (vinyl != null)
                vinyl.Stop();
            trackNo = 0;
            return true;
        }
        else if (!activated && base.Use())
        {
            activated = true;
            if (scratch != null)
            {
                source.PlayOneShot(scratch);
            }
            if (vinyl != null)
                vinyl.Play();
            return true;
        }
        return false;
    }

    void Update()
    {
        if (activated && !source.isPlaying)
        {
            if (trackNo == playlist.Count)
            {
                source.Stop();
                activated = false;
                if (vinyl != null)
                    vinyl.Stop();
                trackNo = 0;
            }
            source.clip = playlist[trackNo];
            source.Play();
            trackNo++;
        }
    }
}
