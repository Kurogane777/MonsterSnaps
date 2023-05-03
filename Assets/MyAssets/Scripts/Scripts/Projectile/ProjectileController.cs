using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProjectileController : MonoBehaviour
{
    public float projectileForce = 5;

    [Space]
    public GameObject bulletB;
    public GameObject bulletR;
    public Transform shotPoint;
    public bool B_R_Start;
    public bool B_R_Current;
    public bool BAllowShoot;
    public bool RAllowShoot;

    public UnityEvent invokeB;
    public UnityEvent invokeR;

    GameObject currentBullet;

    private void Awake()
    {
        if (!B_R_Start)
        {
            currentBullet = bulletB;
            B_R_Current = false;
        }
        else
        {
            currentBullet = bulletR;
            B_R_Current = true;
        }
    }

    private void Update()
    {
        if (!B_R_Current)
        {
            if (BAllowShoot)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0)) // Shoot bullet
                {
                    GameObject CreatedCannonball = Instantiate(currentBullet, shotPoint.position, shotPoint.rotation);
                    CreatedCannonball.GetComponent<Rigidbody>().velocity = shotPoint.transform.up * projectileForce;
                }
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                invokeB.Invoke();
            }
        }
        else
        {
            if (RAllowShoot)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0)) // Shoot bullet
                {
                    GameObject CreatedCannonball = Instantiate(currentBullet, shotPoint.position, shotPoint.rotation);
                    CreatedCannonball.GetComponent<Rigidbody>().velocity = shotPoint.transform.up * projectileForce;
                }
            }


            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                invokeR.Invoke();
            }
        }
    }

    public void bullB()
    {
        currentBullet = bulletB;
        B_R_Current = false;
    }

    public void bullBNotAllow()
    {
        BAllowShoot = false;
    }

    public void bullBAllow()
    {
        BAllowShoot = true;
    }

    public void bullR()
    {
        currentBullet = bulletR;
        B_R_Current = true;
    }

    public void bullRNotAllow()
    {
        RAllowShoot = false;
    }

    public void bullRAllow()
    {
        RAllowShoot = true;
    }
}
