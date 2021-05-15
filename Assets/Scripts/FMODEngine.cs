using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODEngine : MonoBehaviour
{
    [SerializeField]
    private FMODUnity.StudioEventEmitter emitter;

    private void Start()
    {
        emitter = GetComponent<FMODUnity.StudioEventEmitter>();
    }
    private void FixedUpdate()
    {
        emitter.SetParameter("PitchToHeight", Submarine.Instance.transform.position.y);
        emitter.SetParameter("PitchToVelocity", Submarine.Instance.velocity.x);
    }
}
