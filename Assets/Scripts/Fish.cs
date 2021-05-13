using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Distance units covered per second when music is at normal speed (1).")]
    private float speed = 1f;

    private void Update()
    {
        float moveSpeed = speed * Submarine.Instance.Velocity.x;
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        // Play note
    }
}
