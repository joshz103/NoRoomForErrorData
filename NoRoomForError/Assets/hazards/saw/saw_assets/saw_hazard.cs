using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class saw_hazard : MonoBehaviour
{
    Vector3 startingPosition;
    Vector3 endingPosition;

    public float movingTime = 3f;
    public float moveDistance;
    public float pauseTime;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        endingPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + moveDistance, gameObject.transform.position.z);

        StartCoroutine(MoveDown());
    }

    IEnumerator MoveUp()
    {
        float elapsedTime = 0f;
        Vector3 currentPosition = transform.position;

        while (elapsedTime < movingTime)
        {
            transform.position = Vector3.Lerp(currentPosition, startingPosition, elapsedTime / movingTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        //transform.position = startingPosition;

        //StartCoroutine(MoveDown());
        StartCoroutine(waitToMoveDown());
    }

    IEnumerator MoveDown()
    {
        float elapsedTime = 0f;
        Vector3 currentPosition = transform.position;

        while (elapsedTime < movingTime)
        {
            transform.position = Vector3.Lerp(currentPosition, endingPosition, elapsedTime / movingTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        //transform.position = endingPosition;

        //StartCoroutine(MoveUp());
        StartCoroutine(waitToMoveUp());
    }

    IEnumerator waitToMoveUp()
    {
        yield return new WaitForSeconds(pauseTime);
        StartCoroutine(MoveUp());
    }

    IEnumerator waitToMoveDown()
    {
        yield return new WaitForSeconds(pauseTime);
        StartCoroutine(MoveDown());
    }

}