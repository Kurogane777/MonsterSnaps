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
            defaultStates.Invoke();

            if (!callOnce)
            {
                callOnceDefaltS.Invoke();
                callOnce = true;
            }
        }
        else
        {
            nonDefaultStates.Invoke();
            callOnce = false;
        }
    }
}
