using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screenshake : MonoBehaviour
{
    public static float ShakeAmount = 0;
    Vector3 StartingPosition;

    public float _SA = 0;

    void Start()
    {
        StartingPosition = transform.position;
    }

    void Update()
    {
        ShakeAmount = Mathf.Lerp(ShakeAmount, 0, 0.02f);
        transform.position = StartingPosition + Random.onUnitSphere * ShakeAmount;
    }

    public void SA()
    {
        ShakeAmount = _SA;
    }
}
