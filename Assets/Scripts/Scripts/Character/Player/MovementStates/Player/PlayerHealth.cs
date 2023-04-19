using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    public GameObject ui;
    public List<GameObject> heartsUI;
    public int numberHearts;
    public UnityEvent heartsZero;

    bool callOnce;

    public static PlayerHealth main;

    void Awake()
    {
        main = this;
        if (numberHearts > heartsUI.Count || numberHearts < heartsUI.Count)
        {
            numberHearts = heartsUI.Count;
        }
    }

    void Update()
    {
        numberHearts = Mathf.Clamp(numberHearts, 0, heartsUI.Count);

        if (!callOnce) { heartsZero.Invoke(); callOnce = true; }
        if (numberHearts != 0) { callOnce = false; }
    }

    public void Damage()
    {
        if (numberHearts > 0)
        {
            numberHearts--;
            heartsUI[numberHearts].gameObject.SetActive(false);
        }
    }

    public void Heal()
    {
        if (numberHearts < heartsUI.Count)
        {
            numberHearts++;
            heartsUI[numberHearts - 1].gameObject.SetActive(true);
        }
    }
}
