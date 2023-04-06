using UnityEngine;

public class MonsterDetection : MonoBehaviour
{

    [SerializeField] DetectionZone detectionZone;
    GameObject target;

    [Header("Detection Range")]
    public float detectionRadiusV = 10.0f; // V = view
    public float detectionRadiusS = 10.0f; // S = sound
    public float detectionAngle = 90.0f;
    [SerializeField] Color detectVColor = new Color(0.8f, 0f, 0f, 0.4f);
    [SerializeField] Color detectSColor = new Color(0f, 0f, 0f, 0.4f);
    [SerializeField] bool detectionGizmo;
    public LayerMask targetMask;

    [Header("<Player Range>")]
    [SerializeField] private Color plyRadiusColor = new Color(0f, 50f, 90f, 0.3f);
    [SerializeField] private float plyRadiusSize = 11f;
    [SerializeField] bool playerGizmo;

    [Header("<Attack Range>")]
    [SerializeField] private Color atkRadiusColor = new Color(255f, 255f, 255f, 0.15f);
    [SerializeField] private float atkRadiusSize = 5f;
    [SerializeField] bool attackGizmo;

    private void Update()
    {
        DetectForPlayer();
        //DetectTargets();
    }

    void DetectForPlayer()
    {
        if (detectionZone.detectedObjs.Count > 0)
        {
            // Calculate direction to target object
            target = detectionZone.detectedObjs[0].gameObject;
        }
        else
        {
            target = null;
        }

        if (target != null)
        {
            LookForPlayer();
        }
    }

    void LookForPlayer()
    {
        Vector3 enemyPosition = transform.position;
        Vector3 toPlayer = target.transform.position - enemyPosition;

        toPlayer.y = 0;

        if (toPlayer.magnitude <= detectionRadiusV) // Detect by Sound and Sight
        {
            if (Vector3.Dot(toPlayer.normalized, transform.forward) > Mathf.Cos(detectionAngle * 0.5f * Mathf.Deg2Rad))
            {
                if (tag == "Player")
                {
                    Debug.Log("Player has been detected! Sight");
                }
            }
        }

        if (toPlayer.magnitude <= detectionRadiusS) // Detect by Sound
        {
            if (Vector3.Dot(toPlayer.normalized, -transform.forward) > Mathf.Cos((360 - detectionAngle) * 0.5f * Mathf.Deg2Rad))
            {
                if (tag == "Player")
                {
                    Debug.Log("Player has been detected! Sound");
                }
            }
        }
    }

    void DetectTargets()
    {
        var targets = Physics.OverlapSphere(transform.position, detectionRadiusV, targetMask);
        foreach (var t in targets)
        {
            Vector3 dir = (t.transform.position - transform.position).normalized;//direction to the target
            float anglea = (detectionAngle / 2) / 180;//convert angle into a 0-1 range
            //Debug.Log((Vector3.Dot(-dir, transform.forward) + 1) / 2 > anglea);
            if ((Vector3.Dot(-dir, transform.forward)+1)/2 < anglea)
            {
                //add to list or whatever
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (playerGizmo)
        {
            // Attack Player Radius
            Color c = plyRadiusColor;
            UnityEditor.Handles.color = c;

            Vector3 fullPlyR = transform.forward;

            UnityEditor.Handles.DrawSolidArc(transform.position, Vector3.up, fullPlyR, 360, plyRadiusSize);
        }

        if (detectionGizmo)
        {
            // Detect Sound and Sight Radius
            Color a = detectVColor;
            UnityEditor.Handles.color = a;

            Vector3 rotatedForward = Quaternion.Euler(0, -detectionAngle * 0.5f, 0) * transform.forward;

            UnityEditor.Handles.DrawSolidArc(transform.position, Vector3.up, rotatedForward, detectionAngle, detectionRadiusV);

            // Detect Sound Radius
            Color b = detectSColor;
            UnityEditor.Handles.color = b;

            Vector3 rotatedBack = Quaternion.Euler(0, -(360 - detectionAngle) * 0.5f, 0) * -transform.forward;

            UnityEditor.Handles.DrawSolidArc(transform.position, Vector3.up, rotatedBack, 360 - detectionAngle, detectionRadiusS);
        }

        if (attackGizmo)
        {
            // Attack Player Radius
            Color d = atkRadiusColor;
            UnityEditor.Handles.color = d;

            Vector3 fullAtkR = transform.forward;

            UnityEditor.Handles.DrawSolidArc(transform.position, Vector3.up, fullAtkR, 360, atkRadiusSize);
        }


    }
#endif
}
