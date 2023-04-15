using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnArea : MonoBehaviour
{
    public float spawnTime = 4;
    public float spawnRadius = 10;
    public float pRad = 10;//player spawn radius
    float timer;
    MonsterAreaList list;
    // Start is called before the first frame update
    void Start()
    {
        list = GetComponent<MonsterAreaList>();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer<=0)
        {
            timer = spawnTime;
            //spawn
            if (RandomPointWithinSpheres(PlayerController.main.transform.position, pRad, transform.position, spawnRadius, out Vector3 point))
                SpawnController.main.SpawnMonster(list.GetRandomWildMonster(), point);
        }
    }
    public bool RandomPointWithinSpheres(Vector3 posA, float radiusA, Vector3 posB, float radiusB, out Vector3 pos)
    {
        pos = Vector3.zero;
        float distance = Vector3.Distance(posA, posB);
        bool intersecting = distance < radiusA + radiusB;
        if (!intersecting) return false;//cancel if not intersecting
        float d = Mathf.Sqrt((distance + radiusA + radiusB) * (distance + radiusA - radiusB) * (distance - radiusA + radiusB) * (-distance + radiusA + radiusB)) / (2f * distance);
        Vector3 direction = (posB - posA).normalized;
        Vector3 intersectionCenter = posA + direction * (Mathf.Sqrt(radiusA * radiusA - d * d));
        float signedDistance = Vector3.Dot(direction, intersectionCenter - posA);
        float intersectionHeight = Mathf.Sqrt(radiusA * radiusA - signedDistance * signedDistance);
        Debug.DrawRay(intersectionCenter, Vector3.up / 10, intersecting ? Color.blue : Color.cyan);
        Debug.DrawRay(intersectionCenter, Vector3.Cross(direction, Vector3.up) * intersectionHeight, Color.magenta);
        for (int i = 0; i < 100; i++)//try a bunch of times
        {
            pos = intersectionCenter + (Random.insideUnitSphere * intersectionHeight);
            if (Vector3.Distance(posA, pos) < radiusA && Vector3.Distance(posB, pos) < radiusB)
            {
                return true;
            }
        }
        return false;
    }
}
