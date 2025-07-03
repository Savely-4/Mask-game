using UnityEngine;
using UnityEngine.InputSystem;

public class DEBUG_CameraControl : MonoBehaviour
{
    private InputAction mouseLook;

    [SerializeField] private Transform horizontalTurnTransform;
    [SerializeField] private Transform verticalTurnTransform;
    [SerializeField] private float sensitivity = 0.1f;


    void Awake()
    {
        mouseLook = InputSystem.actions.FindAction("Look");

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        LookAround();
    }



    void LookAround()
    {
        var delta = mouseLook.ReadValue<Vector2>() * sensitivity;
        var verticalAngle = verticalTurnTransform.eulerAngles.x - delta.y;

        verticalTurnTransform.localRotation = Quaternion.Euler(verticalAngle, 0f, 0f);
        horizontalTurnTransform.Rotate(Vector3.up * delta.x);
    }
}
