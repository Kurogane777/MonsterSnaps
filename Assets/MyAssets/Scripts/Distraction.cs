using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Distraction : MonoBehaviour
{
    public float radius;
    public float attractTime;
    // Start is called before the first frame update
    void Start()
    {
        var cols = Physics.OverlapSphere(transform.position, radius);
        foreach (var col in cols)
        {
            if (col.CompareTag("Enemy"))
            {
                if (col.TryGetComponent(out MonsterController monster))
                {
                    monster.Distraction(transform, attractTime);
                }
            }
        }
    }
}
