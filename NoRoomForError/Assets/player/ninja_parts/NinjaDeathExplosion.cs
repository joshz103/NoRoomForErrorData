using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaDeathExplosion : MonoBehaviour
{
    public GameObject[] ninjaParts;



    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < ninjaParts.Length; i++)
        {
            Vector3 RandomCircle = Random.onUnitSphere;

            var part = Instantiate(ninjaParts[i], transform.position, transform.rotation);
            part.GetComponent<Rigidbody>().velocity = RandomCircle * Random.Range(15, 40);
            Destroy(part, 3);
        }

        Destroy(gameObject, 0.1f);

    }

}
