using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    public TextMeshProUGUI fpsCounter;

    private int lastFrameIndex;
    private float[] frameDeltaTimeArray;
    //public int maxFPS = 500;

    // Start is called before the first frame update
    void Start()
    {
        frameDeltaTimeArray = new float[50];
        //QualitySettings.vSyncCount = 1;
        //Application.targetFrameRate = maxFPS;
    }

    // Update is called once per frame
    void Update()
    {
        frameDeltaTimeArray[lastFrameIndex] = Time.deltaTime;
        lastFrameIndex = (lastFrameIndex + 1) % frameDeltaTimeArray.Length;

        fpsCounter.text = Mathf.RoundToInt(CalculateFPS()).ToString();


        //fpsCounter.text = ((int)(1f / Time.deltaTime)).ToString();
    }

    private float CalculateFPS()
    {
        float total = 0f;

        foreach (float deltaTime in frameDeltaTimeArray)
        {
            total += deltaTime;
        }
        return frameDeltaTimeArray.Length / total;
    }



}
