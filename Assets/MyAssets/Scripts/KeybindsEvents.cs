using UnityEngine;
using UnityEngine.Events;

public class KeybindsEvents : MonoBehaviour
{
    public KeyCode keyPress;
    public UnityEvent activateEvents;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyPress))
        {
            activateEvents.Invoke();
        }
    }
}
