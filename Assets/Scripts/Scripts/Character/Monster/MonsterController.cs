using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    [Header("State Script")]
    [SerializeField] private MonsterPatrol mPatrol;
    [SerializeField] private MonsterRunAway mRun;
    [SerializeField] private MonsterChase mChase;
    [SerializeField] private DetectionZone dZone;

    [Header("Detect and Lost Player")]
    [SerializeField] private GameObject dPlyerParticles;
    [SerializeField] private GameObject lPlyerParticles;

    [Header("Detection Range")]
    public float detectionRadiusV = 10.0f; // V = view
    public float detectionRadiusS = 10.0f; // S = sound
    public float detectionAngle = 90.0f;
    [SerializeField] Color detectVColor = new Color(0.8f, 0f, 0f, 0.4f);
    [SerializeField] Color detectSColor = new Color(0f, 0f, 0f, 0.4f);
    [SerializeField] bool detectionGizmo;

    [Header("<Player Range>")]
    [SerializeField] private Color plyRadiusColor = new Color(255f, 255f, 255f, 0.2f);
    [SerializeField] private float plyRadiusSize = 8f;
    [SerializeField] bool playerGizmo;

    [Header("<Attack Range>")]
    [SerializeField] private Color atkRadiusColor = new Color(255f, 255f, 255f, 0.15f);
    [SerializeField] private float atkRadiusSize = 5f;
    [SerializeField] bool attackGizmo;

    private GameObject player;

    public enum State
    { patrol, run, chase, attack, hide, attract, Scare }

    public State currentState;

    private void Start()
    {
        currentState = State.patrol;
    }

    private void Update()
    {
        // Movement Control State
        switch (currentState)
        {
            case State.patrol:
                mPatrol.enabled = true;
                mRun.enabled = false;
                mChase.enabled = false;
                break;
            case State.run:
                mPatrol.enabled = false;
                mRun.enabled = true;
                mChase.enabled = false;
                break;
            case State.chase:
                mPatrol.enabled = false;
                mRun.enabled = false;
                mChase.enabled = true;
                break;
            case State.attack:
                mPatrol.enabled = false;
                mRun.enabled = false;
                mChase.enabled = false;
                break;
            case State.hide:
                break;
            case State.attract:
                break;
        }

        DetectRayCast(true);
        AttackRayCast();
    }

    bool Detect()
    {
        return true;
    }

    void LookForPlayer()
    {
        //Vector3 enemyPosition = transform.position;
        //Vector3 toPlayer = target.transform.position - enemyPosition;

        //toPlayer.y = 0;

        //if (toPlayer.magnitude <= detectionRadiusV) // Detect by Sound and Sight
        //{
        //    if (Vector3.Dot(toPlayer.normalized, transform.forward) > Mathf.Cos(detectionAngle * 0.5f * Mathf.Deg2Rad))
        //    {
        //        if (tag == "Player")
        //        {
        //            Debug.Log("Player has been detected! Sight");
        //        }
        //    }
        //}

        //if (toPlayer.magnitude <= detectionRadiusS) // Detect by Sound
        //{
        //    if (Vector3.Dot(toPlayer.normalized, -transform.forward) > Mathf.Cos((360 - detectionAngle) * 0.5f * Mathf.Deg2Rad))
        //    {
        //        if (tag == "Player")
        //        {
        //            Debug.Log("Player has been detected! Sound");
        //        }
        //    }
        //}
    }

    void DetectRayCast(bool variable)
    {
        if (variable)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, plyRadiusSize);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.tag == "Player")
                {
                    currentState = State.chase;
                }
                else
                {
                    currentState = State.patrol;
                }
            }
        }
    }

    void AttackRayCast()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, Mathf.Clamp(atkRadiusSize, 0, plyRadiusSize));
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.tag == "Player")
            {
                DetectRayCast(false);

                currentState = State.attack;
            }
        }
    }

    #region Ignore
    //void OnDrawGizmosSelected()
    //{
    //    // Draw a red sphere at the transform's position
    //    Gizmos.color = plySphereColor;
    //    Gizmos.DrawWireSphere(transform.position, plySphereSize);

    //    // Draw a blue sphere at the transform's position
    //    Gizmos.color = atkSphereColor;
    //    Gizmos.DrawWireSphere(transform.position, Mathf.Clamp(atkSphereSize, 0, plySphereSize));
    //}
    #endregion

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
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

        if (playerGizmo)
        {
            // Attack Player Radius
            Color c = plyRadiusColor;
            UnityEditor.Handles.color = c;

            Vector3 fullPlyR = transform.forward;

            UnityEditor.Handles.DrawSolidArc(transform.position, Vector3.up, fullPlyR, 360, plyRadiusSize);
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
