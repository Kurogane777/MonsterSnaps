using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class pStates : MonoBehaviour
{
    public PlayerController pState;
    public UnityEvent callOnceDefaltS;
    public UnityEvent defaultStates;
    public UnityEvent nonDefaultStates;

    public bool callOnce;


    private void Update()
    {
        if (pState.currentState == PlayerController.State.movement)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PhotoBook.main.Toggle();
            }
            defaultStates.Invoke();

            if (!callOnce)
            {
                callOnceDefaltS.Invoke();
                callOnce = true;
            }
        }
        else
        {
            PhotoBook.main.Close();
            nonDefaultStates.Invoke();
            callOnce = false;
        }
    }
}
