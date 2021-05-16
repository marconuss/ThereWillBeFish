using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODEngine : MonoBehaviour
{
    private FMOD.Studio.EventInstance eventInstance;
    [FMODUnity.EventRef]
    public string fmodEvent;
    [SerializeField]
    private int clipLengthInBeats = 8;

    //[SerializeField] [Range(-5f, 5f)]
    //private float pitchToHeight = 0.0f;
    //
    //[SerializeField] [Range(0.5f, 2f)]
    //private float pitchToVelocity = 1.0f;
    
    private int beatsSinceLastPlay = 69420;

    private void OnEnable()
    {
        Metronome.Instance.OnBeat += PlayEngineBeat;
    }

    private void OnDisable()
    {
        if (Metronome.Instance)
            Metronome.Instance.OnBeat -= PlayEngineBeat;
    }

    private void PlayEngineBeat()
    {
        beatsSinceLastPlay++;
        if (beatsSinceLastPlay >= clipLengthInBeats)
        {
            eventInstance = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);
            eventInstance.start();
            beatsSinceLastPlay = 0;
        }
    }

/*
    private void Update()
    {
        eventInstance.setParameterByName("PitchToHeight", Submarine.Instance.transform.position.y);
        eventInstance.setParameterByName("PitchToVelocity", Submarine.Instance.velocity.x);
    }
*/
}
