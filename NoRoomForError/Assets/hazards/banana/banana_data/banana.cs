using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class banana : MonoBehaviour
{
    public Animator bananaAnim;
    public PlayerMovement playerMovement;
    public float slipDuration = 1f;
    public float speed;
    public float height;
    public AudioSource audioSource;

    private void Start()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bananaAnim.Play("banana");
            audioSource.Play();
            playerMovement.Slip(slipDuration, speed, height);
        }
    }
}
