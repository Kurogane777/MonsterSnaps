using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour
{
    [Header("Detect and Lost Player")]
    [SerializeField] DetectionZone plyDetectionZone;
    [SerializeField] DetectionZone prjDetectionZone;
    [HideInInspector] public GameObject target;
    [Space]
    [SerializeField] private GameObject expressionSlot;
    [SerializeField] private GameObject dPlyerParticles;
    [SerializeField] private GameObject lPlyerParticles;
    GameObject expressionObj;
    bool exclaimationMark;
    bool questionMark;

    [Header("Attack")]
    public NavMeshAgent agent;
    bool attacking;
    public float attackDelay = 1;
    float attackTimer, stunTimer;
    public float stunDuration = 0.5f;
    float nmSpeed,nmAcc;
    public float knockback = 0.2f;

    [Header("Detection Range")]
    public float detectionRadiusV = 10.0f; // V = view
    public float detectionRadiusS = 10.0f; // S = sound
    public float detectionAngle = 90.0f;
    [Range(0, 100)] public float detectionRangeHiddenV;
    [Range(0, 100)] public float detectionRangeHiddenS;
    [SerializeField] Color detectVColor = new Color(0.8f, 0f, 0f, 0.4f);
    [SerializeField] Color detectSColor = new Color(0f, 0f, 0f, 0.4f);
    [SerializeField] bool detectionGizmo;
    [SerializeField] bool hiddenRangeGizmo;

    [Header("<Player Range>")]
    [SerializeField] private Color plyRadiusColor = new Color(0f, 50f, 90f, 0.3f);
    [SerializeField] private float plyRadiusSize = 11f;
    [SerializeField] bool playerGizmo;

    [Header("<Attack Range>")]
    [SerializeField] private Color atkRadiusColor = new Color(255f, 255f, 255f, 0.15f);
    [SerializeField] private float atkRadiusSize = 5f;
    [SerializeField] bool attackGizmo;

    public enum State
    { patrol, run, chase, attack, hide, attract, Scare }

    public State currentState;

    private void Start()
    {
        currentState = State.patrol;
        agent = GetComponent<NavMeshAgent>();
        nmSpeed = agent.speed;
        nmAcc = agent.acceleration;

        #region Patrol
        // Disabling auto-braking allows for continuous movement
        // between points (ie, the agent doesn't slow down as it
        // approaches a destination point).
        agent.autoBraking = false;

        if (pList != null)
        {
            GotoNextPoint();
        }
        #endregion
    }

    private void Update()
    {
        DetectForPlayer();
        ProjectileDetectRange();

        // Do the action after getting the Target
        if (target != null) { Action(); }

        // Movement Control State
        switch (currentState)
        {
            case State.patrol:
                PatrolFunction();
                break;
            case State.run:
                RunFunction();
                break;
            case State.chase:
                ChaseFunction();
                break;
            case State.attack:
                #region AttackPlayerReload
                if (stunTimer >=0)
                {
                    stunTimer -= Time.deltaTime;
                    break;
                }
                AttackPlayer();
                #endregion
                break;
            case State.hide:
                break;
            case State.attract:
                AttractFunction();
                break;
        }

        if (currentState != State.patrol) { pList = null; pListArea = null; }
    }

    #region StateChange
    void PatrolFunction()
    {
        Patrol();
        agent.speed = nmSpeed;
        agent.acceleration = nmAcc;
    }
    void RunFunction()
    {
        RunAway();
        agent.speed = nmSpeed;
        agent.acceleration = nmAcc;
    }
    void ChaseFunction()
    {
        Chase();
        attacking = false;
        agent.speed = nmSpeed;
        agent.acceleration = nmAcc;
    }
    void AttractFunction()
    {
        Chase();
        agent.speed = nmSpeed;
        agent.acceleration = nmAcc;
    }
    #endregion

    void Action()
    {
        if (LookForPlayerSoundSight() == true && AttackPlayerRange() == false) { currentState = State.chase; } 
        if (LookForPlayerSoundSight() == true) // ExclaimationMark
        {
            if (exclaimationMark == false)
            {
                expressionObj = Instantiate(dPlyerParticles, expressionSlot.transform.position, Quaternion.identity);
                expressionObj.transform.SetParent(expressionSlot.transform);
                exclaimationMark = true;
            }
        } 
        if (expressionObj == null) { expressionObj = null; } 
        if (AttackPlayerRange() == true) { currentState = State.attack; } 
        if (OutofRangePlayer() == false) { currentState = State.patrol; exclaimationMark = false; }
    }

    #region Detect&Attack
    void DetectForPlayer()
    {
        if (plyDetectionZone.detectedObjs.Count > 0)
        {
            // Calculate direction to target object
            target = plyDetectionZone.detectedObjs[0].gameObject;
        }
        else
        {
            target = null;
        }
    }

    void DetectOnProjectile()
    {
        if (prjDetectionZone.detectedObjs.Count > 0)
        {
            // Calculate direction to target object
            target = prjDetectionZone.detectedObjs[0].gameObject;
        }
        else
        {
            target = null;
        }
    }

    void AttackPlayer()
    {
        if (!attacking)
        {
            agent.speed = nmSpeed;
            agent.acceleration = nmAcc;
            if (CheckRange())
            {
                if (attackTimer <= 0)
                {
                    attacking = true;
                    attackTimer = attackDelay;
                    agent.speed = nmSpeed * 5;
                    agent.acceleration = 100;
                }
                else
                {
                    attackTimer -= Time.deltaTime;
                }
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, target.transform.position) <= 1.1f)
            {
                PlayerHealth.main.Damage();
                PlayerController.main.ApplyKnockback((target.transform.position - transform.position).normalized * knockback);
                agent.speed = 0;
                stunTimer = stunDuration;
                attacking = false;
            }
            else if (Vector3.Distance(transform.position, agent.path.corners[agent.path.corners.Length - 1]) <= 0.1f || !agent.hasPath)
            {
                agent.speed = 0;
                stunTimer = stunDuration;
                attacking = false;
            }
        }
    }
    #endregion

    #region CheckRangeBool
    bool CheckRange()
    {
        if (target)
        {
            agent.SetDestination(target.transform.position);
            if (agent.hasPath)
            {
                //draw debug
                for (int i = 0; i < agent.path.corners.Length - 1; i++)
                {
                    Debug.DrawLine(agent.path.corners[i], agent.path.corners[i + 1]);
                }
                if (agent.path.corners.Length == 2)
                {
                    return true;
                }
            }
        }
        return false;
    }

    bool LookForPlayerSoundSight()
    {
        Vector3 enemyPosition = transform.position;
        Vector3 toPlayer = target.transform.position - enemyPosition;

        toPlayer.y = 0;

        if (toPlayer.magnitude <= detectionRadiusV) // Detect by Sound and Sight
        {
            if (Vector3.Dot(toPlayer.normalized, transform.forward) > Mathf.Cos(detectionAngle * 0.5f * Mathf.Deg2Rad))
            {
                if (target)
                {
                    //Debug.Log("Player has been detected! Sight");
                    return true;
                }
            }
        }

        if (toPlayer.magnitude <= detectionRadiusS) // Detect by Sound
        {
            if (Vector3.Dot(toPlayer.normalized, -transform.forward) > Mathf.Cos((360 - detectionAngle) * 0.5f * Mathf.Deg2Rad))
            {
                if (target)
                {
                    //Debug.Log("Player has been detected! Sound");
                    return true;
                }
            }
        }

        return false;
    }

    bool AttackPlayerRange()
    {
        Vector3 enemyPosition = transform.position;
        Vector3 toPlayer = target.transform.position - enemyPosition;

        toPlayer.y = 0;

        if (toPlayer.magnitude <= atkRadiusSize) // Attack Player Range
        {
            //Debug.Log("Attack Player!");
            return true;
        }

        return false;
    }

    bool OutofRangePlayer()
    {
        Vector3 enemyPosition = transform.position;
        Vector3 toPlayer = target.transform.position - enemyPosition;

        toPlayer.y = 0;

        if (toPlayer.magnitude <= plyRadiusSize) // Out of Player Range
        {
            //Debug.Log("Chase Player!");
            return true;
        }

        return false;
    }

    bool ProjectileDetectRange()
    {
        Vector3 enemyPosition = transform.position;
        if (target)
        {
            Vector3 toProjectile = target.transform.position - enemyPosition;

            toProjectile.y = 0;

            if (toProjectile.magnitude <= plyRadiusSize)
            {
                if (gameObject.TryGetComponent(out ProjectileBullet proj))
                {
                    Debug.Log("Detect Projectile!");
                    return true;
                }
            }
        }

        return false;
    }
    #endregion

    #region Patrol
    public PatrolList pList;
    private int destPoint = 0;

    void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (pList.points.Length == 0)
            return;

        // Set the agent to go to the currently selected destination.
        agent.destination = pList.points[destPoint].position;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = (destPoint + 1) % pList.points.Length;
    }

    public PatrolList[] pListArea;

    void NearestPoint()
    {
        pListArea = FindObjectsOfType<PatrolList>();

        if (pListArea.Length > 0)
        {
            PatrolList closestPArea = pListArea[0];
            foreach (PatrolList plistA in pListArea)
            {
                if (Vector3.Distance(plistA.transform.position, transform.position) < Vector3.Distance(closestPArea.transform.position, transform.position))
                {
                    closestPArea = plistA;
                }
            }
            pList = closestPArea.gameObject.GetComponent<PatrolList>();
        }
    }

    void Patrol()
    {
        NearestPoint();

        // Choose the next destination point when the agent gets
        // close to the current one.
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            if (pList != null)
            {
                GotoNextPoint();
            }
        }
    }
    #endregion

    #region RunAway
    void RunAway()
    {
        if (target != null)
        {
            Vector3 dirToPlayer = transform.position - target.transform.position;
            Vector3 newPos = transform.position + dirToPlayer;
            agent.SetDestination(newPos);
        }
    }
    #endregion

    #region Chase
    void Chase()
    {
        if (target != null)
        {
            agent.SetDestination(target.transform.position);
        }
    }
    #endregion

    #region Gizmos
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

        if (detectionGizmo)
        {
            if (hiddenRangeGizmo)
            {
                // Detect Sound and Sight Radius
                Color a = new Color(1f, 1f, 1f, 0.05f);
                UnityEditor.Handles.color = a;

                Vector3 rotatedForward = Quaternion.Euler(0, -detectionAngle * 0.5f, 0) * transform.forward;

                UnityEditor.Handles.DrawSolidArc(transform.position, Vector3.up, rotatedForward, detectionAngle, detectionRadiusV * (detectionRangeHiddenV / 100));

                // Detect Sound Radius
                Color b = new Color(1f, 1f, 1f, 0.05f);
                UnityEditor.Handles.color = b;

                Vector3 rotatedBack = Quaternion.Euler(0, -(360 - detectionAngle) * 0.5f, 0) * -transform.forward;

                UnityEditor.Handles.DrawSolidArc(transform.position, Vector3.up, rotatedBack, 360 - detectionAngle, detectionRadiusS * (detectionRangeHiddenS / 100));
            }
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
    #endregion
}
