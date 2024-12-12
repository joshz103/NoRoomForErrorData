using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFramerate : MonoBehaviour
{
    [SerializeField]private int targetFramerate = 120;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = targetFramerate;
        //QualitySettings.vSyncCount = 0;
    }
}
