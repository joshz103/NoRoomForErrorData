using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    GameObject player;
    private bool isPlayerDead = false;

    private GameObject ui;
    private Canvas deathui;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        ui = GameObject.FindGameObjectWithTag("UI");
        deathui = GameObject.FindGameObjectWithTag("DeathUI").GetComponent<Canvas>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.tag == "Player" || other.gameObject.tag == "PlayerAttachment") && !isPlayerDead)
        {
            isPlayerDead = true;
            player.GetComponent<PlayerMovement>().killPlayer();

            ui.SetActive(false);
            deathui.enabled = true;
        }
    }
}
