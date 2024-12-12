using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class static_saw : MonoBehaviour
{
    public Vector3 startingPosition;
    //public float moveSpeed = 1;
    [SerializeField]private float distance = 0;
    public Vector3 moveSpeed = new Vector3(1, 0, 0);
    public float maxDistanceTravelled = 10;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position += (moveSpeed * Time.deltaTime);
        distance = Vector3.Distance(startingPosition, transform.position);

        if (distance >= maxDistanceTravelled)
        {
            transform.position = startingPosition;
        }
    }
}
