using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SeedButton : MonoBehaviour
{
    [Header("Button")]
    public MeshRenderer objectRenderer;
    private Color originalColor;
    public Color hoverColor = Color.red;

    [Header("UI")]
    public GameObject SeedUI;
    public GameObject BackButton;

    [Header("Info")]
    public TMP_InputField textField;

    private bool isOpen = false;

    void Start()
    {
        originalColor = objectRenderer.material.color;
    }

    private void Update()
    {
        if (isOpen && Input.GetButtonDown("Pause"))
        {
            SeedMenuClose();
        }
    }

    void OnMouseEnter()
    {
        // Change the color when the mouse is over the object
        objectRenderer.material.color = hoverColor;
    }

    void OnMouseExit()
    {
        // Revert to the original color when the mouse leaves the object
        objectRenderer.material.color = originalColor;
    }

    private void OnMouseDown()
    {
        if (isOpen)
        {
            SeedMenuClose();
        }
        else
        {
            SeedMenuOpen();
        }
    }

    private void SeedMenuOpen()
    {
        isOpen = true;
        SeedUI.SetActive(true);
        BackButton.SetActive(false);
    }

    private void SeedMenuClose()
    {
        isOpen = false;
        SeedUI.SetActive(false);
        BackButton.SetActive(true);
    }

    public void ApplySeed()
    {
        PlayerPrefs.SetInt("seed", int.Parse(textField.text));
        Debug.Log("Set seed to " + int.Parse(textField.text));
    }
}
