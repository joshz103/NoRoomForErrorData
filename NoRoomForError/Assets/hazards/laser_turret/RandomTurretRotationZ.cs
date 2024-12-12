using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RandomTurretRotationZ : MonoBehaviour
{
    public float maxZ;
    public float minZ;

    private float value;
    private float angle;
    //private float originalVal;

    public RandomSeed randomSeed;
    public HazardSpawner hazardSpawner;

    // Start is called before the first frame update
    void Start()
    {
        randomSeed = GameObject.FindGameObjectWithTag("Seed").GetComponent<RandomSeed>();
        hazardSpawner = GameObject.FindGameObjectWithTag("HazardSpawner").GetComponent<HazardSpawner>();
        value = (int)transform.position.x;

        //int rng = value * randomSeed.GetIndexValue();

        angle = (value * randomSeed.currentIndex / (hazardSpawner.roundNumber + 1)) * 10; //Gets a random angle based on the seed
        //angle = 10;
        //originalVal = angle;

        if (angle < minZ)
        {
            angle = minZ;
        }

        if (angle > maxZ)
        {
            angle = maxZ;
        }

        //float angle = Mathf.Lerp(minZ, maxZ, rng);

        //Debug.Log("Z rotation of turret is " + angle + " | Original value was " + originalVal);
        Debug.Log("Z rotation of turret is " + angle);

        //float random = Random.Range(minZ, maxZ);

        gameObject.transform.localRotation = Quaternion.Euler(0, 0, angle);
    }
}
