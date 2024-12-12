using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    [Header("Properties")]
    public MeshRenderer objectRenderer;
    private Color originalColor;
    public Color hoverColor = Color.red;

    public int levelNumber;

    [Header("Camera Transition")]
    public Camera cam;
    public Transform targetPosition; // Target position to move to
    public float transitionDuration = 1.0f; // Time to complete the transition

    public Vector3 startPosition;
    private bool isTransitioning = false;

    [Header("UI")]
    public Image fadeImage;
    public float fadeDuration = 1.0f;

    void Start()
    {
        originalColor = objectRenderer.material.color;

        Color color = fadeImage.color;
        color.a = 0;
        fadeImage.color = color;
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
        StartCoroutine(FadeToWhite());
        startPosition = cam.transform.position;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject clickedObject = hit.transform.gameObject;
            Debug.Log("Clicked on: " + clickedObject.name);

            targetPosition = hit.transform;
        }
        
        StartTransition();
    }

    private IEnumerator WaitToStart()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(levelNumber);
    }

    //Camera Transition
    public void StartTransition()
    {
        if (!isTransitioning)
        {
            StartCoroutine(Transition());
        }
    }

    private IEnumerator Transition()
    {
        isTransitioning = true;
        float elapsedTime = 0;

        while (elapsedTime < transitionDuration)
        {
            // Calculate the new position as a percentage of the transition duration
            cam.transform.position = Vector3.Lerp(startPosition, targetPosition.position, elapsedTime / transitionDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(WaitToStart());
    }


    private IEnumerator FadeToWhite()
    {
        float timer = 0f;
        Color color = fadeImage.color;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(0, 1, timer / fadeDuration); // Interpolates alpha to 1
            fadeImage.color = color;
            yield return null;
        }

        // Ensure full white at the end
        color.a = 1;
        fadeImage.color = color;
    }



}
