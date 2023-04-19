using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPDash : MonoBehaviour
{
    ThirdPersonMovemnt moveScript;

    public float dashSpeed;
    public float dashTime;
    public float reloadTime = 0.5f;
    float currentReload;
    bool boolReload;

    void Start()
    {
        moveScript = GetComponent<ThirdPersonMovemnt>();
        currentReload = reloadTime;
    }

    private void Update()
    {
        if (boolReload)
        {
            currentReload -= Time.deltaTime;
        }

        if (currentReload <= 0.0f)
        {
            boolReload = false;
        }
    }

    public void Dfunction()
    {
        if (!boolReload)
        {
            StartCoroutine(Dash());
            currentReload = reloadTime;
            boolReload = true;
        }
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
