using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class RoundRestarter : MonoBehaviour
{
    public PlayerMovement player;
    private bool canRestart = false;
    private bool isRestarting = false;
    public Volume deathscreenfx;

    private float currentValue = 0f;
    private bool isTransitioning = false;

    private void Start()
    {
        deathscreenfx = GameObject.FindGameObjectWithTag("deathscreenfx").GetComponent<Volume>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.isDead && !isRestarting)
        {
            isRestarting = true;
            StartCoroutine(Wait());
        }

        if (player.isDead && Input.GetButtonDown("Jump"))
        {
            if (canRestart)
            {
                RestartRound();
            }
        }

        if (player.isDead && !isTransitioning)
        {
            isTransitioning = true;
            StartCoroutine(DeathScreenFXTransition(1));
        }

    }

    public void RestartRound()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        //SceneManager.LoadScene(currentScene.name);
        StartCoroutine(ReloadCurrentScene());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.5f);
        canRestart = true;
    }

    private IEnumerator ReloadCurrentScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    private IEnumerator DeathScreenFXTransition(float duration)
    {
        float startValue = 0f;
        float endValue = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            currentValue = Mathf.Lerp(startValue, endValue, elapsedTime / duration);
            deathscreenfx.weight = currentValue;
            yield return null;
        }

        currentValue = endValue;
    }

    }
