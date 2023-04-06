using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    Transform target;
    Vector3 targetPos;
    bool attacking;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            if (attacking)
            {
                //attack at targetPos
                attacking = false;
            }
            else
            {
                targetPos = target.position;
                //move towards player or patrol/whatever
            }
        }
    }
}
