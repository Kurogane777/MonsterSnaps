using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuInvokeSelected : MonoBehaviour
{

    public UnityEvent _selctedAcess;

    public void AcessInvoke()
    {
        _selctedAcess.Invoke();
    }
}
