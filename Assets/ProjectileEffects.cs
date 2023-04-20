using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProjectileEffects : MonoBehaviour
{
    public string tagHit;
    public UnityEvent applyHitEvent;
    public bool hitOnce;

    void OnTriggerEnter(Collider collider)
    {
        //Debug.Log("KKK");

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
