using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

public class LevelStartRoutine : MonoBehaviour
{
    [Header("Setup")]
    public GameObject player;

    [Header("UI")]
    public Image fadeImage;
    public float fadeDuration = 1.0f;

    [Header("Camera")]
    public CinemachineVirtualCamera camera;
    public Transform[] cameraPoints;
    //public float transitionDuration = 1f;

    private bool hasSkipped = false;
    private bool canSkip = true;

    //[Header("Settings")]
    //public Settings settings;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        player = GameObject.FindGameObjectWithTag("Player");

        //settings.ApplySettings();

        Color color = fadeImage.color;
        color.a = 1;
        fadeImage.color = color;
        StartCoroutine(FadeToWhite());
        StartCoroutine(CameraTransition());
        player.GetComponent<PlayerMovement>().actionable = false;
        canSkip = true;
    }

    private void Update()
    {
        if (!hasSkipped && Input.GetButtonDown("Jump") && canSkip)
        {
            hasSkipped = true;
            StopAllCoroutines();
            CameraPlayer();
            StartCoroutine(FadeToWhite());
        }
    }


    private IEnumerator FadeToWhite()
    {
        float timer = 0f;
        Color color = fadeImage.color;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(1, 0, timer / fadeDuration); // Interpolates alpha to 1
            fadeImage.color = color;
            yield return null;
        }

        // Ensure full white at the end
        color.a = 0;
        fadeImage.color = color;
    }

    private IEnumerator CameraTransition()
    {
        float elapsedTime = 0;

        for (int i = 0; i < cameraPoints.Length; i++)
        {
            camera.Follow = cameraPoints[i].transform;
            Debug.Log("Transitioning camera to " + cameraPoints[i].name);

            if (cameraPoints[i].GetComponent<CameraMarker>().lookAtPlayer)
            {
                camera.LookAt = player.transform;
                camera.AddCinemachineComponent<CinemachineComposer>();
                camera.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance = cameraPoints[i].GetComponent<CameraMarker>().cameraDistance;
            }
            else
            {
                camera.LookAt = null;
                camera.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance = 11;
                camera.DestroyCinemachineComponent<CinemachineComposer>();
            }    

            if(cameraPoints[i].GetComponent<CameraMarker>().animatePlayer == true)
            {
                canSkip = false;
                player.GetComponent<Animator>().SetInteger("randomready", Random.Range(0, 3));
                player.GetComponent<Animator>().SetTrigger("ready");
            }

            while (elapsedTime < cameraPoints[i].GetComponent<CameraMarker>().cameraStayTime)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            elapsedTime = 0;
        }

        CameraPlayer();
    }

    private void CameraPlayer()
    {
        camera.LookAt = null;
        camera.DestroyCinemachineComponent<CinemachineComposer>();
        camera.AddCinemachineComponent<CinemachinePOV>();
        //camera.m_Lens.FieldOfView = 60;
        camera.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance = 11;
        camera.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = 0;
        camera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = 0;
        camera.Follow = player.transform;
        player.GetComponent<PlayerMovement>().actionable = true;
    }



}
