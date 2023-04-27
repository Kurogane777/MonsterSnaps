using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PauseController : MonoBehaviour
{
    [SerializeField] InputController inputC;
    public UnityEvent GamePaused;
    public UnityEvent GameResumed;
    private bool _isPaused;

    void Update()
    {
        Toggle();
    }

    public void Toggle()
    {
        if (Input.GetKeyDown(inputC.pause))
        {
            //_isPaused = !_isPaused;

            //if (_isPaused)
            //{
            //    Time.timeScale = 0;
            //    GamePaused.Invoke();
            //}
            //else
            //{
            //    Time.timeScale = 1;
            //    GameResumed.Invoke();
            //}

            PauseR();
        }
    }

    public void PauseR()
    {
        Time.timeScale = 0; _isPaused = false; GamePaused.Invoke();
    }

    public void ResumeR()
    {
        Time.timeScale = 1; _isPaused = true; GameResumed.Invoke();
    }
}
