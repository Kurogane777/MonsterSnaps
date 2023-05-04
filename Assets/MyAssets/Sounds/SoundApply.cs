using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundApply : MonoBehaviour
{
    public AudioSource playSound;

    public void PlaySound()
    {
        playSound.Play();
    }
}
