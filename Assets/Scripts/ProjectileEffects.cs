using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProjectileEffects : MonoBehaviour
{
    [TagSelector]
    public string[] hitTag = new string[] { };

    //public string tagHit;
    public UnityEvent applyHitEvent;
    public bool hitOnce;
    public GameObject instance;

    void OnTriggerEnter(Collider collider)
    {
        foreach (string tList in hitTag)
        {
            if (collider.gameObject.tag == tList)
            {
                //Do Something
                if (!hitOnce)
                {
                    Instantiate(instance, transform.position, Quaternion.identity);
                    applyHitEvent.Invoke();
                    hitOnce = true;
                    Destroy(this.gameObject);
                }
            }
        }
    }
}
