using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class pStates : MonoBehaviour
{
    public PlayerController pState;

    public UnityEvent callOnceDefaltS;

    public UnityEvent defaultStates;
    public UnityEvent damageStates;
    public UnityEvent croutchStates;
    public UnityEvent photoStates;
    public UnityEvent projectileStates;

    public UnityEvent nonDefaultStates;

    public bool boolCallOnceDefaltS;


    private void Update()
    {
        if (pState.currentState == PlayerController.State.movement)
        {
            defaultStates.Invoke();

            if (!boolCallOnceDefaltS)
            {
                callOnceDefaltS.Invoke();
                boolCallOnceDefaltS = true;
            }
        }
        else if (pState.currentState == PlayerController.State.damage)
        {
            damageStates.Invoke();
            NonDefaultState();
        }
        else if (pState.currentState == PlayerController.State.croutch)
        {
            croutchStates.Invoke();
            NonDefaultState();
        }
        else if (pState.currentState == PlayerController.State.photo)
        {
            photoStates.Invoke();
            NonDefaultState();
        }
        else if (pState.currentState == PlayerController.State.projectile)
        {
            projectileStates.Invoke();
            NonDefaultState();
        }
    }

    void NonDefaultState()
    {
        nonDefaultStates.Invoke();
        boolCallOnceDefaltS = false;
    }
}
