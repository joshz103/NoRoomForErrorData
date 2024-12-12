using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    public AudioClip[] audioClips;
    public AudioSource audioSource;
    public AudioSource audioSource2;

    public float pitchMin;
    public float pitchMax;

    public void PlayRandomFootstepSound()
    {
        int randomIndex = Random.Range(0, audioClips.Length);

        audioSource.clip = audioClips[randomIndex];
        audioSource.pitch = Random.Range(pitchMin, pitchMax);
        audioSource.Play();
    }

    public void PlayerJumpSound()
    {
        audioSource2.Play();
    }

    }
