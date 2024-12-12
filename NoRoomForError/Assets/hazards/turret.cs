using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class turret : MonoBehaviour
{
    public GameObject projectile;
    public Transform firingPoint;
    public float projectileVelocity = 1;
    public float firingCooldown = 1;
    public float firingWarmupTime = 1;
    public bool disableLaserWhenTriggered = false;
    public MeshRenderer laserMesh;
    private bool canFire = true;
    private bool canBeTriggered = true;

    public bool firesWhenTriggered = false;

    public AudioSource fireSource;
    public AudioSource triggerSource;

    public void fire()
    {
        GameObject obj =  Instantiate(projectile, firingPoint.transform.position, Quaternion.identity);

        Rigidbody rb = obj.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.velocity = projectileVelocity * firingPoint.forward;
            rb.transform.rotation = Quaternion.LookRotation(rb.velocity) * Quaternion.Euler(0, 90, 0);
        }
    }


    public void OnTriggerStay(Collider other)
    {
        if (canBeTriggered)
        {
            if ((other.gameObject.tag == "Player" || other.gameObject.tag == "PlayerAttachment") && firesWhenTriggered && canFire)
            {
                canBeTriggered = false;
                triggerSource.Play();
                canFire = false;
                StartCoroutine(warmupTime());

                if (disableLaserWhenTriggered)
                {
                    StartCoroutine(disableLaser());
                }

            }
        }
    }

    private void Update()
    {
        if (!firesWhenTriggered && canFire)
        {
            canFire = false;
            StartCoroutine(warmupTime());
        }
    }

    IEnumerator cooldownReset()
    {
        yield return new WaitForSeconds(firingCooldown);
        canFire = true;
        canBeTriggered = true;
        if (disableLaserWhenTriggered)
        {
            laserMesh.enabled = true;
        }
    }

    IEnumerator warmupTime()
    {
        yield return new WaitForSeconds(firingWarmupTime);
        fire();
        fireSource.Play();
        StartCoroutine(cooldownReset());
    }

    IEnumerator disableLaser()
    {
        yield return new WaitForSeconds(0.25f);
        laserMesh.enabled = false;
    }


}
