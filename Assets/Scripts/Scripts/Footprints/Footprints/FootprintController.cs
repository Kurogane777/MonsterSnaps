using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootprintController : MonoBehaviour
{
    public int step = 900;
    private Color matColor;

    public float speedSize = 1f;
    public float currentTime;

    public bool sizeAdded;

    void Start()
    {
        matColor = GetComponent<MeshRenderer>().material.color;
        StartCoroutine(Disappearing());

        currentTime = 0;
    }

    private void Update()
    {
        if (sizeAdded)
        {
            currentTime += Time.deltaTime;
            transform.localScale = transform.localScale + new Vector3(currentTime, currentTime, currentTime) * speedSize;
        }
    }

    IEnumerator Disappearing()
    {
        for (int i = 0; i < step; i++)
        {
            transform.GetChild(0).GetComponent<MeshRenderer>().material.color = new Color(matColor.r, matColor.g, matColor.b, matColor.a - (1.0f * i / step));
            yield return null;
        }
        Destroy(gameObject);
    }
}
