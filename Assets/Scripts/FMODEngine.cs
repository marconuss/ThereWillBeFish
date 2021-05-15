using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODEngine : MonoBehaviour
{
    private FMOD.Studio.EventInstance eventInstance;

    [FMODUnity.EventRef]
    public string fmodEvent;

    //[SerializeField] [Range(-5f, 5f)]
    //private float pitchToHeight = 0.0f;
    //
    //[SerializeField] [Range(0.5f, 2f)]
    //private float pitchToVelocity = 1.0f;


    private void Start()
    {
        eventInstance = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);
        eventInstance.start();

    }

    private void Update()
    {

        //eventInstance.setParameterByName("PitchToHeight", pitchToHeight);
        //eventInstance.setParameterByName("PitchToVelocity", pitchToVelocity);

        eventInstance.setParameterByName("PitchToHeight", Submarine.Instance.transform.position.y);
        eventInstance.setParameterByName("PitchToVelocity", Submarine.Instance.velocity.x);
    }
}
