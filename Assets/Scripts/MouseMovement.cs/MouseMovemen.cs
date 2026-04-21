using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform cameraHolder;  // drag CameraHolder here in Inspector

    float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        xRotation = cameraHolder.localEulerAngles.x;

        // Convert from Unity's 0-360 range to -180 to 180
        if (xRotation > 180f) xRotation -= 360f;
    }

    void Update()
    {
        if (PauseMenu.Instance != null && PauseMenu.Instance.isPaused) return;
        if (SaveLoadUI.Instance != null && SaveLoadUI.Instance.isOpen) return;
         
        // rest of your mouse movement code

        
        
        bool menuOpen = InventorySystem.Instance.isOpen 
                     || CraftingSystem.Instance.isOpen;

        if (menuOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotate player left/right
        transform.Rotate(Vector3.up * mouseX);

        // Rotate camera up/down
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        cameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}