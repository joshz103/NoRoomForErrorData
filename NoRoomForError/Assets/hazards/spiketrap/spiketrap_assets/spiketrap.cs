using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spiketrap : MonoBehaviour
{
    [SerializeField]private bool isTriggered = false;

    Vector3 startingPosition;
    Vector3 endingPosition;

    public AudioSource alarmSource;
    public AudioSource deploySource;

    public float movingTime = 0.5f;
    public float moveDistance;
    public float pauseTime;
    public float startTime;

    public GameObject spikes;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = new Vector3(spikes.transform.localPosition.x, spikes.transform.localPosition.y, spikes.transform.localPosition.z);
        endingPosition = new Vector3(spikes.transform.localPosition.x, spikes.transform.localPosition.y + moveDistance, spikes.transform.localPosition.z);
    }

    public void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.tag == "Player" || other.gameObject.tag == "PlayerAttachment") && !isTriggered)
        {
            alarmSource.Play();
            StartCoroutine(waitToMoveUp());
        }
    }

    IEnumerator MoveDown()
    {
        float elapsedTime = 0f;
        Vector3 currentPosition = spikes.transform.localPosition;

        while (elapsedTime < movingTime)
        {
            spikes.transform.localPosition = Vector3.Lerp(currentPosition, startingPosition, elapsedTime / movingTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        isTriggered = false;
    }

    IEnumerator MoveUp()
    {
        isTriggered = true;
        deploySource.Play();
        float elapsedTime = 0f;
        Vector3 currentPosition = spikes.transform.localPosition;

        while (elapsedTime < movingTime)
        {
            spikes.transform.localPosition = Vector3.Lerp(currentPosition, endingPosition, elapsedTime / movingTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(waitToMoveDown());
    }

    IEnumerator waitToMoveUp()
    {
        yield return new WaitForSeconds(startTime);
        StartCoroutine(MoveUp());
    }

    IEnumerator waitToMoveDown()
    {
        yield return new WaitForSeconds(pauseTime);
        StartCoroutine(MoveDown());
    }


}
