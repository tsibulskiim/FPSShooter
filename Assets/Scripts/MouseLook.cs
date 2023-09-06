using UnityEngine;

public class MouseLook : MonoBehaviour
{

    public float mouseSensitivity = 100f;
    public Transform playerBody;

    private float xRotation;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        //Debug.Log($"x: {mouseX}; y: {mouseY};");

        xRotation -= mouseY * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -80, 80);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        playerBody.Rotate(mouseX * Time.deltaTime * Vector3.up);

    }
}
