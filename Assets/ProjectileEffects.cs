using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProjectileEffects : MonoBehaviour
{
    [TagSelector]
    public string[] hitTag = new string[] { };

    public string tagHit;
    public UnityEvent applyHitEvent;
    public bool hitOnce;

    void OnTriggerEnter(Collider collider)
    {
        //Debug.Log("KKK");

        foreach (string tList in hitTag)
        {
            if (collider.gameObject.tag == tList)
            {
                //Do Something
            }
        }


        if (collider.gameObject.tag == tagHit)
        {
            if (!hitOnce)
            {
                Debug.Log("KKK");
                applyHitEvent.Invoke();
                hitOnce = true;
            }
        }
    }

    public void DestroyRigid()
    {
        Destroy(this.gameObject.transform.GetComponent<Rigidbody>());
    }
    public void DestroyTRenderer()
    {
        Destroy(this.gameObject.transform.GetComponent<TrailRenderer>());
    }
}
