using UnityEngine;
using UnityEngine.InputSystem;

public class CameraLookController : MonoBehaviour
{
    [Header("Look range in degrees")]
    public float maxYaw = 30f;
    public float maxPitch = 15f;

    [Header("1 = full screen maps to the look range, lower = more of the screen stays centered")]
    [Range(0.3f, 1f)]
    public float sensitivity = 0.9f;

    public float turnSpeed = 5f;

    Quaternion baseRotation;

    void Awake()
    {
        baseRotation = transform.localRotation;
    }

    void Update()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        float nx = Mathf.Clamp((mousePos.x / Screen.width - 0.5f) * 2f, -1f, 1f);
        float ny = Mathf.Clamp((mousePos.y / Screen.height - 0.5f) * 2f, -1f, 1f);

        float yaw = nx * sensitivity * maxYaw;
        float pitch = ny * sensitivity * maxPitch;

        Quaternion targetRot = baseRotation * Quaternion.Euler(-pitch, yaw, 0f);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRot, Time.deltaTime * turnSpeed);
    }
}
