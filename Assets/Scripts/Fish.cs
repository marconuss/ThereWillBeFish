using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{

    public GameObject bubbles;

    [SerializeField]
    [Tooltip("Distance units covered per second when music is at normal speed (1).")]
    private float speed = 1f;
    [SerializeField]
    private float destroyXCoord = -20f;
    [SerializeField]
    [FMODUnity.EventRef]
    private string hitFishEvent;

    private FMOD.Studio.EventInstance hitFishEventInstance;
    private bool wasHit = false;

    private void OnEnable()
    {
        Metronome.Instance.OnBeat += OnBeat;
    }

    private void OnDisable()
    {
        if (Metronome.Instance)
            Metronome.Instance.OnBeat -= OnBeat;
    }

    private void Update()
    {
        if (!PauseManager.Instance.IsPaused)
        {
            float moveSpeed = speed * Submarine.Instance.Velocity.x;
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;
            if (transform.position.x < destroyXCoord)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnBeat()
    {
        if (wasHit)
        {
            hitFishEventInstance = FMODUnity.RuntimeManager.CreateInstance(hitFishEvent);
            hitFishEventInstance.start();
            hitFishEventInstance.setParameterByName("FishHitHeight", transform.position.y);
            if(Submarine.Instance.enableVerticalPitch)
                hitFishEventInstance.setParameterByName("PitchToHeight", Submarine.Instance.transform.position.y);
            else
                hitFishEventInstance.setParameterByName("PitchToHeight", 0f);
            hitFishEventInstance.release();
            Instantiate(bubbles, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        wasHit = true;
    }
}
