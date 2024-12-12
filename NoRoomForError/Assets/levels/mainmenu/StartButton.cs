using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    [Header("Button")]
    public MeshRenderer objectRenderer;
    private Color originalColor;
    public Color hoverColor = Color.red; // Change to your preferred color

    [Header("Camera Transition")]
    public Camera cam;
    public Transform targetPosition; // Target position to move to
    public float transitionDuration = 2.0f; // Time to complete the transition

    public Transform startPosition;
    private bool isTransitioning = false;

    public GameObject title;
    public GameObject startButton;
    public GameObject optionsButton;

    public CameraSway sway;

    void Start()
    {
        // Get the Renderer component and store the original color
        //objectRenderer = GetComponent<Renderer>();
        originalColor = objectRenderer.material.color;
        //startPosition = cam.transform.position; // Initial position of the camera
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

        startButton.SetActive(false);
        title.SetActive(false);
        optionsButton.SetActive(false);

        while (elapsedTime < transitionDuration)
        {
            // Calculate the new position as a percentage of the transition duration
            cam.transform.position = Vector3.Lerp(startPosition.position, targetPosition.position, elapsedTime / transitionDuration);

            Quaternion startRot = startPosition.rotation;
            Quaternion targetRot = targetPosition.rotation;

            cam.transform.rotation = Quaternion.Lerp(startRot, targetRot, elapsedTime / transitionDuration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final position is set to the target
        cam.transform.position = targetPosition.position;
        isTransitioning = false;
    }

    private void OnMouseDown()
    {
        sway.on = false;
        StartTransition();
    }

}
