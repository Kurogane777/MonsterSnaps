using UnityEngine;

public class InputController : MonoBehaviour
{
    public KeyCode pause = KeyCode.Escape;

    public KeyCode dash = KeyCode.LeftShift;

    public KeyCode CSCameraPhoto = KeyCode.E;
    public KeyCode CSProjectile = KeyCode.R;

    public KeyCode openInvBook = KeyCode.Escape;
    public KeyCode prvInvBook = KeyCode.T;
    public KeyCode nxtInvBook = KeyCode.Y;

    public void lockMouse()
    { Cursor.lockState = CursorLockMode.Locked; }

    public void unlockMouse()
    { Cursor.lockState = CursorLockMode.None; }
}
