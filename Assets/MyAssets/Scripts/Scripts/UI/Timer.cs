using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    public float targetTime = 60.0f;
    public Text timerText;
    public UnityEvent timerEnd;

    public bool setZero;

    public float keepData;

    private void Awake()
    {
        keepData = targetTime;

        if (setZero)
        {
            targetTime = 0;
        }
    }

    void Update()
    {
        if (timerText != null)
        {
            timerText.text = Mathf.FloorToInt(targetTime).ToString("00");
        }

        if (targetTime > 0.0f)
        {
            targetTime -= Time.deltaTime;
        }

        if (targetTime <= 0.0f)
        {
            timerEnded();
        }
    }

    void timerEnded()
    {
        timerEnd.Invoke();
    }

    public void timerRestart()
    {
        if (targetTime <= 0.0f)
        {
            targetTime = keepData;
        }
    }

    public void ObjDestroy()
    {
        Destroy(this.gameObject);
    }
}
