using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class MonsterIndex
{
    public string monsterName;
    public bool capture;
}

public class MonsterDex : MonoBehaviour
{
    public Sprite captureIcon;
    public Sprite unknownIcon;

    public List<MonsterIndex> monsterDex;
    public bool allCapture;
    public UnityEvent allCaptureEvent;
    public static MonsterDex main;

    public void Awake()
    {
        main = this;
    }
    private void Update()
    {
        foreach (var index in monsterDex) { if (index.capture == true) { allCapture = true; } else { allCapture = false; } }
        if (allCapture == true) { allCaptureEvent.Invoke(); }
    }
    public void Captured(List<CaptureTarget> targets)
    {
        foreach (var item in targets)
        {
            if (item.unvisibility > 0.75f)
                continue;//skip if the target is more than 25% blocked
            if (TryGetIndex(item.name, out int index))
            {
                monsterDex[index].capture = true;
            }            
        }
    }
    public bool TryGetIndex(string inp, out int ind)
    {
        ind = 0;
        try
        {
            ind = monsterDex.FindIndex(n => n.monsterName == inp);
        }
        catch (System.Exception)
        {
            return false;
        }
        return true;
    }
}
