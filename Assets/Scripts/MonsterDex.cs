using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[System.Serializable]
public class MonsterIndex
{
    [HideInInspector] public GameObject uiBoxDex;
    public string mName;
    [TextArea]
    public string mDesc;
    public Sprite mSprite;
    public bool capture;
}

public class MonsterDex : MonoBehaviour
{
    public Sprite captureIcon;
    public Sprite unknownIcon;
    public Image monsterUIImage;
    public List<Text> monsterName;
    public List<Text> monsterDesc;
    public GameObject mDex;
    public GameObject uiPrefabBox;

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
        UIDex();
        UISelect();

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
            ind = monsterDex.FindIndex(n => n.mName == inp);
        }
        catch (System.Exception)
        {
            return false;
        }
        return true;
    }

    int currentInt = 0;

    void UIDex()
    {
        for (int i = 0; i < monsterDex.Count; i++)
        {
            if (currentInt < monsterDex.Count)
            {
                monsterDex[i].uiBoxDex = Instantiate(uiPrefabBox, transform.position, Quaternion.identity, mDex.transform);
                currentInt++;
            }

            monsterDex[i].uiBoxDex.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = i.ToString("000");
            monsterDex[i].uiBoxDex.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = monsterDex[i].mName;

            if (monsterDex[i].capture == true)
            {
                monsterDex[i].uiBoxDex.transform.GetChild(0).GetChild(2).GetComponent<Image>().sprite = captureIcon;
            }
            else
            {
                monsterDex[i].uiBoxDex.transform.GetChild(0).GetChild(2).GetComponent<Image>().sprite = unknownIcon;
            }
        }
    }

    GameObject MSelect;
    int currentSlelectedInt;

    void UISelect()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && currentSlelectedInt > 0)
        {
            currentSlelectedInt--;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && currentSlelectedInt < monsterDex.Count - 1)
        {
            currentSlelectedInt++;
        }

        MSelect = monsterDex[currentSlelectedInt].uiBoxDex;

        MSelect.transform.GetChild(0).GetChild(0).GetComponent<Text>().color = Color.red;
        MSelect.transform.GetChild(0).GetChild(1).GetComponent<Text>().color = Color.red;
        monsterUIImage.sprite = monsterDex[currentSlelectedInt].mSprite;

        foreach (var mListName in monsterName)
        {
            mListName.text = monsterDex[currentSlelectedInt].mName;
        }

        foreach (var mListDesc in monsterDesc)
        {
            mListDesc.text = monsterDex[currentSlelectedInt].mDesc;
        }

        foreach (var ui in monsterDex)
        {
            if (ui.uiBoxDex != MSelect)
            {
                ui.uiBoxDex.transform.GetChild(0).GetChild(0).GetComponent<Text>().color =
                uiPrefabBox.transform.GetChild(0).GetChild(0).GetComponent<Text>().color;

                ui.uiBoxDex.transform.GetChild(0).GetChild(1).GetComponent<Text>().color = 
                uiPrefabBox.transform.GetChild(0).GetChild(1).GetComponent<Text>().color;
            }
        }
    }
}
