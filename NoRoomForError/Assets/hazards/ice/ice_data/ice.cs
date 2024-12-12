using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ice : MonoBehaviour
{
    public PhysicMaterial playerMaterial;

    public float dynamicFrictionPlayer = 0.3f;
    public float staticFrictionPlayer = 0.95f;

    public float dynamicFrictionPlayerIce = 0.1f;
    public float staticFrictionPlayerIce = 0.5f;

    public PlayerMovement playerMovement;
    private bool cooldown = false;

    void Start()
    {
        playerMaterial.dynamicFriction = dynamicFrictionPlayer;
        playerMaterial.staticFriction = staticFrictionPlayer;
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (cooldown == false)
        {
            playerMaterial.staticFriction = staticFrictionPlayerIce;
            playerMaterial.dynamicFriction = dynamicFrictionPlayerIce;
            playerMovement.isOnIce = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        cooldown = true;
        playerMaterial.staticFriction = staticFrictionPlayer;
        playerMaterial.dynamicFriction = dynamicFrictionPlayer;
        playerMovement.isOnIce = false;
        StartCoroutine(ResetCooldown());
    }

    //Fix for trigger exit and stay happening on the same frame
    IEnumerator ResetCooldown()
    {
        yield return new WaitForSeconds(0.1f);
        cooldown = false;
    }

}
