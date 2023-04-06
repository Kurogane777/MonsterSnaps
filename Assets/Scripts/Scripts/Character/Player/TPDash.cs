using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPDash : MonoBehaviour
{
    ThirdPersonMovemnt moveScript;

    public float dashSpeed;
    public float dashTime;

    // Start is called before the first frame update
    void Start()
    {
        moveScript = GetComponent<ThirdPersonMovemnt>();
    }

    // Update is called once per frame
    public void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Dfunction();
        //}
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
