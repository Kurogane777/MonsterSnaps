using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    public List<GameObject> heartsUI;
    public int numberHearts;

    public UnityEvent heartsDamage;
    public UnityEvent heartsHeal;

    public UnityEvent heartsZero;

    public bool callOnceDeath;

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

        if (callOnceDeath) { heartsZero.Invoke(); callOnceDeath = false; }
        if (numberHearts <= 0) { callOnceDeath = true; }
    }

    public void Damage()
    {
        if (numberHearts > 0)
        {
            heartsDamage.Invoke();

            numberHearts--;
            heartsUI[numberHearts].gameObject.SetActive(false);
        }
    }

    public void Heal()
    {
        if (numberHearts < heartsUI.Count)
        {
            heartsHeal.Invoke();

            numberHearts++;
            heartsUI[numberHearts - 1].gameObject.SetActive(true);
        }
    }
}
