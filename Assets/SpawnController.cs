using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public static SpawnController main;
    public int monsterCap = 10;
    // Start is called before the first frame update
    void Start()
    {
        main = this;
    }
    public void SpawnMonster(GameObject toSpawn, Vector3 pos)
    {
        if (PlayerController.main.GetSurroundingMonsters() < monsterCap)
        {
            pos.y = 0;
            Instantiate(toSpawn, pos, Quaternion.identity);
        }
        else
        {
            Debug.Log("Spawn Cap Reached!");
        }
    }
}
