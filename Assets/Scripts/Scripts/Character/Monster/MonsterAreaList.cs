using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MonsterAreaList : MonoBehaviour
{
    [SerializeField] List<MonsterEncounterRecord> wildMonsters;

    [HideInInspector][SerializeField] int totalChance = 0;

    private void OnValidate()
    {
        totalChance = 0;
        foreach (var record in wildMonsters)
        {
            record.chanceLower = totalChance;
            record.chanceUpper = totalChance + record.chancePercentage;

            totalChance = totalChance + record.chancePercentage;
        }
    }

    public void GetRandomWildMonster()
    {
        int randVal = Random.Range(1, 101);
        var monsterRecord = wildMonsters.First(m => randVal >= m.chanceLower && randVal <= m.chanceUpper);

        var wildMonster = monsterRecord.monster;
    }

    [System.Serializable]
    public class MonsterEncounterRecord
    {
        public GameObject monster;
        public int chancePercentage;

        public int chanceLower { get; set; }
        public int chanceUpper { get; set; }
    }

}
