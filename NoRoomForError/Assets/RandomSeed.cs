using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;
using System.Linq;

public class RandomSeed : MonoBehaviour
{
    public int seed = 0;
    public TextMeshProUGUI pauseSeedDisplay;
    public int rngLogic;
    [SerializeField]private int[] seedArr;
    public int currentIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        seed = PlayerPrefs.GetInt("seed");

        //UnityEngine.Random.InitState(seed);
        if (seed < 1000000 || seed > 9999999)
        {
            //seed = Random.Range(1000000, 9999999);
            seed = GetSevenDigitSeed();
            UnityEngine.Random.InitState(seed);
        }

        if (pauseSeedDisplay != null)
        {
            pauseSeedDisplay.text = "Seed: " + seed;

            seedArr = seed.ToString().Select(digit => int.Parse(digit.ToString())).ToArray();

            rngLogic = seedArr[0];
        }

        Debug.Log("Generated Seed: " + seed);
    }


    private int GetSevenDigitSeed()
    {
        // Generate a random seed based on a Guid and constrain it to 7 digits
        int seed = Math.Abs(Guid.NewGuid().GetHashCode());

        // Ensure it's exactly 7 digits by using modulo operation
        seed = 1000000 + (seed % 9000000); // Range: 1000000 to 9999999

        return seed;
    }

    public int NextRNG()
    {
        if (currentIndex >= 6)
        {
            currentIndex = 0;
        }
        else
        {
            currentIndex++;
        }

        return seedArr[currentIndex];
    }

    public int GetIndexValue()
    {
        return seedArr[currentIndex];
    }

}
