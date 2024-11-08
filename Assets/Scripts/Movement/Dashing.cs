using UnityEngine;
using UnityEngine.InputSystem;

public class Dashing : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform playerCam;
    private Rigidbody rb;
    private PlayerMovementAdvanced pm;

    [Header("Dashing")]
    public float dashForce;
    public float dashUpwardForce;
    public float dashDuration;

    [Header("CameraEffects")]
    public PlayerCam cam;
    public float dashFov;

    [Header("Settings")]
    public bool useCameraForward = true;
    public bool allowAllDirections = true;
    public bool disableGravity = false;
    public bool resetVel = true;

    [Header("Cooldown")]
    public float dashCd;
    private float dashCdTimer;

    [Header("Input")]
    public InputActionReference moveInput;
    public InputActionReference dashInput;

    private Vector3 delayedForceToApply;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovementAdvanced>();
    }

    private void Update()
    {
        if (dashInput.action.WasPressedThisFrame() && dashCdTimer <= 0)
        {
            Dash();
        }

        if (dashCdTimer > 0)
        {
            dashCdTimer -= Time.deltaTime;
        }
    }

    private void Dash()
    {
        dashCdTimer = dashCd;
        pm.dashing = true;
        cam.DoFov(dashFov);

        Transform forwardT = useCameraForward ? playerCam : orientation;
        Vector3 direction = GetDirection(forwardT);
        delayedForceToApply = direction * dashForce + orientation.up * dashUpwardForce;

        if (disableGravity)
        {
            rb.useGravity = false;
        }

        delayedForceToApply.y = 0;
        Invoke(nameof(ApplyDashForce), 0.025f);
        Invoke(nameof(ResetDash), dashDuration);
    }

    private void ApplyDashForce()
    {
        if (resetVel)
        {
            rb.linearVelocity = Vector3.zero;
        }

        rb.AddForce(delayedForceToApply, ForceMode.Impulse);
    }

    private void ResetDash()
    {
        pm.dashing = false;
        cam.DoFov(85f);

        if (disableGravity)
        {
            rb.useGravity = true;
        }
    }

    private Vector3 GetDirection(Transform forwardT)
    {
        Vector2 input = moveInput.action.ReadValue<Vector2>();
        Vector3 direction = forwardT.forward * input.y + forwardT.right * input.x;

        if (input == Vector2.zero)
        {
            direction = forwardT.forward;
        }

        return direction.normalized;
    }
}