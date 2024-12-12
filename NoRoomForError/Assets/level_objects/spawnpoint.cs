using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnpoint : MonoBehaviour
{
    private GameObject player;
    public Transform spawnMarker;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        player.transform.position = spawnMarker.transform.position;
    }



}
