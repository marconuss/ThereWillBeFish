using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metronome : Singleton<Metronome>
{
    [SerializeField]
    private float defaultBPM = 140f;

    public delegate void BeatAction();
    public BeatAction OnBeat;

    private float bpm = 0f;

    private void Start()
    {
        StartCoroutine(Beat());
    }

    private IEnumerator Beat()
    {
        yield return new WaitForSeconds(1f);
        while(true)
        {
            if (OnBeat != null)
            {
                OnBeat();
            }
            bpm = defaultBPM * Submarine.Instance.Velocity.x;
            yield return new WaitForSeconds(60f / bpm);
        }
    }
}
