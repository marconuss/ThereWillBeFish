using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Submarine : Singleton<Submarine>
{
    [SerializeField]
    private bool enableSpeedControl = false;
    [SerializeField]
    private float minHorizontalSpeed = .5f;
    [SerializeField]
    private float maxHorizontalSpeed = 2f;
    [SerializeField]
    private float horizontalAcceleration = .2f;
    [SerializeField]
    private float maxVerticalMoveSpeed = 3f;

    private Vector2 velocity = new Vector2(1f, 0f);
    public Vector2 Velocity
    {
        get => velocity;
        private set
        {
            float horizontalVelocity = velocity.x;
            if (enableSpeedControl)
            {
                horizontalVelocity = Mathf.Clamp(value.x, minHorizontalSpeed, maxHorizontalSpeed);
            }
            float verticalVelocity = Mathf.Clamp(value.y, -maxVerticalMoveSpeed, maxVerticalMoveSpeed);
            rb2D.velocity = new Vector2(rb2D.velocity.x, verticalVelocity);
            velocity = new Vector2(horizontalVelocity, verticalVelocity);
        }
    }
    private Rigidbody2D rb2D;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Vector2 inputDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        float xVel = Velocity.x + inputDirection.x * horizontalAcceleration * Time.fixedDeltaTime;
        float yVel = inputDirection.y * maxVerticalMoveSpeed;
        Velocity = new Vector2(xVel, yVel);
    }
}
