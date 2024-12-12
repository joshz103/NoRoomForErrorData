using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class FlashingText : MonoBehaviour
{
    public Image text;
    public Color targetColor = Color.red;
    public float duration = 2f;

    private Color originalColor;

    private void Start()
    {
        originalColor = text.color;
    }

    private void Update()
    {
        float t = Mathf.PingPong(Time.time / duration, 1f);

        text.color = Color.Lerp(originalColor, targetColor, t);
    }
}
