using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterChase : MonoBehaviour
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
            _agent.SetDestination(target.transform.position);
        }
    }

}
