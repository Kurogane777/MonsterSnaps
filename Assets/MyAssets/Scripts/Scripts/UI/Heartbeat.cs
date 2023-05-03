using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heartbeat : MonoBehaviour
{
    public Vector3 startScale, endScale;
    public bool repeatable = false;
    public float speed = 1f;  
    public float duration = 3f;  
    float startTime, totalDistance;

    void Awake()
    {
        startScale = transform.localScale;
    }

    void Start()
    {
        StartCoroutine(StartBeat());
    }

    public void callOnceBeat()
    {
        StartCoroutine(StartBeat());
    }

    IEnumerator StartBeat()
    {
        startTime = Time.time;
        totalDistance = Vector3.Distance(startScale, endScale);
        while (repeatable)
        {
            yield
            return RepeatLerp(startScale, endScale, duration);
            yield
            return RepeatLerp(endScale, startScale, duration);
        }
    }

    void Update()
    {
        if (!repeatable)
        {
            float currentDuration = (Time.time - startTime) * speed;
            float scaleFraction = currentDuration / totalDistance;
            this.transform.localScale = Vector3.Lerp(startScale, endScale, scaleFraction);
        }
    }
    public IEnumerator RepeatLerp(Vector3 a, Vector3 b, float time)
    {
        float i = 0.0f;
        float rate = (1.0f / time) *speed;
        while (i < 1.0f) {
            i += Time.deltaTime * rate;
            this.transform.localScale = Vector3.Lerp(a, b, i);
            yield
            return null;
        }
    }
}
