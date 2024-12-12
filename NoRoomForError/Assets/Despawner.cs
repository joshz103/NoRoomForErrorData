using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despawner : MonoBehaviour
{
    public float time;
    public bool despawnOnCollision = false;
    public float despawnOnCollisionTime = 0f;
    public bool removePlayerCollisionOnHit = false;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, time);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (despawnOnCollision)
        {
            if (removePlayerCollisionOnHit)
            {
                if (collision.gameObject.CompareTag("Player"))
                {
                    gameObject.layer = 9;
                }
            }


            Invoke("Delete", despawnOnCollisionTime);
        }
    }

    private void Delete()
    {
        Destroy(gameObject);
    }

}
