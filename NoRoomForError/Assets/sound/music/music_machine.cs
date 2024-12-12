using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class music_machine : MonoBehaviour
{
    public float volumeLevel = 0.5f;

    public List<AudioClip> audioTracks;
    public AudioSource track1;
    public AudioSource track2;
    public AudioSource track3;

    private bool track2Enabled = false;

    private bool track3Enabled = false;

    // Start is called before the first frame update
    void Start()
    {
        track1.clip = audioTracks[0];
        track1.volume = volumeLevel;
        track1.Play();
        track2.clip = audioTracks[1];
        track2.Play();
        track3.clip = audioTracks[2];
        track3.Play();
    }

    public void track2Start()
    {

        track2.volume = volumeLevel;
        track2Enabled = true;
    }

    public void track3Start()
    {
        track3.volume = volumeLevel;
        track3Enabled = true;
    }

    public void FixedUpdate()
    {
        track1.volume = volumeLevel;
        
        if(track2Enabled)
        {
            track2.volume = volumeLevel;
        }

        if(track3Enabled)
        {
            track3.volume = volumeLevel;
        }
    }


}
