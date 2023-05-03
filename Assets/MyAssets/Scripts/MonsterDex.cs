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

    public GameObject backUI;

    public bool OnOff;

    public void Awake()
    {
        main = this;
    }
    private void Update()
    {
        if (OnOff) { InputPressMonsterDex(); }
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
        if (!backUIBool)
        {
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
        else
        {
            foreach (var ui in monsterDex)
            {
                ui.uiBoxDex.transform.GetChild(0).GetChild(0).GetComponent<Text>().color =
                uiPrefabBox.transform.GetChild(0).GetChild(0).GetComponent<Text>().color;

                ui.uiBoxDex.transform.GetChild(0).GetChild(1).GetComponent<Text>().color =
                uiPrefabBox.transform.GetChild(0).GetChild(1).GetComponent<Text>().color;
            }
        }
    }

    int extracurrentInt;
    bool backUIBool;
    public UnityEvent returnBack;

    public void InputPressMonsterDex()
    {
        if (extracurrentInt <= currentSlelectedInt)
        {
            extracurrentInt = currentSlelectedInt;
        }

        if (!backUIBool)
        {

            backUI.transform.GetChild(1).GetComponent<Text>().color =
            uiPrefabBox.transform.GetChild(0).GetChild(1).GetComponent<Text>().color;

            if (Input.GetKeyDown(KeyCode.UpArrow) && currentSlelectedInt > 0)
            {
                currentSlelectedInt--;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) && currentSlelectedInt < monsterDex.Count - 1)
            {
                currentSlelectedInt++;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) && extracurrentInt == monsterDex.Count - 1)
            {
                backUI.transform.GetChild(1).GetComponent<Text>().color = Color.red;
                extracurrentInt++;
                backUIBool = true;
            }
        }
        else
        {
            if (currentSlelectedInt < monsterDex.Count - 1)
            {
                backUIBool = false;
            }
            if (Input.GetKeyDown(KeyCode.UpArrow) && currentSlelectedInt == monsterDex.Count - 1)
            {
                extracurrentInt--;
                backUIBool = false;
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                ResetCurrentDex();
                TurnOffDex();

                returnBack.Invoke();
            }
        }
    }

    void ResetCurrentDex()
    {
        currentSlelectedInt = 0;
        extracurrentInt = 0;
    }

    void TurnOffDex()
    {
        OnOff = false;
    }

    public void TurnOnDex()
    {
        OnOff = true;
    }
}
