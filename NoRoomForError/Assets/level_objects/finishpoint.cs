using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class finishpoint : MonoBehaviour
{
    GameObject player;
    GameObject spawnPoint;
    GameObject hazardSpawner;
    public int faceDirection = 1;

    private bool hasSpawned = false;

    private AudioSource audioSource;
    public AudioClip[] soundClips;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        spawnPoint = GameObject.FindGameObjectWithTag("Spawnpoint");
        hazardSpawner = GameObject.FindGameObjectWithTag("HazardSpawner");

        audioSource = GetComponent<AudioSource>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "PlayerAttachment")
        {
            player.transform.position = spawnPoint.transform.position;
            player.GetComponent<PlayerMovement>().direction = faceDirection;
            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            audioSource.clip = soundClips[0];
            audioSource.Play();
        }

        if (!hasSpawned)
        {
            hasSpawned = true;
            hazardSpawner.GetComponent<HazardSpawner>().spawnRandomHazard();
            Invoke("resetSpawn", 0.1f);
        }
    }

    public void resetSpawn()
    {
        hasSpawned = false;
        Debug.Log("Resetting...");
    }

}
