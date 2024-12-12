using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spring : MonoBehaviour
{
    public Vector3 strength = new Vector3(0, 3, 0);
    private Vector3 resetYVel = new Vector3(1, 0, 1);
    public Animator animator;

    public bool resetVelocity = true;

    private Rigidbody rbPlayer;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        rbPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            animator.Play("spring_anim");
            
            if (resetVelocity)
            {
                other.GetComponent<Rigidbody>().velocity = new Vector3(rbPlayer.velocity.x, 0f, rbPlayer.velocity.z);
            }

            other.GetComponent<Rigidbody>().velocity += strength;
            audioSource.Play();
        }
    }

}
