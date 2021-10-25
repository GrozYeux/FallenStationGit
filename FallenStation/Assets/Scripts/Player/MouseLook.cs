using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 500f;

    public Transform playerBody;

    float xRotation = 0f;

    void Start()
    {
        // Curseur invisible
        Cursor.lockState = CursorLockMode.Locked ;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Bloque la rotation entre -90 et 90°

        this.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        playerBody.Rotate(Vector3.up * mouseX);
    }

    public void Unlock()
    {
        Cursor.lockState = CursorLockMode.None;
    }
}
