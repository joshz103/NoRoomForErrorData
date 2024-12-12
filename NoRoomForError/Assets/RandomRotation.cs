using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotation : MonoBehaviour
{
    //public RandomSeed randomSeed;
    public RandomSeed randomSeed;
    private int random;

    // Start is called before the first frame update
    void Start()
    {
        randomSeed = GameObject.FindGameObjectWithTag("Seed").GetComponent<RandomSeed>();
        //Random.InitState(GameObject.FindGameObjectWithTag("Seed").GetComponent<RandomSeed>().seed);
        //int random = Random.Range(0, 2);
        int rng = randomSeed.NextRNG();

        if (rng % 2 == 0)
        {
            random = 0;
            Debug.Log("Seed digit " + randomSeed.GetIndexValue() + " made object face right");
        }
        else
        {
            random = 1;
            Debug.Log("Seed digit " + randomSeed.GetIndexValue() + " made object face left");
        }

        if (random == 1)
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }
}
