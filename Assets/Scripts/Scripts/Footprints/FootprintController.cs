using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootprintController : MonoBehaviour
{
    public int step = 900;

    private Color matColor;

    void Start()
    {
        matColor = GetComponent<MeshRenderer>().material.color;

        StartCoroutine(Disappearing());
    }

    IEnumerator Disappearing()
    {
        for (int i = 0; i < step; i++)
        {
            GetComponent<MeshRenderer>().material.color = new Color(matColor.r, matColor.g, matColor.b, matColor.a - (1.0f * i / step));
            yield return null;
        }
        Destroy(gameObject);
    }
}
