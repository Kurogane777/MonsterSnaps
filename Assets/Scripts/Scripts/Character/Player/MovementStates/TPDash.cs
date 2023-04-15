using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPDash : MonoBehaviour
{
    ThirdPersonMovemnt moveScript;

    public float dashSpeed;
    public float dashTime;

    void Start()
    {
        moveScript = GetComponent<ThirdPersonMovemnt>();
    }

    public void Dfunction()
    {
        StartCoroutine(Dash());
    }

    IEnumerator Dash()
    {
        float startTime = Time.time;

        while(Time.time < startTime + dashTime)
        {
            moveScript.characterController.Move(moveScript.movementDirection * dashSpeed * Time.deltaTime);

            yield return null;
        }

    }
}
