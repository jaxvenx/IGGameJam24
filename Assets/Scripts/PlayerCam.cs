using PrimeTween;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCam : MonoBehaviour
{
    public InputActionReference mouseInput;

    public float sensX;
    public float sensY;

    public Transform orientation;
    public Transform camHolder;

    float xRotation;
    float yRotation;

    private Camera playerCamera;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerCamera = GetComponent<Camera>();
    }

    private void Update()
    {
        //get mouse input
        float mouseX = mouseInput.action.ReadValue<Vector2>().x * Time.deltaTime * sensX;
        float mouseY = mouseInput.action.ReadValue<Vector2>().y * Time.deltaTime * sensY;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // rotate cam and orientation
        camHolder.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    public void DoFov(float endValue)
    {
        Tween.CameraFieldOfView(playerCamera, endValue, 0.25f);
    }

    public void DoTilt(float zTilt)
    {
        //Tilt the camHolder on the z axis
        Tween.LocalRotation(playerCamera.gameObject.transform, new Vector3(0, 0, zTilt), 0.25f);
    }
}