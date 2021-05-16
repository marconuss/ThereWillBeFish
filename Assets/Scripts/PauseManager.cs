using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : Singleton<PauseManager>
{
    private bool isPaused = false;
    public bool IsPaused
    {
        get => isPaused;
        set
        {
            if ((value != isPaused) && (OnPause != null))
            {
                isPaused = value;
                OnPause(value);
            }
        }
    }
    public delegate void PauseAction(bool isPaused);
    public PauseAction OnPause;
}
