using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    [SerializeField]
    private float minSpawnHeight = -4f;
    [SerializeField]
    private float maxSpawnHeight = 4f;
    [SerializeField]
    private GameObject fishPrefab = null;

    private void OnEnable()
    {
        Metronome.Instance.OnBeat += SpawnFish;
    }

    private void OnDisable()
    { 
        if (Metronome.Instance)
            Metronome.Instance.OnBeat += SpawnFish;
    }

    private void SpawnFish()
    {
        if (!PauseManager.Instance.IsPaused)
        {
            float spawnHeight = Random.Range(minSpawnHeight, maxSpawnHeight);
            Vector3 spawnPos = new Vector3(transform.position.x, spawnHeight, 0f);
            Instantiate(fishPrefab, spawnPos, Quaternion.identity, transform);
        }
    }
}
