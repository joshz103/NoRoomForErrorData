using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject item;
    public float spawnTimeMin = 1f;
    public float spawnTimeMax = 1f;
    private float spawnTime;
    //public Transform firingPoint;
    public float velocity;
    private bool hasSound = false;
    private AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        if (TryGetComponent<AudioSource>(out AudioSource audioSource))
        {
            hasSound = true;
            source = gameObject.GetComponent<AudioSource>();
        }

        StartCoroutine(spawnTimer());
    }

    IEnumerator spawnTimer()
    {
        spawnTime = Random.Range(spawnTimeMin, spawnTimeMax);
        yield return new WaitForSeconds(spawnTime);
        GameObject obj = Instantiate(item, transform.position, transform.rotation);
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        rb.velocity = transform.forward * velocity;

        if (hasSound)
        {
            source.Play();
        }


        StartCoroutine(spawnTimer());
    }
}
