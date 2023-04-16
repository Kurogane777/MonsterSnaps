using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterRunAway : MonoBehaviour
{
    MonsterController monC;
    private GameObject target;
    private NavMeshAgent _agent;

    void Start()
    {
        monC = GetComponent<MonsterController>();
        _agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        target = monC.target;

        if (target != null)
        {
            Vector3 dirToPlayer = transform.position - target.transform.position;
            Vector3 newPos = transform.position + dirToPlayer;
            _agent.SetDestination(newPos);
        }
    }
}
