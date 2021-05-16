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
    [SerializeField]
    private float minHeight = -4f;
    [SerializeField]
    private float maxHeight = 4f;
    [SerializeField]
    private float autoMoveMinDirectionChangeDelay = 1f;
    [SerializeField]
    private float autoMoveMaxDirectionChangeDelay = 5f;
    [SerializeField] [Range(0f, 1f)]
    private float autoMoveMinSteeringIntensity = .3f;
    [SerializeField] [Range(0f, 1f)]
    private float autoMoveMaxSteeringIntensity = 1f;

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
    private enum MoveDir { UNKNOWN, STRAIGHT, UP, DOWN };
    private bool isAutoPilotEnabled = false;
    public bool IsAutoPilotEnabled
    {
        get => isAutoPilotEnabled;
        set
        {
            bool wasAutoPilotEnabled = isAutoPilotEnabled;
            isAutoPilotEnabled = value;
            if (value && !wasAutoPilotEnabled)
            {
                StartCoroutine(AutoPilot());
            }
        }
    }

    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        PauseManager.Instance.OnPause += OnPause;
    }

    private void OnDisable()
    {
        if (PauseManager.Instance)
        {
            PauseManager.Instance.OnPause -= OnPause;
        }
    }

    private void OnPause(bool isPaused)
    {
        Velocity = Vector2.zero;
    }

    private void FixedUpdate()
    {
        if (!PauseManager.Instance.IsPaused)
        {
            Vector2 inputDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            if (inputDirection != Vector2.zero)
            {
                IsAutoPilotEnabled = false;
            }
            if (!isAutoPilotEnabled)
            {
                float xVel = Velocity.x + inputDirection.x * horizontalAcceleration * Time.fixedDeltaTime;
                float yVel = inputDirection.y * maxVerticalMoveSpeed;
                Velocity = new Vector2(xVel, yVel);
            }
            EnforceHeightLimits();
        }
    }

    private void EnforceHeightLimits()
    {
        bool goingBelowMinHeight = (transform.position.y <= minHeight) && (Velocity.y < 0f);
        bool goingAboveMaxHeight = (transform.position.y >= maxHeight) && (Velocity.y > 0f);
        if (goingBelowMinHeight || goingAboveMaxHeight)
        {
            Velocity = new Vector2(Velocity.x, 0f);
        }
    }

    private IEnumerator AutoPilot()
    {
        while (isAutoPilotEnabled && !PauseManager.Instance.IsPaused)
        {
            // Determine current move direction
            MoveDir currentMoveDir = MoveDir.UNKNOWN;
            if (Velocity.y < 0f)
            {
                currentMoveDir = MoveDir.DOWN;
            }
            else if (Velocity.y > 0f)
            {
                currentMoveDir = MoveDir.UP;
            }
            else
            {
                currentMoveDir = MoveDir.STRAIGHT;
            }

            // Determine next move
            MoveDir nextMoveDir = MoveDir.UNKNOWN;
            bool atMinHeight = (transform.position.y <= minHeight + .1f);
            if (atMinHeight)
            {
                nextMoveDir = MoveDir.UP;
            }
            bool atMaxHeight = (transform.position.y >= maxHeight - .1f);
            if (atMaxHeight)
            {
                nextMoveDir = MoveDir.DOWN;
            }
            if (nextMoveDir == MoveDir.UNKNOWN)
            {
                switch (currentMoveDir)
                {
                    case MoveDir.UNKNOWN:
                        Debug.LogError("Submarine auto pilot failed to find current move direction!");
                        break;
                    case MoveDir.STRAIGHT:
                        nextMoveDir = Random.value > 0.5f ? MoveDir.UP : MoveDir.DOWN;
                        break;
                    case MoveDir.UP:
                        nextMoveDir = Random.value > 0.5f ? MoveDir.STRAIGHT : MoveDir.DOWN;
                        break;
                    case MoveDir.DOWN:
                        nextMoveDir = Random.value > 0.5f ? MoveDir.STRAIGHT : MoveDir.UP;  
                        break;
                }
            }

            // Change direction
            float verticalMoveInput = 0f;
            switch (nextMoveDir)
            {
                case MoveDir.UNKNOWN:
                    Debug.LogError("Submarine auto pilot failed to find next move direction!");
                    break;
                case MoveDir.STRAIGHT:
                    verticalMoveInput = 0f;
                    break;
                case MoveDir.UP:
                    verticalMoveInput = Random.Range(autoMoveMinSteeringIntensity, autoMoveMaxSteeringIntensity);
                    break;
                case MoveDir.DOWN:
                    verticalMoveInput = -Random.Range(autoMoveMinSteeringIntensity, autoMoveMaxSteeringIntensity);  
                    break;
            }
            Velocity = new Vector2(Velocity.x, verticalMoveInput * maxVerticalMoveSpeed);

            // Wait for a few seconds
            float timeUntilDirectionChange = Random.Range(autoMoveMinDirectionChangeDelay, autoMoveMaxDirectionChangeDelay);
            yield return new WaitForSeconds(timeUntilDirectionChange);
        }
        IsAutoPilotEnabled = false;
    }
}
