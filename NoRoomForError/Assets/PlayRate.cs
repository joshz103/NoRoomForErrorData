using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayRate : MonoBehaviour
{
    public VisualEffect effect;
    public float rate;

    // Start is called before the first frame update
    void Start()
    {
        effect.playRate = rate;
    }
}
