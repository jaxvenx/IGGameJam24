using UnityEngine;
using UnityEngine.InputSystem;

public class WallRunningAdvanced : MonoBehaviour
{
    [Header("Wallrunning")]
    public LayerMask whatIsWall;
    public LayerMask whatIsGround;
    public float wallRunForce;
    public float wallJumpUpForce;
    public float wallJumpSideForce;
    public float wallClimbSpeed;
    public float maxWallRunTime;
    private float wallRunTimer;

    [Header("Input")]
    public InputActionReference moveInput;
    public InputActionReference jumpInput;
    public InputActionReference upwardsRunInput;
    public InputActionReference downwardsRunInput;

    private bool upwardsRunning;
    private bool downwardsRunning;
    private Vector2 moveDirection;

    [Header("Detection")]
    public float wallCheckDistance;
    public float minJumpHeight;
    private bool wallLeft;
    private bool wallRight;
    private RaycastHit wallHitInfo;
    private RaycastHit leftWallhit;
    private RaycastHit rightWallhit;

    [Header("Exiting")]
    private bool exitingWall;
    public float exitWallTime;
    private float exitWallTimer;

    [Header("Gravity")]
    public bool useGravity;
    public float gravityCounterForce;

    [Header("References")]
    public Transform orientation;
    public PlayerCam cam;
    private PlayerMovementAdvanced pm;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovementAdvanced>();
    }

    private void Update()
    {
        ProcessInput();
        CheckForWall();
        HandleWallRunState();
    }

    private void FixedUpdate()
    {
        if (pm.wallrunning)
            ApplyWallRunningMovement();
    }

    private void ProcessInput()
    {
        moveDirection = moveInput.action.ReadValue<Vector2>();
        upwardsRunning = upwardsRunInput.action.ReadValue<float>() > 0;
        downwardsRunning = downwardsRunInput.action.ReadValue<float>() > 0;
    }

    private void CheckForWall()
    {
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallhit, wallCheckDistance, whatIsWall);
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallhit, wallCheckDistance, whatIsWall);
    }

    private bool IsAboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, whatIsGround);
    }

    private void HandleWallRunState()
    {
        if ((wallLeft || wallRight) && moveDirection.y > 0 && IsAboveGround() && !exitingWall)
        {
            if (!pm.wallrunning)
                StartWallRun();

            wallRunTimer -= Time.deltaTime;
            if (wallRunTimer <= 0)
                BeginWallExit();

            if (jumpInput.action.WasPressedThisFrame())
                PerformWallJump();
        }
        else if (exitingWall)
        {
            exitWallTimer -= Time.deltaTime;
            if (exitWallTimer <= 0)
                exitingWall = false;
        }
        else if (pm.wallrunning)
        {
            StopWallRun();
        }
    }

    private void StartWallRun()
    {
        pm.wallrunning = true;
        wallRunTimer = maxWallRunTime;

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        ApplyCameraEffects(90f, wallLeft ? -5f : 5f);
    }

    private void ApplyWallRunningMovement()
    {
        rb.useGravity = useGravity;

        // Determine the wall's normal based on which side the player is wall-running on
        Vector3 wallNormal = wallRight ? rightWallhit.normal : leftWallhit.normal;

        // Calculate wallForward consistently based on wall side and player orientation
        Vector3 wallForward = Vector3.Cross(wallNormal, Vector3.up);
        if (Vector3.Dot(wallForward, orientation.forward) < 0)
        {
            wallForward = -wallForward;
        }

        // Apply forward force along the wall's forward direction
        rb.AddForce(wallForward * wallRunForce, ForceMode.Force);

        // Apply vertical force if climbing up or down
        if (upwardsRunning)
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, wallClimbSpeed, rb.linearVelocity.z);
        else if (downwardsRunning)
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, -wallClimbSpeed, rb.linearVelocity.z);

        // Apply a force that keeps the player pushed against the wall
        if (!(wallLeft && moveDirection.x > 0) && !(wallRight && moveDirection.x < 0))
            rb.AddForce(-wallNormal * 100, ForceMode.Force);

        // Counteract gravity while wall-running
        if (useGravity)
            rb.AddForce(Vector3.up * gravityCounterForce, ForceMode.Force);
    }

    private void StopWallRun()
    {
        pm.wallrunning = false;
        ApplyCameraEffects(80f, 0f);
    }

    private void PerformWallJump()
    {
        // Begin exiting wall state
        exitingWall = true;
        exitWallTimer = exitWallTime;

        // Determine the correct wall normal based on which wall the player is on
        Vector3 wallNormal = wallRight ? rightWallhit.normal : leftWallhit.normal;
        Vector3 jumpDirection = Vector3.up * wallJumpUpForce + wallNormal;

        // Reset y velocity and add force in jump direction
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(jumpDirection, ForceMode.Impulse);

        pm.canDoubleJump = true;
    }


    private void ApplyCameraEffects(float fov, float tilt)
    {
        cam.DoFov(fov);
        cam.DoTilt(tilt);
    }

    private void BeginWallExit()
    {
        exitingWall = true;
        exitWallTimer = exitWallTime;
        StopWallRun();
    }
}
