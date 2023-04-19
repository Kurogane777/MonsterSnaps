using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterZone : MonoBehaviour
{
    public string tagTarget = "Monster";

    public List<Collider> detectedObjs = new List<Collider>();
    public Collider col;

    [SerializeField] Color color = Color.red;
    [SerializeField] float radius = 15;

    // Detect when object enter range
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == tagTarget)
        { detectedObjs.Add(collider); }
    }

    // Detect when object leave range
    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == tagTarget)
        { detectedObjs.Remove(collider); }
    }

    void OnDrawGizmosSelected()
    {
        // Draw a red sphere at the transform's position
        Gizmos.color = color;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
