using UnityEngine;
using UnityEngine.AI;
using System.Collections;


public class MonsterPatrol : MonoBehaviour
{
    //public Transform[] points;
    PatrolList pList;
    private int destPoint = 0;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Disabling auto-braking allows for continuous movement
        // between points (ie, the agent doesn't slow down as it
        // approaches a destination point).
        agent.autoBraking = false;

        if (pList != null)
        {
            GotoNextPoint();
        }
    }


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

    void NearestPoint()
    {
        //GameObject[] pListArea = FindObjectOfType<PatrolList>().gameObject;
        GameObject[] pListArea = GameObject.FindGameObjectsWithTag("A");
        if (pListArea.Length > 0)
        {
            GameObject closestPArea = pListArea[0];
            foreach (GameObject plistA in pListArea)
            {
                if (Vector3.Distance(plistA.transform.position, transform.position) < Vector3.Distance(closestPArea.transform.position, transform.position))
                {
                    closestPArea = plistA;
                }
            }
            pList = closestPArea.gameObject.GetComponent<PatrolList>();
        }
    }


    void Update()
    {
        if (!enabled) { pList = null; }

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
}
