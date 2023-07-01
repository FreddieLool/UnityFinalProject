using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleScript : MonoBehaviour
{
    [SerializeField] AudioSource AudioSource;
    public bool isToggled = false;



    public void TurnMusicOffOrOn()
    {
        if (isToggled)
        {
            AudioSource.mute = !AudioSource.mute;
        }
        else
        {
            AudioSource.mute = AudioSource.mute;
        }
    }
}
