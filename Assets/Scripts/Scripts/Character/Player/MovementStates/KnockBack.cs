using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    CharacterController CC;
    Vector3 knockback;

    void Awake()
    {
        CC = GetComponent<CharacterController>();
    }

    public void Kfunction()
    {
        knockback = Vector3.right / 10;//use tranform.forward of the enemy
        CC.Move(knockback);
        knockback *= 0.95f;
    }

}
