using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour
{
    [Header("State Script")]
    [SerializeField] private MonsterPatrol mPatrol;
    [SerializeField] private MonsterRunAway mRun;
    [SerializeField] private MonsterChase mChase;

    [SerializeField] DetectionZone detectionZone;
    [HideInInspector] public GameObject target;

    [Header("Detect and Lost Player")]
    [SerializeField] private GameObject expressionSlot;
    [SerializeField] private GameObject dPlyerParticles;
    [SerializeField] private GameObject lPlyerParticles;
    GameObject expressionObj;
    bool exclaimationMark;

    [Header("Detection Range")]
    public float detectionRadiusV = 10.0f; // V = view
    public float detectionRadiusS = 10.0f; // S = sound
    public float detectionAngle = 90.0f;
    [SerializeField] Color detectVColor = new Color(0.8f, 0f, 0f, 0.4f);
    [SerializeField] Color detectSColor = new Color(0f, 0f, 0f, 0.4f);
    [SerializeField] bool detectionGizmo;


    [Header("<Player Range>")]
    [SerializeField] private Color plyRadiusColor = new Color(0f, 50f, 90f, 0.3f);
    [SerializeField] private float plyRadiusSize = 11f;
    [SerializeField] bool playerGizmo;

    [Header("<Attack Range>")]
    [SerializeField] private Color atkRadiusColor = new Color(255f, 255f, 255f, 0.15f);
    [SerializeField] private float atkRadiusSize = 5f;
    [SerializeField] bool attackGizmo;

    [SerializeField] bool LOL;
    public NavMeshAgent agent;
    bool attacking;
    public float attackDelay = 1;
    float attackTimer, stunTimer;
    public float stunDuration = 0.5f;
    float nmSpeed,nmAcc;
    public float knockback = 0.2f;
    public enum State
    { patrol, run, chase, attack, hide, attract, Scare }

    public State currentState;

    private void Start()
    {
        currentState = State.patrol;
        agent = GetComponent<NavMeshAgent>();
        nmSpeed = agent.speed;
        nmAcc = agent.acceleration;
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
                mPatrol.enabled = true;
                mRun.enabled = false;
                mChase.enabled = false;
                agent.speed = nmSpeed;
                agent.acceleration = nmAcc;
                break;
            case State.run:
                mPatrol.enabled = false;
                mRun.enabled = true;
                mChase.enabled = false;
                agent.speed = nmSpeed;
                agent.acceleration = nmAcc;
                break;
            case State.chase:
                mPatrol.enabled = false;
                mRun.enabled = false;
                mChase.enabled = true;
                attacking = false;
                agent.speed = nmSpeed;
                agent.acceleration = nmAcc;
                break;
            case State.attack:
                mPatrol.enabled = false;
                mRun.enabled = false;
                mChase.enabled = false;
                if (stunTimer >=0)
                {
                    stunTimer -= Time.deltaTime;
                    break;
                }
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
                        PlayerController.main.ApplyKnockback((target.transform.position-transform.position).normalized*knockback);
                        agent.speed = 0;
                        stunTimer = stunDuration;
                        attacking = false;
                    }
                    else if (Vector3.Distance(transform.position, agent.path.corners[agent.path.corners.Length-1]) <= 0.1f || !agent.hasPath)
                    {
                        agent.speed = 0;
                        stunTimer = stunDuration;
                        attacking = false;
                    }
                }
                break;
            case State.hide:
                break;
            case State.attract:
                mPatrol.enabled = false;
                mRun.enabled = false;
                mChase.enabled = true;
                agent.speed = nmSpeed;
                agent.acceleration = nmAcc;
                break;
        }
    }
    bool CheckRange()
    {
        if (target)
        {
            agent.SetDestination(target.transform.position);
            if (agent.hasPath)
            {
                //draw debug
                for (int i = 0; i < agent.path.corners.Length-1; i++)
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
    }

    void DetectOnProjectile()
    { 
        
    }

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
