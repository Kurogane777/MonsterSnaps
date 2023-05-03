using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputPhotoBookUI : MonoBehaviour
{
    public UnityEvent LeftArrow;
    public UnityEvent RightArrow;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            LeftArrow.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            RightArrow.Invoke();
        }
    }
}
