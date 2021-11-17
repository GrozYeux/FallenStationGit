using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public Transform playerBody;

    [Header("Look Parameters")]
    [SerializeField, Range(1, 10)] private float lookSpeedX = 2.0f;
    [SerializeField, Range(1, 10)] private float lookSpeedY = 2.0f;
    [SerializeField, Range(1, 90)] private float upperLookLimit = 80.0f;
    [SerializeField, Range(1, 90)] private float lowerLookLimit = 80.0f;
    private float rotationX = 0;

    void Start()
    {
        // Curseur invisible
        Cursor.lockState = CursorLockMode.Locked ;
    }

    void Update()
    {
        HandleMouseLook();
    }

    private void HandleMouseLook()
    {
        rotationX -= Input.GetAxis("Mouse Y") * lookSpeedY;
        rotationX = Mathf.Clamp(rotationX, -upperLookLimit, lowerLookLimit);

        transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        playerBody.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeedX, 0);
    }

    public void Unlock()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public void OnAwake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
}
