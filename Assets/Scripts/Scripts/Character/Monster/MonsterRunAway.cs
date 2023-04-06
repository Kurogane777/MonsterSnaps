using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterRunAway : MonoBehaviour
{
    public GameObject target;
    private NavMeshAgent _agent;

    //public float EnemyDistanceRun = 4.0f;

    void Start()
    {
        target = GameObject.Find("Player");
        _agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        Vector3 dirToPlayer = transform.position - target.transform.position;
        Vector3 newPos = transform.position + dirToPlayer;
        _agent.SetDestination(newPos);
    }

    //void Update()
    //{
    //    float distance = Vector3.Distance(transform.position, target.transform.position);

    //    Debug.Log("Distance: " + distance);

    //    // Run away from player
    //    if (distance < EnemyDistanceRun)
    //    {
    //        // Vector player to me
    //        Vector3 dirToPlayer = transform.position - target.transform.position;
    //        Vector3 newPos = transform.position + dirToPlayer;
    //        _agent.SetDestination(newPos);
    //    }
    //}
}
