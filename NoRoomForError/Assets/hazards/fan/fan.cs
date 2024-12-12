using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fan : MonoBehaviour
{
    public float pushForce = 0.1f;
    [SerializeField] private string direction = "up";
    public GameObject parentObj;
    private Vector3 dir = Vector3.up;
    private Vector3 offsetY = new Vector3(0, 1.2f, 0);

    public RandomSeed randomSeed;

    // Start is called before the first frame update
    void Start()
    {
        randomSeed = GameObject.FindGameObjectWithTag("Seed").GetComponent<RandomSeed>();

        //int randomDirection = Random.Range(0, 3);

        int randomDirection = 0;
        int randomValue = randomSeed.NextRNG();

        if (randomValue < 4) //If value is 0-3. SLIGHTLY more likely to be this direction (up)
        {
            randomDirection = 0;
        }
        else if (randomValue >= 4 && randomValue <= 6) //If value is 4-6 (right)
        {
            randomDirection = 1;
        }
        else
        {
            randomDirection = 2; //If value is 7-9 (left)
        }



        //float randomScale = Random.Range(0.9f, 1.5f);

        //parentObj.transform.localScale = new Vector3(randomScale, randomScale, randomScale);

        if (randomDirection == 0)
        {
            direction = "up";
            dir = Vector3.up;
        }
        if (randomDirection == 1)
        {
            direction = "right";
            parentObj.transform.rotation = Quaternion.Euler(0, 0, -90);
            dir = Vector3.right;
            parentObj.transform.localPosition += offsetY;
        }
        if (randomDirection == 2)
        {
            direction = "left";
            parentObj.transform.rotation = Quaternion.Euler(0, 0, 90);
            dir = Vector3.left;
            parentObj.transform.localPosition += offsetY;
        }


    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            rb.velocity += (dir * pushForce);
        }
    }
}
