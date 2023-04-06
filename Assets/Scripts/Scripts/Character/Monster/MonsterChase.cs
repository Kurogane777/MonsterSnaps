using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterChase : MonoBehaviour
{
    public GameObject target;
    private NavMeshAgent _agent;

    void Start()
    {
        target = GameObject.Find("Player");
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        _agent.destination = target.transform.position;
    }

}
