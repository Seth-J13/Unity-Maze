using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI.Table;

public class FirstPersonMovement : MonoBehaviour
{
    CharacterController controller;
    Camera mainCamera;
    public float walkspeed = 2;
    public float turnSpeed = 3;
    Vector2 movementVector;
    private float verticalLookAngle;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        mainCamera = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void OnSprint(InputValue v)
    {

    }

    void OnMove(InputValue v)
    {
        movementVector = v.Get<Vector2>();

    }

    public void ResetMovement()
    {
        movementVector = Vector2.zero;
    }


    void OnLook(InputValue v)
    {
        Vector2 lookVector = v.Get<Vector2>();
        transform.Rotate(Vector3.up, lookVector.x * turnSpeed * Time.deltaTime);
        //mainCamera.transform.Rotate(Vector3.right, lookVector.y * turnSpeed * Time.deltaTime);
        verticalLookAngle = Mathf.Clamp(verticalLookAngle - lookVector.y * turnSpeed * Time.deltaTime, -85, 85);
        Vector3 angles = mainCamera.transform.eulerAngles;
        mainCamera.transform.eulerAngles = new Vector3(verticalLookAngle, angles.y, angles.z);
    }

    void Update()
    {
        //Quaternion rot = Quaternion.FromToRotation(new Vector3(movementVector.x, 0, movementVector.y), transform.forward);
        Quaternion rot = Quaternion.FromToRotation(Vector3.forward, transform.forward);
        controller.Move(rot * new Vector3(movementVector.x, 0, movementVector.y) * walkspeed * Time.deltaTime);
        //Walk(rot);
        //transform.Rotate(Vector3.up, movementVector.x * turnSpeed * Time.deltaTime);
    }

    public void Walk(Quaternion rot)
    {
        controller.Move(rot * new Vector3(movementVector.x, 0, movementVector.y) * walkspeed * Time.deltaTime);
    }
}
