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
        if (Input.GetKeyDown(inputC.pause))
        {
            _isPaused = !_isPaused;

            if (_isPaused)
            {
                Time.timeScale = 0;
                GamePaused.Invoke();
            }
            else
            {
                Time.timeScale = 1;
                GameResumed.Invoke();
            }
        }
    }

    public void ResumeR()
    {
        if (_isPaused)
        { Time.timeScale = 1; _isPaused = false; }
    }
}
