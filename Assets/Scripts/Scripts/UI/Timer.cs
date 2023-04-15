using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    public float targetTime = 60.0f;
    public UnityEvent timerEnd;

    void Update()
    {

        targetTime -= Time.deltaTime;

        if (targetTime <= 0.0f)
        {
            timerEnded();
        }

    }

    void timerEnded()
    {
        timerEnd.Invoke();
    }

    public void ObjDestroy()
    {
        Destroy(this.gameObject);
    }
}
