using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorial_walljump : MonoBehaviour
{
    Quaternion forwardRot = Quaternion.Euler(0, 90, 0);
    Quaternion backwardRot = Quaternion.Euler(0, 270, 0);

    public int direction = 1;

    public void ChangeDirection()
    {
        if (direction == 1)
        {
            TurnLeft();
        }
        else
        {
            TurnRight();
        }
    }
    public void TurnLeft()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, backwardRot, 200);
        direction = -1;
    }

    public void TurnRight()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, forwardRot, 200);
        direction = 1;
    }
}
