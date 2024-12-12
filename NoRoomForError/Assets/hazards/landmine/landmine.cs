using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class landmine : MonoBehaviour
{
    public GameObject explosion;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "PlayerAttachment")
        {
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(gameObject, 0.02f);
        }
    }


}
